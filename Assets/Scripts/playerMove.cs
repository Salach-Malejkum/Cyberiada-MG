using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float airSpeedChange = 1f;
    public float jumpForce = 10f;
    public float jumpMultiBase = 0.5f;
    public float wallJumpForce = 5f;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private Rigidbody rb;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isWalled = false;
    private bool doubleJumped = false;
    private bool isFacingRight = true;
    private bool isJumpCharging = false;
    private float jumpMulti = 0.5f;

    private bool canDash = true;
    private bool isDashing;
    private float dashPower = 24f;
    private float dashTime = 0.2f;
    private float dashCooldown = 2f;

    private float timerToSprint = 3f;
    private float targetTime = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity.x != 0f && rb.linearVelocity.y == 0)
        {
            targetTime -= Time.deltaTime;
        }
        else
        {
            ResetTimer();
        }

        if (targetTime <= 0.0f)
        {
            Sprint();
        }

        if (isJumpCharging && jumpMulti <= 1)
        {
            jumpMulti += 0.05f;
            Debug.Log(jumpMulti);
        }
        
        Move();
        CheckGrounded();
        CheckWall();

        if (isGrounded && !canDash)
        {
            canDash = true;
        }
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
                if (rb.linearVelocity.x + moveInput.x * airSpeedChange < moveSpeed && rb.linearVelocity.x + moveInput.x * airSpeedChange > -moveSpeed)
                {
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x + moveInput.x*airSpeedChange, rb.linearVelocity.y, 0f);
                }
                else
                {
                    rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, 0f);
                }
            }
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce*jumpMulti, 0f);
            doubleJumped = false;
        }
        else
        {
            if (!doubleJumped && isWalled && moveInput.x == 0 || !doubleJumped && !isWalled)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce*jumpMulti, 0f);
                doubleJumped = true;
            }
            if (isWalled && moveInput.x != 0)
            {
                rb.linearVelocity = new Vector3(moveInput.x * -wallJumpForce*jumpMulti, jumpForce*jumpMulti, 0f);
            }
        }

        if (isDashing)
        {
            rb.linearVelocity = new Vector3(moveSpeed, jumpForce*jumpMulti, 0f);
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
            if (!isWalled)
            {
                isWalled = Physics.Raycast(transform.position, Vector3.left, out hit, 1.1f, wallLayer);
            }
        }

    }

    void Sprint()
    {
        moveSpeed = 10f;
    }

    void ResetTimer()
    {
        targetTime = timerToSprint;
        moveSpeed = 5f;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        SendMessage("HandleMoveInput", moveInput, SendMessageOptions.DontRequireReceiver);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("pressed");
            isJumpCharging = true;
        }
        if (!value.isPressed)
        {
            Debug.Log("released");
            ResetTimer();
            Jump();
            isJumpCharging = false;
            jumpMulti = jumpMultiBase;
        }
        SendMessage("HandleJumpInput", true, SendMessageOptions.DontRequireReceiver);
    }

    void OnDash(InputValue value)
    {
        if (canDash)
        {
            ResetTimer();
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.useGravity = false;
        rb.linearVelocity = new Vector3((isFacingRight ? 1f : -1f) * dashPower, 0f, 0f);
        yield return new WaitForSeconds(dashTime);
        rb.useGravity = true;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        if (isGrounded)
        {
            canDash = false;
        }
    }
}
