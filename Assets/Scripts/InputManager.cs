using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    [SerializeField] private PlayerInput playerInput;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one InputManager instance");
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }
    public Vector2 GetHorizontalMovementNormalized() {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    public bool GetIsJumping(){
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
}
