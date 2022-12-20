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

    private void CalcAccelerationAndDeceleration()
    {
        float force = movementInput.x * maxSpeed * accelerationSpeed * Time.deltaTime;
        if (Mathf.Abs(movementInput.x) < 0.1f)
        {
            force = -rb.velocity.x * decelerationSpeed;
        }
        rb.AddForce(new Vector2(force, 0));
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        //rb.AddForce(movementInput * maxSpeed * Time.deltaTime);
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
