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

    Rigidbody2D rb;
    float normalGravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        normalGravityScale = rb.gravityScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
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
