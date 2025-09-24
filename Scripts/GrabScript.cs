using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    //Player camera
    public Camera cam;
    //Force given to the object when you fling it with your camera
    public float cameraForce = 600.0f;
    //Range of grabbing an object
    public float grabRange = 5.0f;
    //Force of the grab
    public float grabForce = 50.0f;
    //Drag of object when grabbing
    public float grabDrag = 20.0f;
    //Force of throwing the object
    public float throwForce = 20.0f;
    //Check for objects
    public LayerMask isObject;
    public LayerMask isWall;
    public LayerMask isFloor;
    //Checks whats in the middle of your screen
    public RaycastHit rayHit;
    //Area that the object hovers around when you're grabbing it
    public Transform grabArea;
    public float grabAreaRange = 3.0f;
    //Rigidbody for player and object
    Rigidbody playerRB;
    Rigidbody objRB;
    //Reference to grabbed object
    GameObject obj;
    //Reference to playerModel
    public GameObject playerModel;
    //Max speed of player
    float playerMaxSpeed = 9;
    //Jump force of the player
    float playerJumpForce = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Prevent object Clipping through wall
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabAreaRange, isWall)) {
            grabArea.transform.localPosition = new Vector3(0,0,rayHit.distance);
        } else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabAreaRange, isFloor)) {
            grabArea.transform.localPosition = new Vector3(0,0,rayHit.distance);
        } else {
            grabArea.transform.localPosition = new Vector3(0,0,grabAreaRange);
        }
        //Grab or drop object when player presses e
        if (Input.GetKeyDown("e")) {
            if (obj == null) {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabRange, isObject)) {
                    obj = rayHit.transform.gameObject;
                    GrabObject();
                }
            } else {
                DropObject();
                obj = null;
            }
        }
        //Moves object that was grabbed
        if(obj != null) {
            MoveObject();
            //Throws the grabbed object if the user left clicks
            if (Input.GetMouseButtonDown(0)) {
                ThrowObject();
                obj = null;
            }
        }
    }

    // Moves the grabbed object infront of the camera
    void MoveObject() {
        obj.transform.rotation = cam.transform.rotation;
        if(Vector3.Distance(obj.transform.position, grabArea.position) > grabAreaRange*2) {
            LoseObject();
            obj = null;
        } else if(Vector3.Distance(obj.transform.position, grabArea.position) > 0.1f) {
            Vector3 moveDirection = (grabArea.position - obj.transform.position);
            objRB.AddForce(moveDirection * grabForce, ForceMode.Acceleration);
        }
    }

    // Throws the object
    void ThrowObject() {
        DropObject();
        objRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
    }

    // Grabs the object
    void GrabObject() {
        if (obj.GetComponent<Rigidbody>().mass >= 20) {
            GetComponent<MovementScript>().maxSpeed = playerMaxSpeed/4;
            GetComponent<MovementScript>().jumpForce = playerJumpForce/2;
        }
        objRB = obj.GetComponent<Rigidbody>();
        objRB.useGravity = false;
        objRB.drag = grabDrag;
        objRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        //obj.transform.parent = grabArea;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), playerModel.GetComponent<Collider>(), true);
    }

    // Drops the object
    void DropObject() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraForce;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraForce;
        Vector3 moveDirection = cam.transform.right * mouseX + cam.transform.up * mouseY;
        objRB.useGravity = true;
        objRB.drag = 1;
        objRB.constraints = RigidbodyConstraints.None;
        objRB.velocity = playerRB.velocity + moveDirection;
        //obj.transform.parent = null;
        GetComponent<MovementScript>().maxSpeed = playerMaxSpeed;
        GetComponent<MovementScript>().jumpForce = playerJumpForce;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), playerModel.GetComponent<Collider>(), false);
    }

    // Drops the object if the player does something to lose it
    void LoseObject() {
        objRB.useGravity = true;
        objRB.drag = 1;
        objRB.constraints = RigidbodyConstraints.None;
        objRB.velocity = new Vector3(0,0,0);
        //obj.transform.parent = null;
        GetComponent<MovementScript>().maxSpeed = playerMaxSpeed;
        GetComponent<MovementScript>().jumpForce = playerJumpForce;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), playerModel.GetComponent<Collider>(), false);
    }
    
}
