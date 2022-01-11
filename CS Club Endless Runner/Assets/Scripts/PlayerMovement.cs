using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float jumpForce;

    [SerializeField]
    float fallGravityScale;

    [SerializeField]
    float groundCheckDistance;

    [SerializeField]
    Transform groundCheckPoint;

    [SerializeField]
    LayerMask groundLayer;


    [SerializeField]
    GameObject groundHitEffect;

    Rigidbody2D rb;
    Animator anim;
    float normalGravityScale;
    bool isGrounded = true;

    CameraController cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
        normalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (IsGrounded())
        {
            if (!isGrounded)
            {
                cam.StartShake(0.05f, 0.5f);
                Instantiate(groundHitEffect, groundCheckPoint.position + Vector3.down * 0.5f, Quaternion.Euler(-90, 0, 0));
            }
            isGrounded = true;
        } 
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("jump");
            rb.velocity = new Vector2(0, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y > 0)
        {
            rb.gravityScale = normalGravityScale;
        }
        else
        {
            rb.gravityScale = fallGravityScale;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
    }
}
