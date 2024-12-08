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
        this.moveSpeed = 7f;
        this.jumpForce = 6f;
        this.jumpCooldown = 0.25f;
        this.totalJumps = 1;
        this.totalDashes = 1;
        this.dashForce = 14f;
        this.dashDuration = 0.5f;

        this.baseMoveSpeed = 7f;
        this.baseJumpForce = 6f;
        this.baseJumpCooldown = 0.25f;
        this.baseTotalJumps = 1;
        this.baseTotalDashes = 1;
        this.baseDashForce = 14f;
        this.baseDashDuration = 0.5f;
    }
}
