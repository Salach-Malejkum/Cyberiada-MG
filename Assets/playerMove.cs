using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        CheckGrounded();
    }

    void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, 0f) * moveSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, 0f);
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }
    }
}
