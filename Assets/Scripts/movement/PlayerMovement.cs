using TwinHookController;
using System;
using UnityEngine;
using System.Collections;


namespace TwinHookController
{

    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] public Stats stats;
        [SerializeField] private GrapplingHook grapplingHook;

        [SerializeField] private GameObject duckedObject;
        //grappleAnchor
        [SerializeField] private GameObject anchorPrefab;
        private GameObject activeAnchor;

        private Rigidbody rb;
        private CapsuleCollider playerCollider;
        private FrameInput frameInput;
        private Vector3 frameVelocity; //2d -> 3d
        private Animator animator;
        #region Interface

        public Vector3 FrameInput => frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;
        public bool standStill = false;

        #endregion

        private float time;

        private bool isFacingRight = true;
        public bool isFrozen = false;
        public bool activeGrapple = false;
        public bool activeGrappleJustEnded = false;
        public Transform grapplePoint;

        private DialogueManager dialogueManager;


        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private bool DebugMode;



        [SerializeField] protected string horizontal = "Horizontal 1";
        [SerializeField] protected string jump = "Jump 1";
        [SerializeField] protected string duck = "Duck 1";
        [SerializeField] public string grapple = "Grapple 1";

        //   public MovementState state;
        //  public enum MovementState
        //  {
        //      freeze,
        //      walking,
        //      grappling,
        //      air
        //  }

        //  public void StateHandler()
        //  {

        //  }


        protected virtual void Start()
        {
            if (stats == null) {
                stats = Resources.Load<Stats>("movementStats");
                if (stats == null) {
                    Debug.LogError("Stats asset not found! Make sure it's at Resources/Stats.cs");
                }
            }
            if (grapplingHook == null) {
                grapplingHook = GetComponent<GrapplingHook>();
                if (grapplingHook == null) {
                    Debug.LogError("GrapplinHook script not found! Make sure it's on the Player");
                }
            }

            animator = GetComponentInChildren<Animator>();
            if (animator == null) {
                Debug.LogError("Animator is missing! Attache it to the model");
            }

            if (!DebugMode)
            {
                dialogueManager = DialogueManager.Instance;
                if (dialogueManager == null)
                {
                    Debug.LogError("DialogueManager is missing in the scene");
                }
            }
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

            if (grounded && getGroundNormal(out var normal))
            {
                Debug.DrawRay(transform.position, normal, Color.green);
                Debug.Log("groundNormals being displayed");
            }



            if (!DebugMode)
            {
                if (!dialogueManager.dialogueIsPlaying)
                {
                    rb.constraints = RigidbodyConstraints.None;
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                    time += Time.deltaTime;
                    if (activeGrappleJustEnded)
                    {
                        activeGrappleJustEnded = false;
                        StartCoroutine(KeepGrapplingMomentum(stats.grappleMomentumTimer));
                    }
                    gatherInput();
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; //in theory this should still be on? but in practice they. are not sometimes
                }
            }

            if(frameInput.Move.x == 0) {
                animator.SetBool("isRunning", false);
            }
            else  {
                animator.SetBool("isRunning", true);
            }

                
        }

        private void gatherInput()
        {
            frameInput = new FrameInput
            {
                JumpDown = Input.GetButtonDown(jump),
                JumpHeld = Input.GetButton(jump),
                DuckHeld = Input.GetButton(duck),
                DuckReleased = Input.GetButtonUp(duck),
                GrappleDown = Input.GetButtonDown(grapple),
                Move = new Vector3(Input.GetAxisRaw(horizontal), 0)
            };

            if (stats.snapInput) {
                frameInput.Move.x = Mathf.Abs(frameInput.Move.x) < stats.horizontalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.x);
                frameInput.Move.y = Mathf.Abs(frameInput.Move.y) < stats.verticalDeadZoneThreshold ? 0 : Mathf.Sign(frameInput.Move.y);
            }

            if (frameInput.JumpDown)
            {
                jumpToConsume = true;
                timeJumpWasPressed = time;
            }

            if (frameInput.DuckHeld)
            {
                standStill = true;
            }
            if (frameInput.DuckReleased)
            {
                standStill = false;
            }

        }


        private void FixedUpdate()
        {
            if (DebugMode)
            {
                checkCollisions();
                handleJump();
                handleDirection();
                handleGravity();

                applyMovement();

                flip(); // really need to overdo this shit

                //If grappling and close to the target, stop grappling
                if (activeGrapple && Vector3.Distance(transform.position, grapplePoint.position) < stats.stopGrapplingAnchorDistance)
                {
                    Debug.Log("grappleStop by proximity");
                    grapplingHook.lineRenderer.enabled = false;  // Let momentum carry you 
                }

                // Optional: Lock to 2D axis
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            }
            else
            {
                if (!dialogueManager.dialogueIsPlaying)
                {
                    checkCollisions();
                    handleJump();
                    handleDirection();
                    handleGravity();

                    applyMovement();

                    flip(); // really need to overdo this shit

                    //If grappling and close to the target, stop grappling
                    if (activeGrapple && Vector3.Distance(transform.position, grapplePoint.position) < stats.stopGrapplingAnchorDistance)
                    {
                        Debug.Log("grappleStop by proximity");
                        grapplingHook.lineRenderer.enabled = false;  // Let momentum carry you 
                    }

                    // Optional: Lock to 2D axis
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                }
            }
            
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

        #region Grappling
        private Vector3 grapplingVelocity;
        private Vector3 lastGrapplingVelocity;


        public void jumpToPosition(Vector3 targetPosition, float trajectoryHeight)
        {
            activeGrapple = true;

            // Calculate launch velocity
            grapplingVelocity = calculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
            lastGrapplingVelocity = grapplingVelocity;
            rb.velocity = grapplingVelocity;
        }

        public Vector3 calculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
        {
            float gravity = stats.fallAcceleration;

            // Vertical velocity to reach arc
            float verticalVelocity = Mathf.Sqrt(2 * gravity * trajectoryHeight);
            float timeToApex = verticalVelocity / gravity;

            float deltaY = endPoint.y - startPoint.y;
            float timeFromApex = Mathf.Sqrt(2 * Mathf.Abs(trajectoryHeight - deltaY) / gravity);
            float totalTime = timeToApex + timeFromApex;

            float deltaX = endPoint.x - startPoint.x;
            float horizontalVelocity = deltaX / totalTime;

            Debug.Log("the grappling velocity: " + new Vector3(horizontalVelocity *2, verticalVelocity, 0)); //*2 because we want to overshoot this
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
            // Assume not grounded by default at the start of each physics frame
            grounded = false;

            // Cast origin slightly above player center to avoid starting inside geometry
            Vector3 origin = transform.position + Vector3.up * 0.1f;

            // How far downward to check for ground
            float rayLength = stats.grounderDistance;

            // Perform a raycast straight down to detect the ground
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundLayer))
            {
                // Measure angle between the surface normal and straight "up"
                float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

                // If the surface is not too steep (angle within limit), we count it as ground
                if (slopeAngle <= stats.maxSlope)
                {
                    grounded = true;

                    // Only trigger landing behavior once when transitioning from air → ground
                    if (!wasGroundedLastFrame)
                    {
                        coyoteUsable = true;           // Allow jump buffering/coyote time
                        bufferedJumpUsable = true;
                        endedJumpEarly = false;

                        // Notify listeners (e.g. animations, VFX)
                        GroundedChanged?.Invoke(true, Mathf.Abs(frameVelocity.y));
                    }
                }
            }

            // If we were grounded last frame but no longer are, record the time we left
            if (!grounded && wasGroundedLastFrame)
            {
                frameLeftGrounded = time;
                GroundedChanged?.Invoke(false, 0);
            }

            // Update memory of last frame's grounded status
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
            if(isFrozen) return;
            if (grounded && frameVelocity.y <= 0f)
            {
                frameVelocity.y = stats.groundingForce;
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

            }
        }




        #endregion

        private void applyMovement()
        {
            if (standStill && grounded)
            {
                rb.velocity = Vector3.zero;

                if (activeAnchor == null)
                {
                    Vector3 spawnPosition = transform.position;
                    activeAnchor = Instantiate(anchorPrefab, spawnPosition, Quaternion.identity);
                    duckedObject.SetActive(true);
                    playerCollider.enabled = false;
                }

                return;
            }

            if (activeAnchor != null)
            {
                Destroy(activeAnchor);
                duckedObject.SetActive(false);
                playerCollider.enabled = true;
                activeAnchor = null;
            }

            if (isFrozen)
            {
                rb.velocity = Vector3.zero;
                frameVelocity = Vector3.zero;
                return;
            }

            Vector3 finalVelocity = Vector3.zero;

            // Get current input-based horizontal movement
            Vector3 horizontalMove = new Vector3(frameVelocity.x, 0f, 0f);

            // If grounded, adjust horizontal movement to follow the slope
            if (grounded && getGroundNormal(out Vector3 groundNormal))
            {
                float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

                if (slopeAngle > stats.maxSlope)
                {
                    horizontalMove = Vector3.zero; // too steep — cancel input
                }
                else
                {
                    // Project the input direction along the slope surface
                    horizontalMove = Vector3.ProjectOnPlane(horizontalMove, groundNormal);
                }
            }

            // Build final velocity
            finalVelocity += horizontalMove;
            finalVelocity.y = frameVelocity.y; // vertical handled via jump/gravity
            finalVelocity += grapplingVelocity; // add any current grapple momentum

            // Apply to rigidbody
            rb.velocity = finalVelocity;
        }




        private bool getGroundNormal(out Vector3 normal)
        {
            RaycastHit hit;
            float castDistance = 0.2f;
            Vector3 origin = transform.position + Vector3.up * 0.1f; // Slightly above player center

            // Raycast downward to detect the ground beneath the player
            if (Physics.Raycast(origin, Vector3.down, out hit, castDistance, groundLayer))
            {
                normal = hit.normal; // return the surface normal (used for slope angle)
                return true;
            }

            // If nothing hit, default to "flat ground"
            normal = Vector3.up;
            return false;
        }






        private IEnumerator KeepGrapplingMomentum(float duration)
        {
            float elapsed = 0f;
            if (activeGrapple)
            {
                yield return null;
            }

            if (!grounded)
            {
                while (elapsed < duration)
                {
                    float t = elapsed / duration;
                    grapplingVelocity = Vector3.Lerp(lastGrapplingVelocity, Vector3.zero, t);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }
            lastGrapplingVelocity = Vector3.zero;
            grapplingVelocity = Vector3.zero;
        }




#if UNITY_EDITOR  
        private void OnValidate()
        {
            if (stats == null) Debug.LogWarning("Please assign a Stats asset to the Player Controller's Stats slot", this);
        }
#endif

    }

    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public bool GrappleDown;
        public bool DuckHeld;
        public bool DuckReleased;
        public Vector3 Move;
    }

    public interface IPlayerMovement
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector3 FrameInput { get; }
    }
}


