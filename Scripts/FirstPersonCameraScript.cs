using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraScript : MonoBehaviour
{
    //Speed of first person camera
    public float cameraSpeed = 600f;
    //Orientation of the player character
    public Transform playerOrientation;
    
    //Camera rotations
    float xRotation;
    float yRotation;
    //Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSpeed;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        //Set max rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        //Rotate camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //Rotate orientation
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
