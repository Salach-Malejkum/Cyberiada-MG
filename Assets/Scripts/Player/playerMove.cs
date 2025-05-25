using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float baseMoveSpeed = 5f;
    private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField][Range(0, 1)] private float jumpCancelMulti = 0.5f;
    [SerializeField] private float airDragMovementModifier = 400f;
    [Header("Attacking")]
    public bool isAttacking = false;
    [Header("IsGrounded")]
    [SerializeField] private GroundedManager groundedManager;
    public bool isGrounded;
    private bool isWalled = false;
    private bool doubleJumped = false;
    public bool isFacingRight { get; private set; }
    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 2f;
    private bool isDashing;

    [Header("Sprint")]
    [SerializeField] private float timeToSprint = 0.5f;
    [SerializeField] private float maxSprintSpeed = 10f;
    [SerializeField] private float sprintSpeedIncrement = 0.1f;
    private float sprintTimer;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enterablePlatform;
    [SerializeField] private LayerMask wallLayer;

    [Header("Unlocked Skills")]
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool canBlock;
    [SerializeField] private bool canAttack;


    private Rigidbody rb;
    private Vector2 moveInput;

    private bool isWallLeft;

    private Animator anim;
    private SpriteRenderer renderer;
    private PlayerStats stats;
    [Header("Attack zone")]
    [SerializeField] private Transform attackPosition;

    [Header("Enterable platforms")]
    private bool isOnEnterablePlatform = false;
    private Collider platformCollider;
    private CapsuleCollider capsuleCollider;

    private void OnEnable()
    {
        if (groundedManager != null)
        {
            groundedManager.OnIsGroundedChanged +=  CheckGrounded;
        }
    }

    private void OnDisable()
    {
        if (groundedManager != null)
        {
            groundedManager.OnIsGroundedChanged -= CheckGrounded;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = baseMoveSpeed;
        sprintTimer = timeToSprint;
        isFacingRight = true;
        anim = this.GetComponentInChildren<Animator>();
        renderer = this.GetComponent<SpriteRenderer>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        stats = GetComponent<PlayerStats>();
    }

    void FixedUpdate()
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
        FallCheckPoint();
    }

    void Move()
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            ResetTimer();
            Flip();
        }
        if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            ResetTimer();
            Flip();
        }

        if (isAttacking)
        {
            rb.linearVelocity = new Vector3(0f, 0f, 0f);
            anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
            return;
        }

        if (!isDashing)
        {
            Vector3 move = new Vector3(moveInput.x, 0f, 0f) * moveSpeed;
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, 0f);
            }
            else
            {
                rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x + (move.x / airDragMovementModifier), -moveSpeed, moveSpeed), rb.linearVelocity.y, 0f);
            }
        }
        anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("jumpSpeed", rb.linearVelocity.y);
    }

    void Jump()
    {
        if (isDashing)
        {
            rb.linearVelocity = new Vector3(moveSpeed, jumpForce, 0f);
            anim.SetBool("justJumped", true);
            anim.SetFloat("jumpSpeed", rb.linearVelocity.y);
            DashCancel();
        }

        if (isGrounded)
        {
            PerformJumpVelocityCalculation();
            doubleJumped = false;
            anim.SetBool("justJumped", true);
            anim.SetFloat("jumpSpeed", rb.linearVelocity.y);
            return;
        }

        if (canDoubleJump && !doubleJumped && !isWalled)
        {
            if (!IsSameSign(moveInput.x, rb.linearVelocity.x))
            {
                SecondJumpOtherDirection();
            }
            else
            {
                SecondJumpSameDirection();
            }

            doubleJumped = true;
            anim.SetBool("justJumped", true);
            anim.SetFloat("jumpSpeed", rb.linearVelocity.y);
        }

        if (canWallJump && isWalled)
        {
            int sign = isWallLeft ? 1 : -1;
            rb.linearVelocity = new Vector3(sign * wallJumpForce, jumpForce, 0f);
            anim.SetBool("justJumped", true);
            anim.SetFloat("jumpSpeed", rb.linearVelocity.y);
        }
    }

    private void PerformJumpVelocityCalculation()
    {
        float kineticEnergy = Mathf.Lerp(0, jumpForce / 6, (moveSpeed - baseMoveSpeed) / (maxSprintSpeed - baseMoveSpeed));
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce + kineticEnergy, 0f);
    }

    private bool IsSameSign(float a, float b)
    {
        return a * b > 0;
    }

    private void SecondJumpSameDirection()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
    }

    private void SecondJumpOtherDirection()
    {
        rb.linearVelocity = new Vector3(0, jumpForce, 0f);
    }

    void JumpTakeOfEnd()
    {
        anim.SetBool("justJumped", false);
    }

    void CheckGrounded(bool isGrouded)
    {
        this.isGrounded = isGrouded;
        anim.SetBool("isGrouded", isGrounded);
    }

    void Sprint()
    {
        if (moveSpeed < maxSprintSpeed)
        {
            moveSpeed += sprintSpeedIncrement;
            anim.SetBool("isRunning", true);
        }
        else
        {
            moveSpeed = maxSprintSpeed;
            anim.SetBool("isRunning", true);
        }
    }

    void ResetTimer()
    {
        anim.SetBool("isRunning", false);
        sprintTimer = timeToSprint;
        moveSpeed = baseMoveSpeed;
    }

    public void OnMove(InputAction.CallbackContext inputAction)
    {
        moveInput = inputAction.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext inputAction)
    {
        if (IsExitPlatformPerformed())
            return;

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
    private void Flip()
    {
        renderer.flipX = !renderer.flipX;

        Vector3 attackPosition = this.attackPosition.localPosition;
        attackPosition.x *= -1;
        this.attackPosition.localPosition = attackPosition;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (IsEnterablePlatform(collision.gameObject))
        {
            isOnEnterablePlatform = true;
            platformCollider = collision.gameObject.GetComponent<MeshCollider>();
        }

        if (IsWall(collision.gameObject))
        {
            isWalled = true;
            isWallLeft = collision.contacts[0].point.x <= transform.position.x;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (IsEnterablePlatform(collision.gameObject))
        {
            isOnEnterablePlatform = false;
            Physics.IgnoreCollision(capsuleCollider, platformCollider, false);
            platformCollider = null;
        }

        if (IsWall(collision.gameObject))
        {
            isWalled = false;
        }
    }


    private bool IsEnterablePlatform(GameObject gameObject)
    {
        return enterablePlatform == (enterablePlatform | (1 << gameObject.layer));
    }

    private bool IsWall(GameObject gameObject)
    {
        return wallLayer == (wallLayer | (1 << gameObject.layer));
    }


    public void OnExitPlatform(InputAction.CallbackContext inputAction)
    {
        if (isOnEnterablePlatform)
        {
            Physics.IgnoreCollision(capsuleCollider, platformCollider, true);
        }
    }

    public bool IsExitPlatformPerformed()
    {
        // Get the current gamepad
        var gamepad = Gamepad.current;
        if (gamepad == null) return false;

        // Check if left stick is down
        if (isOnEnterablePlatform && gamepad.leftStick.down.ReadValue() > 0.9f)
        {
            return true;
        }

        return false;
    }
    //todo move or rename
    private void FallCheckPoint()
    {
        if (isGrounded)
        {
            stats.UpdateFallCheckPointCoordinates(transform.position);
        }
    }

    public bool GetCanAttack()
    {
        return canAttack;
    }
}
