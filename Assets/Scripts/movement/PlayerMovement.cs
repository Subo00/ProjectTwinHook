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

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    // Update is called once per frame
    void Update()
    {
        horizontal1 = Input.GetAxisRaw("Horizontal 1");

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




}

