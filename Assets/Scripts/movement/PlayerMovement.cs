using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal1;
    private float speed = 5f;
    private float coyoteTime = 0.02f;
    private float jumpHeightFalloff = 0.1f;
    private float jumpingPower = 7f;
    private bool isFacingRight = true;
    public bool isFrozen = false;
    public bool activeGrapple = false;

    [SerializeField] private Rigidbody rb;
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

    // Update is called once per frame
    void Update()
    {


        //if (isFrozen) // freeze for a moment while grappling
        //{
        //    rb.velocity = Vector3.zero;
        //}




    }

    private void FixedUpdate()
    {

        float horizontalInput = Input.GetAxis("Horizontal 1");

        // If there's input, set the velocity; otherwise, you might want to gradually decelerate.
        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, rb.velocity.z);
        }

        if (activeGrapple) return; // dont jump OR move while grappling




        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower); //apply the current vewlocity and the jump power
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpHeightFalloff);
        }


        flip();
    }

    private bool isGrounded()
    {
        if(Physics.OverlapSphere(groundCheck.position, coyoteTime, groundLayer) != null) //coyote time is the radius of the sphere checking if we are able to jump
        {
            return true;
        } else { 
            return false;
        }
    }

    private void flip()
    {
        if (isFacingRight && horizontal1 < 0f || !isFacingRight && horizontal1 > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;  
        }
    }

    public void jumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = calculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(setVelocity), 0.1f);
    }

    private Vector3 velocityToSet;

    private void setVelocity()
    {
        rb.velocity = velocityToSet;
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


}

