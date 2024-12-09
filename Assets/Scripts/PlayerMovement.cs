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
    private Vector3 dashVector;
    private bool readyToJump;
    private float speedModifier = 10f;
    private Vector3 moveDir;
    private Rigidbody rb;
    private int currentJumpsRemaining, currentDashesRemaining;
    private bool currentlyDashing, currentlyCanDash;
    private float dashTimer;
    public event EventHandler<OnInvulnerabilityStateChangedEventArgs> OnInvulnerabilityStateChanged;
    public class OnInvulnerabilityStateChangedEventArgs
    {
        public bool invulnerabilityState;
    }
    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        currentJumpsRemaining = PlayerGlobals.Singleton.totalJumps;
        currentDashesRemaining = PlayerGlobals.Singleton.totalDashes;
        InputManager.Singleton.OnJumpPerformed += InputManager_OnJumpPerformed_HandleJumpPerformed;
        InputManager.Singleton.OnDashPerformed += InputManager_OnDashPerformed_HandleDashPerformed;
        currentlyDashing = false;
        currentlyCanDash = true;
        dashTimer = 0f;
    }
    private void Update(){
        GetInput();
        HandleDrag();
        HandleJump(false);
        LimitMovementSpeed();
        AddDashSpeed();
        UpdateTimers();
    }
    private void FixedUpdate(){
        MovePlayer();
    }
    private void UpdateTimers(){
        if (dashTimer < PlayerGlobals.Singleton.dashResetTimer)
            dashTimer += Time.deltaTime;
        if (dashTimer >= PlayerGlobals.Singleton.dashResetTimer) {
            currentDashesRemaining = PlayerGlobals.Singleton.totalDashes;
        }
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
    private void LimitMovementSpeed(){
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
    private void ResetIsDashing(){
        currentlyDashing = false;
        OnInvulnerabilityStateChanged?.Invoke(this, new OnInvulnerabilityStateChangedEventArgs{invulnerabilityState = false});
    }
    private void ResetCurrentlyCanDash(){
        currentlyCanDash = true;
    }
    private void HandleDashPerformed(){
        //If they have enough dashes, aren't currently dashing
        if(currentDashesRemaining > 0 && !currentlyDashing && currentlyCanDash) {
            dashTimer = 0f;
            currentlyDashing = true;
            currentlyCanDash = false;
            currentDashesRemaining--;
            Invoke(nameof(ResetIsDashing), PlayerGlobals.Singleton.dashDuration);
            Invoke(nameof(ResetCurrentlyCanDash), PlayerGlobals.Singleton.dashDuration + PlayerGlobals.Singleton.dashCooldown);
            Dash();
            OnInvulnerabilityStateChanged?.Invoke(this, new OnInvulnerabilityStateChangedEventArgs{invulnerabilityState = true});
        }
    }
    //sets the dashVector
    private void Dash(){
        if(moveDir.magnitude == 0){
            dashVector = orientation.forward * speedModifier * PlayerGlobals.Singleton.dashForce;
        }
        else{
            dashVector = moveDir.normalized * speedModifier * PlayerGlobals.Singleton.dashForce;
        }
    }
    private void AddDashSpeed(){
        if(currentlyDashing)
            rb.AddForce(dashVector, ForceMode.Force);
    }
    private void InputManager_OnJumpPerformed_HandleJumpPerformed(object sender, EventArgs e){
        HandleJump(true);
    }
    private void InputManager_OnDashPerformed_HandleDashPerformed(object sender, EventArgs e){
        HandleDashPerformed();
    }
}
