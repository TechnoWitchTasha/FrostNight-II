using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float groundDrag, airDrag, jumpForce, jumpCooldown;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask GroundLayer;
    public bool isGrounded;
    public Transform orientation;
    private Vector2 movementInputNormalized;
    private bool readyToJump;
    private float speedModifier = 10f;
    private Vector3 moveDir;
    private Rigidbody rb;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }
    private void Update(){
        GetInput();
        HandleDrag();
        HandleJump();
        LimitSpeed();
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void GetInput(){
        movementInputNormalized = InputManager.Instance.GetHorizontalMovementNormalized();
    }
    private void HandleDrag()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, GroundLayer);
        if (isGrounded) {
            rb.drag = groundDrag;
        }
        else
            rb.drag = airDrag;
    }
    private void MovePlayer(){
        moveDir = orientation.forward * movementInputNormalized.y + orientation.right * movementInputNormalized.x;
        
        rb.AddForce(moveDir.normalized * speedModifier * moveSpeed, ForceMode.Force);
    }
    private void LimitSpeed(){
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(horizontalVelocity.magnitude > moveSpeed) {
            Vector3 limitedVelocity = horizontalVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }
    private void Jump(){
        //reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump(){
        readyToJump = true;
    }
    private void HandleJump(){
        if(InputManager.Instance.GetIsJumping() && readyToJump && isGrounded) {
            readyToJump = false;
            Jump();
            //Will call ResetJump after jumpCooldown seconds
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
}
