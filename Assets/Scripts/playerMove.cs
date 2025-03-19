using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float baseMoveSpeed = 5f;
    private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float airSpeedChange = 1f;
    [SerializeField] private float jumpForce = 10f;
    //[SerializeField] private float jumpMultiBase = 0.5f;
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private float jumpCancelMulti = 0.5f;
    [SerializeField] private float maxHorisontalAirSpeed = 5f;
    [SerializeField] private float airDragMovementModifier = 400f;

    private bool isGrounded;
    private bool isWalled = false;
    private bool doubleJumped = false;
    private bool isFacingRight = true;

    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    private bool canDash = true;
    private bool isDashing;

    [Header("Sprint")]
    [SerializeField] private float timeToSprint = 0.5f;
    [SerializeField] private float maxSprintSpeed = 10f;
    [SerializeField] private float sprintSpeedIncrement = 0.1f;
    private float sprintTimer;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody rb;
    private Vector2 moveInput;

    private bool isWallLeft;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = baseMoveSpeed;
        sprintTimer = timeToSprint;
    }

    void Update()
    {
        if (rb.linearVelocity.x != 0f && isGrounded)
        {
            sprintTimer -= Time.deltaTime;
        }
        else if (rb.linearVelocity.x == 0f)
        {
            ResetTimer();
        }

        if (sprintTimer <= 0.0f)
        {
            Sprint();
        }
        
        Move();
        CheckGrounded();
        CheckWall();
    }

    void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, 0f) * moveSpeed;
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            ResetTimer();
        }
        if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            ResetTimer();
        }

        if (!isDashing)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, 0f);
            }
            else
            {
                if (rb.linearVelocity.x + moveInput.x * airSpeedChange <= maxHorisontalAirSpeed && rb.linearVelocity.x + moveInput.x * airSpeedChange >= -maxHorisontalAirSpeed)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x + moveInput.x*airSpeedChange, rb.linearVelocity.y, 0f);
                }
                else
                {
                    // Clamp to create drag in air
                    rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x + (move.x / airDragMovementModifier), -maxSprintSpeed, maxSprintSpeed), rb.linearVelocity.y, 0f);
                }
            }
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
            doubleJumped = false;
        }
        else
        {
            if (!doubleJumped && !isWalled)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
                doubleJumped = true;
            }
            if (isWalled && isWallLeft)
            {
                rb.linearVelocity = new Vector3(wallJumpForce, jumpForce, 0f);
            }
            if (isWalled && !isWallLeft)
            {
                rb.linearVelocity = new Vector3(-wallJumpForce, jumpForce, 0f);
            }
        }

        if (isDashing)
        {
            rb.linearVelocity = new Vector3(moveSpeed, jumpForce, 0f);
            DashCancel();
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer);
    }

    void CheckWall()
    {
        if (!isGrounded)
        {
            RaycastHit hit;
            isWalled = Physics.Raycast(transform.position, Vector3.right, out hit, 1.1f, wallLayer);
            isWallLeft = false;
            if (!isWalled)
            {
                isWalled = Physics.Raycast(transform.position, Vector3.left, out hit, 1.1f, wallLayer);
                isWallLeft = true;
            }
        }

    }

    void Sprint()
    {
        if (moveSpeed < maxSprintSpeed)
        {
            moveSpeed += sprintSpeedIncrement;
        }
        else
        {
            moveSpeed = maxSprintSpeed;
        }
    }

    void ResetTimer()
    {
        sprintTimer = timeToSprint;
        moveSpeed = baseMoveSpeed;
    }

    public void OnMove(InputAction.CallbackContext inputAction)
    {
        moveInput = inputAction.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext inputAction)
    {
        if (inputAction.started)
        {
            Jump();
        }
        else if (inputAction.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCancelMulti, 0f);
        }
    }

    public void OnDash(InputAction.CallbackContext inputAction)
    {
        if (inputAction.started && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void DashCancel()
    {
        isDashing = false;
        rb.useGravity = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.useGravity = false;
        float speedBefourDash = rb.linearVelocity.x;
        rb.linearVelocity = new Vector3((isFacingRight ? 1f : -1f) * dashPower, 0f, 0f);
        yield return new WaitForSeconds(dashTime);
        if (!isDashing)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.y, rb.linearVelocity.y, 0f);
        }
        else
        {
            DashCancel();
            rb.linearVelocity = new Vector3(speedBefourDash, rb.linearVelocity.y, 0f);
        }
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
