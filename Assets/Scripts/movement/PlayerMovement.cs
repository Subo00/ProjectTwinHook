using TwinHookController;
using System;
using UnityEngine;


namespace TwinHookController
{

    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private Stats stats;
        private Rigidbody rb;
        private CapsuleCollider playerCollider;
        private FrameInput frameInput;
        private Vector3 frameVelocity; //2d -> 3d

        #region Interface

        public Vector3 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float time;


        private float horizontal1;
        private bool isFacingRight = true;
        public bool isFrozen = false;
        public bool activeGrapple = false;

        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        public MovementState state;
        public enum MovementState
        {
            freeze,
            walking,
            grappling,
            air
        }

        public void StateHandler()
        {

        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            playerCollider = GetComponent<CapsuleCollider>();

            rb.centerOfMass = new Vector3(0, -transform.localScale.y / 2, 0);
        }

        // Update is called once per frame
        void Update()
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            time += Time.deltaTime;
            gatherInput();

        }

        private bool grappling = false;
        private void gatherInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
                GrappleDown = Input.GetButtonDown("Grapple"),
                Move = new Vector3(Input.GetAxisRaw("Horizontal 1"), Input.GetAxisRaw("Vertical 1"))
            };

            if (stats.snapInput)
            {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < stats.horizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < stats.verticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.GrappleDown)
            {
                grappling = !grappling;
            }

            if (frameInput.JumpDown)
            {
                Debug.Log("Jump pressed and touching ground = " + grounded);
                jumpToConsume = true;
                timeJumpWasPressed = time;
            }
        }

        private void FixedUpdate()
        {

            checkCollisions();
            handleJump();
            handleDirection();
            handleGravity();

            applyMovement();

            flip(); // really need to overdo this shit
        }

        float yRotation = 0f;
        private void flip()
        {
            // Use frameInput.Move.x instead of horizontal1 if horizontal1 isn’t being updated
            float horizontal = frameInput.Move.x;
            if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
            {
                isFacingRight = !isFacingRight;
                // Rotate around the Y-axis instead of scaling
                yRotation = isFacingRight ? 0f : 180f;
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }

        #region grappling
        private Vector3 grapplingVelocity;

        public void jumpToPosition(Vector3 targetPosition, float trajectoryHeight)
        {
            activeGrapple = true;
            grapplingVelocity = calculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
            Invoke(nameof(applyMovement), 0.1f);
        }




        public Vector3 calculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Physics.gravity.y;
            float displacementY = endPoint.y - startPoint.y;
            float displacementX = endPoint.x - startPoint.x;

            if (Mathf.Approximately(displacementX, 0f))
            {
                displacementX = 1f;
            }
            Debug.Log("displacementX: " + displacementX);
            float timeToApex = Mathf.Sqrt(-2 * trajectoryHeight / gravity);
            float timeFromApex = Mathf.Sqrt(2 * displacementY - trajectoryHeight / -gravity);
            float flightTime = timeToApex + timeFromApex;

            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
            Vector3 velocityX = Vector3.right * displacementX / flightTime;

            return velocityX + velocityY;
        }
        #endregion

        #region Collisions

        private float frameLeftGrounded = float.MinValue;
        private bool grounded;

        private void checkCollisions()
        {
            // Assume _col is your CapsuleCollider (3D)
     
            Vector3 center = playerCollider.bounds.center;

            // Calculate the effective segment length along the playerCollider's height.
            // For a vertically oriented playerCollider, the segment length is (height/2 - radius).
            float radius = playerCollider.radius;
            float segment = Mathf.Max(0, playerCollider.height * 0.5f - radius);
            Vector3 up = transform.up; // assuming "up" is the capsule's local up direction
            Vector3 point1 = center + up * segment;
            Vector3 point2 = center - up * segment;

            // Perform capsule casts in 3D.
            // Use a layer mask that excludes the player’s own layer.
            // Note: Unlike 2D, 3D raycasts always report a hit if starting inside a collider.
            // However, if your player's collider is on _stats.PlayerLayer and you are casting with ~_stats.PlayerLayer,
            // then your own collider will be automatically ignored.
            bool groundHit = Physics.CapsuleCast(point1, point2, radius, Vector3.down, stats.grounderDistance, stats.playerLayer, QueryTriggerInteraction.Ignore);
            bool ceilingHit = Physics.CapsuleCast(point1, point2, radius, Vector3.up, stats.grounderDistance, stats.playerLayer, QueryTriggerInteraction.Ignore);

            // If a ceiling is hit, adjust the vertical velocity.
            if (ceilingHit)
                frameVelocity.y = Mathf.Min(0, frameVelocity.y);

            // Ground collision: if we just landed, or we left the ground.
            if (!grounded && groundHit)
            {
                grounded = true;
                coyoteUsable = true;
                bufferedJumpUsable = true;
                endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
            }
            else if (grounded && !groundHit)
            {
                grounded = false;
                frameLeftGrounded = time;
                GroundedChanged?.Invoke(false, 0);
            }
        }

        #endregion

        #region Jumping

        private bool jumpToConsume;
        private bool bufferedJumpUsable;
        private bool endedJumpEarly;
        private bool coyoteUsable;
        private float timeJumpWasPressed;

        private bool HasBufferedJump => bufferedJumpUsable && time < timeJumpWasPressed + stats.jumpBuffer;
        private bool CanUseCoyote => coyoteUsable && !grounded && time < frameLeftGrounded + stats.coyoteTime;

        private void handleJump()
        {
            if (!endedJumpEarly && !grounded && !frameInput.JumpHeld && rb.velocity.y > 0) endedJumpEarly = true;

            if (!jumpToConsume && !HasBufferedJump) return;

            if (grounded || CanUseCoyote)
            {
                executeJump();
                Debug.Log("executed jump"); 
            }

            jumpToConsume = false;
        }

        private void executeJump()
        {
            endedJumpEarly = false;
            timeJumpWasPressed = 0;
            bufferedJumpUsable = false;
            coyoteUsable = false;
            frameVelocity.y = stats.jumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void handleDirection()
        {
            if (frameInput.Move.x == 0)
            {
                var deceleration = grounded ? stats.groundDeceleration : stats.airDeceleration;
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                frameVelocity.x = Mathf.MoveTowards(frameVelocity.x, frameInput.Move.x * stats.maxSpeed, stats.acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void handleGravity()
        {
            if (grounded && frameVelocity.y <= 0f)
            {
                frameVelocity.y = stats.groundingForce;
            }
            else
            {
                var inAirGravity = stats.fallAcceleration;
                if (endedJumpEarly && frameVelocity.y > 0) inAirGravity *= stats.jumpEndEarlyGravityModifier;
                frameVelocity.y = Mathf.MoveTowards(frameVelocity.y, -stats.maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void applyMovement()
        {
            if (activeGrapple)
            {
                rb.velocity = grapplingVelocity;
            }
            else rb.velocity = frameVelocity;


        }


#if UNITY_EDITOR  //wtf is this doing? Just a warning i guess?
        private void OnValidate()
        {
            if (stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif

    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool GrappleDown;
        public Vector3 Move;
    }

    public interface IPlayerMovement
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector3 FrameInput { get; }
    }
}


