using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float groundDrag, airDrag;
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
    private int currentJumpsRemaining;
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        currentJumpsRemaining = PlayerGlobals.Singleton.totalJumps;
        InputManager.Singleton.OnJumpPerformed += HandleJumpPerformed;
    }
    private void Update(){
        GetInput();
        HandleDrag();
        HandleJump(false);
        LimitSpeed();
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void GetInput(){
        movementInputNormalized = InputManager.Singleton.GetMoveNormalized();
    }
    private void HandleDrag()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, GroundLayer);
        if (isGrounded) {
            rb.drag = groundDrag;
            currentJumpsRemaining = PlayerGlobals.Singleton.totalJumps;
        }
        else
            rb.drag = airDrag;
    }
    private void MovePlayer(){
        moveDir = orientation.forward * movementInputNormalized.y + orientation.right * movementInputNormalized.x;
        rb.AddForce(moveDir.normalized * speedModifier * PlayerGlobals.Singleton.moveSpeed, ForceMode.Force);
    }
    private void LimitSpeed(){
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(horizontalVelocity.magnitude > PlayerGlobals.Singleton.moveSpeed) {
            Vector3 limitedVelocity = horizontalVelocity.normalized * PlayerGlobals.Singleton.moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }
    private void Jump(){
        //reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * PlayerGlobals.Singleton.jumpForce, ForceMode.Impulse);
    }
    private void ResetJump(){
        readyToJump = true;
    }
    private void HandleJump(bool jumpPerformed){
        if((jumpPerformed && readyToJump && currentJumpsRemaining >= 0) ||
        (InputManager.Singleton.GetJumpPressed() && readyToJump && isGrounded)) {
            readyToJump = false;
            currentJumpsRemaining--;
            Jump();
            //Will call ResetJump after jumpCooldown seconds
            Invoke(nameof(ResetJump), PlayerGlobals.Singleton.jumpCooldown);
        }
    }
    private void HandleJumpPerformed(object sender, EventArgs e){
        HandleJump(true);
    }
}
