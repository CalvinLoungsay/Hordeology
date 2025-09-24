using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraScript : MonoBehaviour
{
    //Speed of first person camera
    public float cameraSpeed = 1f;
    //Mouse sensitivity
    public float sensitivity = 8f;
    //Orientation of the player character
    public Transform playerOrientation;

    //Input Actions
    private InputActions inputActions;
    private InputAction aim;

    //Camera rotations
    float xRotation;
    float yRotation;
    //Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputActions = new InputActions();
        aim = inputActions.Aiming.Aim;
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse inputs
        Vector2 v2 = aim.ReadValue<Vector2>();
        float mouseX = (v2.x / sensitivity) * Time.deltaTime * cameraSpeed;
        float mouseY = (v2.y / sensitivity) * Time.deltaTime * cameraSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        //Set max rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //Rotate camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //Rotate orientation
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public float GetSensitivity()
    {
        return this.sensitivity; // getter of current sensitivity
    }

    public void SetSensitivity(float newSen)
    {
        if (newSen > 0)
        {
            this.sensitivity = newSen;
        }
    }
}
