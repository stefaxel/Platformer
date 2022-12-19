using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float maxSpeed = 500f;
    [SerializeField] float jumpForce = 300f;
    private bool canDoubleJump;
    private RaycastHit2D isPlayerOnGround;
    [SerializeField, Range(0f, 10f)] float accelerationSpeed;
    [SerializeField, Range(0f, 10f)] float decelerationSpeed;
    [SerializeField, Range(0f, 5f)] float fallRate;
    [SerializeField, Range(0f, 5f)] float lowJumpRate;

    [Header("Player Conditions")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;
    Vector2 movementInput;
    Rigidbody2D rb;

    PlayerInput playerInput;
    InputAction jumpAction;
    InputAction moveAction;

    bool changeDirection => (rb.velocity.x > 0f && moveAction.ReadValue<float>() < 0f)
        || (rb.velocity.x < 0f && moveAction.ReadValue<float>() > 0f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        rb.AddForce(movementInput * maxSpeed * Time.deltaTime);

        //float speedDifference = targetSpeed - rb.velocity.x;
        //float movement = speedDifference * acceleration;

        //rb.AddForce(movementInput * accelerationSpeed * Time.deltaTime);
        //if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        //    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);

        //if(Mathf.Abs(moveAction.ReadValue<float>()) < 0.4f || changeDirection)
        //{
        //    rb.drag = decelerationSpeed;
        //}
        //else
        //{
        //    rb.drag = 0f;
        //}

        //if (moveAction.IsPressed())
        //{
        //    playerVelocity += accelRate * Time.deltaTime;
        //    playerVelocity = Mathf.Min(playerVelocity, maxSpeed);

        //    rb.AddForce(movementInput * playerVelocity * Time.deltaTime);
        //}
        //if (!moveAction.IsPressed())
        //{
        //    playerVelocity += decelRate * Time.deltaTime;
        //    playerVelocity = Mathf.Max(playerVelocity, 0);

        //    rb.AddForce(movementInput * playerVelocity * Time.deltaTime);
        //}
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

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (IsGrounded() && context.performed)
        {
            //Debug.Log("Spacebar pressed a first time");
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = true;
        }
        if (context.action.triggered && !IsGrounded() && canDoubleJump)
        {
            //Debug.Log("Spacebar pressed a second time");
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = false;
        }

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
    }
}
