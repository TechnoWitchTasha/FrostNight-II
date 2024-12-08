using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensX, sensY;
    private float sensModifier = 100f;
    public Transform orientation;

    private float xRotation, yRotation;
    private Vector2 look;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        look = InputManager.Singleton.GetLook() * Time.deltaTime;

        yRotation += look.x * sensModifier * sensX;
        xRotation -= look.y * sensModifier * sensY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //apply rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
