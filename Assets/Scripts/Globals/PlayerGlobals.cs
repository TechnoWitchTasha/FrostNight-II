using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This class lists all current Player variables and their base values.
public class PlayerGlobals : MonoBehaviour
{
    public static PlayerGlobals Singleton { get; private set;}
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("There is more than one PlayerGlobals instance");
        }
        Singleton = this;
    }

    public float moveSpeed, jumpForce, jumpCooldown, dashSpeed, dashDuration, dashCooldown, dashResetTimer;
    public float baseMoveSpeed, baseJumpForce, baseJumpCooldown, baseDashSpeed, baseDashDuration, baseDashCooldown, baseDashResetTimer;
    public int totalJumps, totalDashes;
    public int baseTotalJumps, baseTotalDashes;
    public bool stateIsInvulnerable;

    public PlayerMovement playerMovement;
    private void Start(){
        playerMovement.OnDashStateChanged += PlayerMovement_OnDashStateChanged;

        this.baseMoveSpeed = 7f;
        this.baseJumpForce = 6f;
        this.baseJumpCooldown = 0.25f;
        this.baseTotalJumps = 1;
        this.baseTotalDashes = 1;
        this.baseDashSpeed = 15f;
        this.baseDashDuration = 1f;
        this.baseDashCooldown = 0.15f;
        this.baseDashResetTimer = 3f;

        this.moveSpeed = this.baseMoveSpeed;
        this.jumpForce = this.baseJumpForce;
        this.jumpCooldown = this.baseJumpCooldown;
        this.totalJumps = this.baseTotalJumps;
        this.totalDashes = this.baseTotalDashes;
        this.dashSpeed = this.baseDashSpeed;
        this.dashDuration = this.baseDashDuration;
        this.dashCooldown = this.baseDashCooldown;
        this.dashResetTimer = this.baseDashResetTimer;

        this.stateIsInvulnerable = false;

    }

    private void PlayerMovement_OnDashStateChanged(object sender, PlayerMovement.OnDashStateChangedEventArgs e)
    {
        this.stateIsInvulnerable = e.dashState;
    }
}
