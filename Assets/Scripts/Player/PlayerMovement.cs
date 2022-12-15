using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float movementSpeed = 500f;
    [SerializeField] float jumpForce = 300f;
    private bool canDoubleJump;
    private RaycastHit2D isPlayerOnGround;
    [SerializeField, Range(0f, 500f)] float decceleration;
    [SerializeField, Range(0f, 500f)] float acceleration;
    [SerializeField, Range(0f, 500f)] float targetSpeed;
    [SerializeField, Range(0f, 5f)] float fallRate;
    [SerializeField, Range(0f, 5f)] float lowJumpRate;

    [Header("Player Conditions")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;
    Vector2 movementInput;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        //float speedDifference = targetSpeed - rb.velocity.x;
        //float movement = speedDifference * acceleration;

        rb.AddForce(movementInput * movementSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallRate - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Keyboard.current[Key.Space].IsActuated())
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpRate - 1) * Time.deltaTime;
        }
        //Keyboard.current[Key.Space].IsActuated() || !Gamepad.current[UnityEngine.InputSystem.LowLevel.GamepadButton.South].IsActuated()
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
