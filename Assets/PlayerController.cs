using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Ground Settings")]
    public Transform groundCheck;
    public float groundCheckWidth = 0.5f;
    public float groundCheckHeight = 0.05f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private float move;
    private bool onGround;
    private int airJumpsLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        airJumpsLeft = 1; // double jump
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        // Ground check
        onGround = Physics2D.BoxCast(
            groundCheck.position,
            new Vector2(groundCheckWidth, groundCheckHeight),
            0f,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        if (onGround)
        {
            airJumpsLeft = 1;
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (airJumpsLeft > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                airJumpsLeft--;
            }
        }

        // UPDATE ANIMATOR
        anim.SetFloat("Speed", Mathf.Abs(move));
        anim.SetBool("OnGround", onGround);
        anim.SetFloat("YVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        // Move
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip
        if (move > 0)
            transform.localScale = Vector3.one;
        else if (move < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(
                groundCheck.position + Vector3.down * groundCheckDistance / 2,
                new Vector3(groundCheckWidth, groundCheckHeight + groundCheckDistance, 0)
            );
        }
    }
}
