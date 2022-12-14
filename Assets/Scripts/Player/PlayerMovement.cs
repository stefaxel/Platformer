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
    
    [Header("Player Conditions")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;
    Vector2 movementX;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(movementX * movementSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        
        movementX = context.ReadValue<Vector2>();
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(IsGrounded() && context.performed)
        {
            Debug.Log("Spacebar pressed a first time");
            rb.AddForce(Vector2.up * jumpForce);
            canDoubleJump = true;
        }
        if(context.action.triggered && !IsGrounded() && canDoubleJump)
        {
            Debug.Log("Spacebar pressed a second time");
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
