using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal1;
    private float speed = 5f;
    private float coyoteTime = 0.2f;
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

        if (activeGrapple) return; // dont jump while grappling


        horizontal1 = Input.GetAxisRaw("Horizontal 1"); // be able to move while grappling


        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower); //apply the current vewlocity and the jump power
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * jumpHeightFalloff);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontal1 * speed, rb.velocity.y);
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
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight/ gravity) + Mathf.Sqrt(2 * displacementY - trajectoryHeight/ gravity));

        return velocityXZ + velocityY;
    }


}

