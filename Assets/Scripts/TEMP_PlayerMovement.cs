using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMulitplier;
    bool readyToJump;

    //[Header("Ground Check")]
    //public float playerHeight;
    //public LayerMask groundMask;
    //public LayerMask boxMask;
    //bool isGrounded;
    //bool isOnBox;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    //[Header("Object Manipulation")]
    public Transform orientation;
    //public Transform attachedObject;
    //public Transform holdArea;
    //public TMP_Text interactionPrompt;

    [Header("Dialogue")]
    public DialogueManager dialogueManager;

    [Header("Audio")]
    public AudioClip footstep1;
    public AudioClip footstep2;

    bool isMoving;
    bool playedFootstep1;
    bool playedFootstep2;
    float footstepTimer = 0f;

    float horizontalInput;
    float verticalInput;

    float itemInteractionDistance = 2.25f;
    bool cast;
    RaycastHit hit;
    RaycastHit standingBox;
    bool inMailDeliveryZone;

    bool readingNote = false;
    bool readyToPutDownNote = false;

    Vector3 moveDirection;

    Rigidbody rb;

    FixedJoint holdJoint;

    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;        
    }

    private void Update() {
        if (!dialogueManager.dialogueIsPlaying) { //no moving while dialogue plays

            //ground check so there's no drag in the air
            //isGrounded and isOnBox are always used together, but we need standingBox as a separate variable, so that's why we need to check both
            //isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
            //isOnBox = Physics.Raycast(transform.position, Vector3.down, out standingBox, playerHeight * 0.5f + 0.2f, boxMask);

            //check for objects in range
            //cast = Physics.Raycast(orientation.position, orientation.forward, out hit, itemInteractionDistance);

            //if (attachedObject == null) {
            //    if (cast && hit.transform.CompareTag("Mailbox") && !hit.transform.GetComponent<MailboxController>().mailDelivered && inMailDeliveryZone && (isGrounded || isOnBox)) {
            //        interactionPrompt.text = "[E] Deliver mail";
            //    }
            //    else if (cast && hit.transform.CompareTag("Cube") && hit.transform != standingBox.transform) {
            //        interactionPrompt.text = "[E] Pick up";
            //    }
            //    else if (cast && hit.transform.CompareTag("Recording") && !hit.transform.GetComponent<RecordingController>().recordingCollected) {
            //        interactionPrompt.text = "[E] Play recording";
            //    }
            //    else if (cast && hit.transform.CompareTag("Note")) {
            //        interactionPrompt.text = "[E] Read note";
            //    }
            //    else {
            //        interactionPrompt.text = "";
            //    }
            //}
            //else {
            //    interactionPrompt.text = "";
            //}

            //if the joint has broken, drop the object
            //if (holdJoint == null && attachedObject != null) {
            //    attachedObject.SetParent(null);
            //    attachedObject.GetComponent<BoxController>().isHeld = false;

            //    Rigidbody cubeRb = attachedObject.GetComponent<Rigidbody>();

            //    cubeRb.useGravity = true;
            //    cubeRb.freezeRotation = false;
            //    cubeRb.mass = 500;

            //    attachedObject = null;
            //}


            GetInput();
            SpeedControl();

            //if (isGrounded || isOnBox) {
            //    rb.drag = groundDrag;
            //}
            //else {
            //    rb.drag = 2;
            //}
        }

        //if (readingNote) {
        //    if (Input.GetKeyDown(KeyCode.E) && readyToPutDownNote) {
        //        readingNote = false;
        //        dialogueManager.dialogueIsPlaying = false;
        //        hit.transform.GetComponent<NoteController>().PutDownNote();
        //    }
        //}
        
    }

    private void FixedUpdate() {
        if (!dialogueManager.dialogueIsPlaying) {
            MovePlayer();
            if (isMoving) {
                PlayFootsteps();
            }
        }
    }

    private void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if ((horizontalInput != 0 || verticalInput != 0) && (isGrounded || isOnBox) && !isMoving) {
        //    isMoving = true; //the grounded check is because this is only used for footsteps sfx
        //}
        //else {
        //    isMoving = false;
        //}

        //if (Input.GetKey(KeyCode.Space) && readyToJump && (isGrounded || isOnBox)) {
        //    readyToJump = false;
        //    Jump();
        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}


        //if (Input.GetKeyDown(KeyCode.E) && cast && hit.transform.CompareTag("Mailbox") && inMailDeliveryZone && (isGrounded || isOnBox)) {
        //    MailboxInteraction();
        //}
        //else if (Input.GetKeyDown(KeyCode.E) && cast && hit.transform.CompareTag("Recording")) {
        //    RecordingInteraction();
        //}
        //else if (Input.GetKeyDown(KeyCode.E) && cast && hit.transform.CompareTag("Note")) {
        //    NoteInteraction();
        //}
        //else if (Input.GetKeyDown(KeyCode.E)) { //run this even if the ray doesn't hit anything, to include dropping the cube
        //    CubeInteraction();
        //}

    }

    private void MovePlayer() {
        //calculate direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection.y = 0; //make sure they don't move on the y axis when they're just walking around

        if (OnSlope() && !exitingSlope) {
            //multiply forces by the mass so that movement stays the same when the player is holding a box
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * rb.mass, ForceMode.Force);

            if (rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 5f, ForceMode.Force);
            }
        }
        //else {
        //    if (isGrounded || isOnBox) {
        //        rb.AddForce(moveDirection.normalized * moveSpeed * rb.mass, ForceMode.Force);
        //    }
        //    else {
        //        rb.AddForce(moveDirection.normalized * moveSpeed * rb.mass * airMulitplier, ForceMode.Force);
        //    }
        //}
        

        rb.useGravity = !OnSlope();
    }

    private void PlayFootsteps() {

        footstepTimer += 0.01f; //gets called in fixedupdate so it's even

        if (footstepTimer >= 0.2 && !playedFootstep1) {
            audioSource.PlayOneShot(footstep1);
            playedFootstep1 = true;
        }
        else if (footstepTimer >= 0.4 && !playedFootstep2) {
            audioSource.PlayOneShot(footstep2);
            playedFootstep2 = true;
        }
        else if (footstepTimer >= 0.4) {
            footstepTimer = 0;
            playedFootstep1 = false;
            playedFootstep2 = false;
        }
    }

    private void SpeedControl() {

        //limit speed on slopes
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed) {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limit speed on ground/air
        else {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //if the velocity gets too high, tamp it down
            if (flatVelocity.magnitude > moveSpeed) {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump() {
        exitingSlope = true;

        //reset y velocity so the jumps are all consistent
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * rb.mass, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope() {
        //if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f)) {
        //    float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
        //    return angle < maxSlopeAngle && angle > 0;
        //}

        return false;
    }

    private Vector3 GetSlopeMoveDirection() {
        Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized, Color.blue);

        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    //private void CubeInteraction() {

    //    //Drop the cube
    //    if (attachedObject != null) {
    //        attachedObject.SetParent(null);
    //        attachedObject.GetComponent<BoxController>().isHeld = false;

    //        //reset player mass
    //        rb.mass = 1;

    //        Destroy(holdJoint);

    //        Rigidbody cubeRb = attachedObject.GetComponent<Rigidbody>();

    //        cubeRb.useGravity = true;
    //        cubeRb.freezeRotation = false;

    //        //reset cube mass
    //        cubeRb.mass = 500;

    //        attachedObject = null;
    //    }
    //    //Pick up a cube
    //    else {
    //        if (cast) {
    //            if (hit.transform.CompareTag("Cube")) {

    //                //ensure the box we're trying to pick up isn't the one we're standing on
    //                if (hit.transform != standingBox.transform) {
    //                    attachedObject = hit.transform;
    //                    attachedObject.SetParent(holdArea);
    //                    attachedObject.GetComponent<BoxController>().isHeld = true;

    //                    //increase the player mass so that the box can't lift up the player when they look down
    //                    rb.mass = 200;

    //                    Rigidbody cubeRb = attachedObject.GetComponent<Rigidbody>();

    //                    cubeRb.useGravity = false;
    //                    cubeRb.freezeRotation = true;

    //                    holdJoint = this.AddComponent<FixedJoint>();
    //                    holdJoint.connectedBody = cubeRb;
    //                    holdJoint.breakForce = 2300f; 

    //                    cubeRb.drag = 4;
    //                    cubeRb.angularDrag = 0;
    //                    cubeRb.mass = 1;

    //                    interactionPrompt.text = ""; //hide the prompt once they've picked it up
    //                }
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.tag == "MailDeliveryZone") {
    //        inMailDeliveryZone = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other) {
    //    if (other.gameObject.tag == "MailDeliveryZone") {
    //        inMailDeliveryZone = false;
    //    }
    //}

    //private void MailboxInteraction() {
    //    hit.transform.GetComponent<MailboxController>().DeliverMail();
    //}

    //private void RecordingInteraction() {
    //    hit.transform.GetComponent<RecordingController>().PlayRecording();
    //}

    //private void NoteInteraction() {
    //    hit.transform.GetComponent<NoteController>().ReadNote();
    //    dialogueManager.dialogueIsPlaying = true; //freeze the player while they're reading
    //    readingNote = true; //keep track of whether it's the note so they can put it down
    //    Invoke(nameof(ResetNote), jumpCooldown);
    //}
    
    //private void ResetNote() {
    //    readyToPutDownNote = true; //ensure that the note isn't put down immediately after the user picks it up
    //}

}
