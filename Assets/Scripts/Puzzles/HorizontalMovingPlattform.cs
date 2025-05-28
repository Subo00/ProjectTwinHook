using UnityEngine;

/// <summary>
/// A physics-based platform that moves between two positions and optionally rotates.
/// Inherits from MovingPlatform to support activation from buttons or levers,
/// but overrides the movement logic to use Rigidbody for better player interaction.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HorizontalMovingPlatform : MovingPlatform
{
    // Rigidbody used for physics-based movement
    private Rigidbody rb;

    // Tracks velocity to affect player movement when standing on the platform
    private Vector3 linearVelocity;

    // Used to compute velocity deltas each frame
    private Vector3 previousPosition;

    /// <summary>
    /// Setup the Rigidbody.
    /// </summary>
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    /// <summary>
    /// Initialize start position and rotation.
    /// Adjust end position if it's relative.
    /// </summary>
    protected void Start()
    {
        base.startPos = transform.position;
        base.startRot = transform.rotation;

        if (isButtonPusher || isLever)
        {
            endPos += startPos; // convert to world-space end position
        }

        previousPosition = transform.position;
    }

    /// <summary>
    /// Move the platform and update velocity each frame.
    /// </summary>
    protected void Update()
    {
        // Determine current target based on activation state
        Vector3 targetPos = buttonActive ? endPos : startPos;

        // --- POSITIONAL MOVEMENT ---
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
        linearVelocity = (newPosition - transform.position) / Time.fixedDeltaTime;
        rb.MovePosition(newPosition);


        // Update previous transform state
        previousPosition = transform.position;
    }

    /// <summary>
    /// Set the platform active or inactive.
    /// This is called by external triggers like buttons or levers.
    /// </summary>
    /// <param name="isActive">True to move to endPos/Rot, false to return to start</param>
    public override void SetBool(bool isActive)
    {
        buttonActive = isActive;
    }

    /// <summary>
    /// Returns the current linear velocity of the platform.
    /// </summary>
    public Vector3 GetVelocity() => linearVelocity;

}
