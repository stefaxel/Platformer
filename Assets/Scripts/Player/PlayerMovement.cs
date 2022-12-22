using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField, Range(100f, 500f)] float maxSpeed = 200f;
    [SerializeField, Range(100f, 500f)] float jumpForce = 325f;
    private bool canDoubleJump;
    private RaycastHit2D isPlayerOnGround;
    private RaycastHit2D isPlayerOnWall;
    [SerializeField, Range(0f, 10f)] float accelerationSpeed = 2.5f;
    [SerializeField, Range(0f, 10f)] float decelerationSpeed = 3f;
    [SerializeField, Range(0f, 5f)] float fallRate = 4.5f;
    [SerializeField, Range(0f, 5f)] float lowJumpRate = 2.5f;

    [Header("Player Conditions")]
    [SerializeField] float rayDistance;
    [SerializeField] float wallJumpCooldown;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;
    Vector2 movementInput;
    Rigidbody2D rb;
    bool turning;
    bool canMove = true;
    bool isFacingRight = true;

    PlayerInput playerInput;
    InputAction jumpAction;
    InputAction wallAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        jumpAction = playerInput.actions["Jump"];
        wallAction = playerInput.actions["WallClimb"];
    }

    private void FixedUpdate()
    {
        CalcAccelerationAndDeceleration();
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallRate - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpAction.IsPressed())
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpRate - 1) * Time.deltaTime;
        }
        if (movementInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        if (movementInput.x < 0 && isFacingRight)
        {
            Flip();
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            movementInput = context.ReadValue<Vector2>();
        }

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = true;
        }
        if (context.action.triggered && !IsGrounded() && canDoubleJump)
        {
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = false;
        }

        //if (!IsGrounded() && IsWallClimbing() && context.performed && wallAction.IsPressed())
        //{
        //    wallJumpCooldown = 0;
        //    rb.AddForce(Vector2.up * jumpForce);

        //}
    }

    public void WallClimb(InputAction.CallbackContext context)
    {
        if (context.performed && IsWallClimbing())
        {
            canMove = false;

            Debug.Log("Shift key is being held can wall climb/jump");

            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            //if (jumpAction.IsPressed() && wallJumpCooldown > 0.2f)
            //{
            //    rb.AddForce(Vector2.up * jumpForce);
            //}
            //else
            //    wallJumpCooldown += Time.deltaTime;

            if (context.canceled)
            {
                canMove = true;
                Debug.Log("Shift key has been released");
                rb.gravityScale = 1;
                //rb.velocity += Vector2.up * Physics2D.gravity.y * (fallRate - 1) * Time.deltaTime;
            }
        }

    }

    private void CalcAccelerationAndDeceleration()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        //if (moveAction.WasReleasedThisFrame())
        //{
        //    turning = true;
        //}
        //else
        //{
        //    turning = false;
        //}

        float force = movementInput.x * maxSpeed * accelerationSpeed * Time.deltaTime;
        if (Mathf.Abs(movementInput.x) < 0.1f)// && !turning)
        {
            force = -rb.velocity.x * decelerationSpeed;
        }
        rb.AddForce(new Vector2(force, 0));
        //rb.AddForce(movementInput * maxSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        isFacingRight = !isFacingRight;
    }

    bool IsWallClimbing()
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
