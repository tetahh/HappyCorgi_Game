using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;

    [Header("Ground Settings")]
    public Transform groundCheck;
    public float groundCheckWidth = 0.5f;      // ngang chân nhân vật
    public float groundCheckHeight = 0.05f;    // chiều cao BoxCast
    public float groundCheckDistance = 0.1f;   // khoảng check xuống dưới
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float move;
    private bool onGround;
    private int airJumpsLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        airJumpsLeft = 1; // double jump = 1 lần nhảy trên không
    }

    void Update()
    {
        move = Input.GetAxisRaw("Horizontal");

        // Ground check bằng BoxCast (ổn định hơn raycast)
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
            airJumpsLeft = 1; // reset double jump khi chạm đất
        }

        // Nhảy
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
    }

    void FixedUpdate()
    {
        // Di chuyển ngang
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Flip hướng đi
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
            // Vẽ BoxCast hình chữ nhật
            Gizmos.DrawWireCube(
                groundCheck.position + Vector3.down * groundCheckDistance / 2,
                new Vector3(groundCheckWidth, groundCheckHeight + groundCheckDistance, 0)
            );
        }
    }
}
