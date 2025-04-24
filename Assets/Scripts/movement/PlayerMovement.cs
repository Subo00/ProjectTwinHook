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
        public Transform grapplePoint;


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

            if (frameInput.JumpDown)
            {
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

            // If grappling and close to the target, stop grappling
            if (activeGrapple && Vector3.Distance(transform.position, grapplePoint.position) < 1f)
            {
                activeGrapple = false;  // Let momentum carry you
            }

            // Optional: Lock to 2D axis
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
        }

        public Vector3 calculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = Mathf.Abs(Physics.gravity.y);

            float verticalVelocity = Mathf.Sqrt(2 * gravity * trajectoryHeight);
            float timeToApex = verticalVelocity / gravity;

            float deltaY = endPoint.y - startPoint.y;
            float timeFromApex = Mathf.Sqrt(2 * Mathf.Abs(trajectoryHeight - deltaY) / gravity);
            float totalTime = timeToApex + timeFromApex;

            float deltaX = endPoint.x - startPoint.x;
            float horizontalVelocity = deltaX / totalTime;

            return new Vector3(horizontalVelocity, verticalVelocity, 0);
        }


        #endregion

        #region Collisions

        private float frameLeftGrounded = float.MinValue;
        private bool grounded;

        private bool wasGroundedLastFrame;


        // testing the damn ground controll
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (!col) return;

            Vector3 center = col.bounds.center;
            float radius = col.radius;
            float height = col.height;
            float castDistance = 0.01f; // How far below the capsule to check for ground

            Vector3 point1 = center + Vector3.up * (height / 2 - radius);
            Vector3 point2 = center + Vector3.down * ((height / 2 - radius) + castDistance);


            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(point1, radius);
            Gizmos.DrawWireSphere(point2, radius);
            Gizmos.DrawLine(point1 + Vector3.left * radius, point2 + Vector3.left * radius);
            Gizmos.DrawLine(point1 + Vector3.right * radius, point2 + Vector3.right * radius);
#endif
        }





        private void checkCollisions()
        {
            Vector3 center = playerCollider.bounds.center;
            float radius = playerCollider.radius * 0.9f; // Slightly shrink radius to prevent edge misses
            float height = playerCollider.height;
            float castDistance = 0.01f; // How far below the capsule to check for ground

            Vector3 point1 = center + Vector3.up * (height / 2 - radius);
            Vector3 point2 = center + Vector3.down * ((height / 2 - radius) + castDistance);


            grounded = Physics.CheckCapsule(point1, point2, radius, stats.groundLayer, QueryTriggerInteraction.Ignore);

            if (grounded)
            {
                if (!wasGroundedLastFrame)
                {
                    coyoteUsable = true;
                    bufferedJumpUsable = true;
                    endedJumpEarly = false;
                    GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
                }
            }
            else
            {
                if (wasGroundedLastFrame)
                {
                    frameLeftGrounded = time;
                    GroundedChanged?.Invoke(false, 0);
                }
            }

            wasGroundedLastFrame = grounded;
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
            // Cut jump short if released early
            if (!endedJumpEarly && !grounded && !frameInput.JumpHeld && rb.velocity.y > 0)
            {
                endedJumpEarly = true;
            }

            // Only try to jump if there's a valid press
            if (!jumpToConsume && !HasBufferedJump) return;

            // Must be on ground or within coyote time
            if (grounded || CanUseCoyote)
            {
                executeJump();
            }

            // Always clear input consumption flag
            jumpToConsume = false;
        }


        private void executeJump()
        {
            endedJumpEarly = false;
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
                Debug.Log("Grounded → grounding force applied: " + stats.groundingForce);
            }
            else
            {
                float gravity = stats.fallAcceleration;
                if (endedJumpEarly && frameVelocity.y > 0)
                    gravity *= stats.jumpEndEarlyGravityModifier;

                frameVelocity.y += -gravity * Time.fixedDeltaTime;

                // Clamp to max fall speed
                if (frameVelocity.y < -stats.maxFallSpeed)
                    frameVelocity.y = -stats.maxFallSpeed;

                Debug.Log("Falling → velocity: " + frameVelocity.y);
            }
        }




        #endregion

        private void applyMovement()
        {
            if (activeGrapple)
            {
                Debug.Log("Applying grapple velocity: " + grapplingVelocity);
                rb.velocity = grapplingVelocity;
            }
            else
            {
                Debug.Log("Applying frame velocity: " + frameVelocity);
                rb.velocity = frameVelocity;
            }
        }




#if UNITY_EDITOR  
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


