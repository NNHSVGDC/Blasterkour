using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    public float wallJumpForce = 5f;
    public LayerMask groundLayers;
    public LayerMask wallLayers;
    public float groundCheckRadius = 0.3f;
    public float wallCheckRadius = 1f;
    public Transform groundCheckPoint;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool canWallJump;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canWallJump = true;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayers);

        if (isGrounded)
        {
            canWallJump = true; // Reset wall jump ability when grounded
        }

        isTouchingWall = !isGrounded && Physics.CheckSphere(transform.position, wallCheckRadius, wallLayers);

        // Player movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.Normalize();

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else if (isTouchingWall && canWallJump)
            {
                Vector3 wallJumpDirection = Vector3.up; // Adjust this vector to control the jump direction off the wall
                rb.AddForce(wallJumpDirection * wallJumpForce, ForceMode.Impulse);
                canWallJump = false; // Disable wall jump until player touches the ground again
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the ground check sphere and wall check sphere in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wallCheckRadius);
    }
}
