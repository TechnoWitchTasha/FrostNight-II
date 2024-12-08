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

    public float moveSpeed, jumpForce, jumpCooldown, dashForce, dashDuration;
    public float baseMoveSpeed, baseJumpForce, baseJumpCooldown, baseDashForce, baseDashDuration;
    public int totalJumps, totalDashes;
    public int baseTotalJumps, baseTotalDashes;
    private void Start(){
        this.baseMoveSpeed = 7f;
        this.baseJumpForce = 6f;
        this.baseJumpCooldown = 0.25f;
        this.baseTotalJumps = 1;
        this.baseTotalDashes = 1;
        this.baseDashForce = 14f;
        this.baseDashDuration = 0.5f;

        this.moveSpeed = this.baseMoveSpeed;
        this.jumpForce = this.baseJumpForce;
        this.jumpCooldown = this.baseJumpCooldown;
        this.totalJumps = this.baseTotalJumps;
        this.totalDashes = this.baseTotalDashes;
        this.dashForce = this.baseDashForce;
        this.dashDuration = this.baseDashDuration;
    }  
}
