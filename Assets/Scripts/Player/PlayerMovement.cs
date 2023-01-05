using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField, Range(100f, 500f)] float maxSpeed = 200f;
    [SerializeField, Range(100f, 500f)] float climbSpeed = 100f;
    [SerializeField, Range(100f, 500f)] float jumpForce = 325f;
    [SerializeField, Range(0f, 10f)] float wallJumpSpeed = 6.5f;
    [SerializeField, Range(0f, 10f)] float wallJumpForce = 5.5f;
    private int wallJumpDirection = 1;
    private bool canDoubleJump;
    private RaycastHit2D isPlayerOnGround;
    private RaycastHit2D isPlayerOnWall;
    [SerializeField, Range(0f, 10f)] float accelerationSpeed = 2.5f;
    [SerializeField, Range(0f, 10f)] float decelerationSpeed = 3f;
    [SerializeField, Range(0f, 5f)] float fallRate = 4.5f;
    [SerializeField, Range(0f, 5f)] float lowJumpRate = 2.5f;
    [SerializeField] float coyoteTime = 0.2f;
    float coyoteTimeCounter;

    [Header("Player Conditions")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    Vector2 movementInputX;
    Vector2 movementInputY;
    Rigidbody2D rb;
    bool turning;
    bool canMoveX = true;
    private bool canMoveY = false;
    private bool wallGrab = false;
    public bool isFacingRight;
    bool wallJump = false;

    PlayerInput playerInput;
    InputAction jumpAction;
    InputAction wallClimbAction;
    InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        isFacingRight = true;

        jumpAction = playerInput.actions["Jump"];
        wallClimbAction = playerInput.actions["WallClimb"];
        moveAction = playerInput.actions["Move"];
    }

    private void FixedUpdate()
    {
        CalcAccelerationAndDeceleration();
    }

    private void Update()
    {
        if (IsGrounded() || IsOnWall())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (rb.velocity.y < 0 && !wallJump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallRate - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpAction.IsPressed() && !wallJump)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpRate - 1) * Time.deltaTime;
        }
        if (movementInputX.x > 0 && !isFacingRight)
        {
            Flip();
        }
        if (movementInputX.x < 0 && isFacingRight)
        {
            Flip();
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMoveX)
        {
            movementInputX = context.ReadValue<Vector2>();
        }
        if (canMoveY)
        {
            movementInputY = context.ReadValue<Vector2>();
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        Jump(context);

        //if (!IsGrounded() && IsWallClimbing() && context.performed && wallAction.IsPressed())
        //{
        //    wallJumpCooldown = 0;
        //    rb.AddForce(Vector2.up * jumpForce);

        //}
    }

    public void WallClimb(InputAction.CallbackContext context)
    {
        WallGrab(context);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (coyoteTimeCounter > 0f && context.performed && !wallJump)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = true;
        }
        if (context.action.triggered && !IsGrounded() && canDoubleJump && !wallJump)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = false;
        }
        if (IsOnWall() && context.action.triggered && !wallClimbAction.IsPressed())
        {
            wallJump = true;

            if (wallJump && IsOnWall() && isFacingRight && context.action.triggered && !wallGrab)
            {
                Vector2 jumpDirection = new Vector2(wallJumpSpeed * -wallJumpDirection , wallJumpForce);
                rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                
                Flip();
            }
            if (wallJump && IsOnWall() && !isFacingRight && context.action.triggered && !wallGrab)
            {
                Vector2 jumpDirection = new Vector2(wallJumpSpeed * wallJumpDirection, wallJumpForce);
                rb.AddForce(jumpDirection, ForceMode2D.Impulse);
                
                Flip();
            }
        }
        if (!IsOnWall())
            wallJump = false;
        if(context.canceled)
        {
            coyoteTimeCounter = 0f;
        }
    }

    private void WallGrab(InputAction.CallbackContext context)
    {
        if (IsOnWall() && context.performed)
        {
            canMoveX = false;
            canMoveY = true;
            wallGrab = true;

            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;

        }
        if (context.canceled)
        {
            canMoveX = true;
            canMoveY = false;
            wallGrab = false;
            
            rb.gravityScale = 1;
        }
    }

    private void CalcAccelerationAndDeceleration()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        float force = movementInputX.x * maxSpeed * accelerationSpeed * Time.deltaTime;
        if (Mathf.Abs(movementInputX.x) < 0.1f)// && !turning)
        {
            force = -rb.velocity.x * decelerationSpeed;
        }
        rb.AddForce(new Vector2(force, 0));
        //rb.AddForce(movementInput * maxSpeed * Time.deltaTime);

        if (wallGrab && wallClimbAction.IsPressed() && IsOnWall())
        {
            if (moveAction.IsPressed())
            {
                rb.velocity = new Vector2(0, movementInputY.y);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, climbSpeed);
                float forceUp = movementInputY.y * climbSpeed * Time.deltaTime;
                rb.AddForce(new Vector2(0, forceUp));
            }
            if (!moveAction.IsPressed())
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
            }
            if (jumpAction.IsPressed() && coyoteTimeCounter > 0f)
            {
                rb.AddForce(Vector2.up * jumpForce);
            }
        }
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;

        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        isFacingRight = !isFacingRight;

    }

    bool IsOnWall()
    {
        isPlayerOnWall = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0), rayDistance, wallLayer.value);
        return isPlayerOnWall;
    }

    bool IsGrounded()
    {
        isPlayerOnGround = Physics2D.Raycast(transform.position, Vector2.down, rayDistance, groundLayer.value);
        return isPlayerOnGround;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector2.down) * rayDistance;
        Gizmos.DrawRay(transform.position, direction);

        Gizmos.color = Color.blue;
        Vector3 wallCheck = transform.TransformDirection(new Vector2(transform.localScale.x, 0)) * rayDistance;
        Gizmos.DrawRay(transform.position, wallCheck);
    }
}

//if (context.performed)
//{
//    if (wallJumpCooldown > 0.2f)
//    {
//        if ((IsWallClimbing() && IsGrounded()) || (IsWallClimbing() && !IsGrounded()))
//        {
//            rb.gravityScale = 0;
//            rb.velocity = Vector2.zero;
//        }
//    }
//    else
//        wallJumpCooldown += Time.deltaTime;
//}
//else
//{
//    rb.gravityScale = 1;
//    rb.velocity += Vector2.up * Physics2D.gravity.y * (fallRate - 1) * Time.deltaTime;
//}
//if (wallJumpCooldown > 0.2f)
//{
//}
//else
//{
//    //wallJumpCooldown += Time.deltaTime;
//}
