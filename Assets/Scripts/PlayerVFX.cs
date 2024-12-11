using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    //subscribe to dash event
    //when dash, update FOV and enable VFX
    //when no dash, lower FOV and disable VFX

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject VFXObj;
    [SerializeField] private Camera playerCamera;

    public float fovStretchingInDegrees = 5f;

    private void Start(){
        playerMovement.OnDashStateChanged += PlayerMovement_OnDashStateChanged;
    }

    private void PlayerMovement_OnDashStateChanged(object sender, PlayerMovement.OnDashStateChangedEventArgs e)
    {
        if(e.dashState)
            EnableDashingVisuals();
        else
            DisableDashingVisuals();
    }

    private void DisableDashingVisuals()
    {
        VFXObj.SetActive(false);
        if(SettingGlobals.Singleton.fovStretchingWhenDashing)
            playerCamera.fieldOfView -= fovStretchingInDegrees;
    }

    private void EnableDashingVisuals()
    {
        VFXObj.SetActive(true);
        if(SettingGlobals.Singleton.fovStretchingWhenDashing)
            playerCamera.fieldOfView += fovStretchingInDegrees;
    }
}
