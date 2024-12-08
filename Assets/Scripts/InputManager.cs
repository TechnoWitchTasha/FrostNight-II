using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Singleton { get; private set; }
    private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerInput playerInput;

    public event EventHandler OnDashPerformed;
    public event EventHandler OnInteractPerformed;
    public event EventHandler OnCycleLeftActionPerformed;
    public event EventHandler OnCycleRightActionPerformed;
    public event EventHandler OnCycleDefensiveActionPerformed;
    public event EventHandler OnToggleInventoryPerformed;
    public event EventHandler OnToggleQuickspellMenuPerformed;
    public event EventHandler OnToggleBuildModePerformed;
    
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogError("There is more than one InputManager instance");
        }
        Singleton = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Dash.performed += Dash_Performed;
        playerInputActions.Player.Interact.performed += Interact_Performed;
        playerInputActions.Player.CycleLeftAction.performed += CycleLeftAction_Performed;
        playerInputActions.Player.CycleRightAction.performed += CycleRightAction_Performed;
        playerInputActions.Player.CycleDefensiveAction.performed += CycleDefensiveAction_Performed;
        playerInputActions.Player.ToggleInventory.performed += ToggleInventory_Performed;
        playerInputActions.Player.ToggleQuickspellMenu.performed += ToggleQuickspellMenu_Performed;
        playerInputActions.Player.ToggleBuildMode.performed += ToggleBuildMode_Performed;

    }
    public Vector2 GetMoveNormalized() {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
    public bool GetJump(){
        return playerInputActions.Player.Jump.IsPressed();
    }
    public Vector2 GetLook() {
        if(playerInput.currentControlScheme == "Keyboard"){
            return new Vector2(Input.GetAxisRaw("Mouse X"),
                            Input.GetAxisRaw("Mouse Y"));
        }
        return new Vector2(playerInputActions.Player.LookX.ReadValue<float>(),
                            playerInputActions.Player.LookY.ReadValue<float>());
    }
    private void Dash_Performed(InputAction.CallbackContext context)
    {
        OnDashPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void Interact_Performed(InputAction.CallbackContext context)
    {
        OnInteractPerformed?.Invoke(this, EventArgs.Empty);
    }
    public bool GetLeftAction(){
        return playerInputActions.Player.LeftAction.IsPressed();
    }
    public bool GetRightAction(){
        return playerInputActions.Player.RightAction.IsPressed();
    }
    public bool GetDefensiveAction(){
        return playerInputActions.Player.DefensiveAction.IsPressed();
    }
    public bool GetMelee(){
        return playerInputActions.Player.Melee.IsPressed();
    }
    private void CycleLeftAction_Performed(InputAction.CallbackContext context)
    {
        OnCycleLeftActionPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void CycleRightAction_Performed(InputAction.CallbackContext context)
    {
        OnCycleRightActionPerformed?.Invoke(this, EventArgs.Empty);
    }   
    private void CycleDefensiveAction_Performed(InputAction.CallbackContext context)
    {
        OnCycleDefensiveActionPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void ToggleInventory_Performed(InputAction.CallbackContext context)
    {
        OnToggleInventoryPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void ToggleQuickspellMenu_Performed(InputAction.CallbackContext context)
    {
        OnToggleQuickspellMenuPerformed?.Invoke(this, EventArgs.Empty);
    }
    private void ToggleBuildMode_Performed(InputAction.CallbackContext context)
    {
        OnToggleBuildModePerformed?.Invoke(this, EventArgs.Empty);
    }
    //0 = no input, negative or positive = input
    public float GetCycleMenu(){
        return playerInputActions.Player.CycleMenu.ReadValue<float>();
    }
    //0 = no input, negative or positive = input
    public float GetRotate(){
        return playerInputActions.Player.Rotate.ReadValue<float>();
    }
}
