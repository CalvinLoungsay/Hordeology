using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class GrabScript : MonoBehaviour
{
    //Player camera
    public CinemachineVirtualCamera cam;
    //Input Actions
    private InputActions inputActions;
    private InputAction grabDrop;
    private InputAction throwObj;
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
    public LayerMask isEnemy;
    public LayerMask isWeaponPickup;
    public LayerMask isHealthPickup;
    //Checks whats in the middle of your screen
    public RaycastHit rayHit;
    //Area that the object hovers around when you're grabbing it
    public Transform grabArea;
    public float grabAreaRange = 3.0f;
    //Rigidbody for player and object
    Rigidbody playerRB;
    Rigidbody objRB;
    //Reference to grabbed object
    [HideInInspector]
    public GameObject obj;
    //Reference to player
    public GameObject player;
    //Max speed of player
    float playerMaxSpeed = 7;
    //Jump force of the player
    float playerJumpForce = 40;
    
    // Start is called before the first frame update
    void Start()
    {
        playerMaxSpeed = GetComponent<MovementScript>().maxSpeed;
        playerJumpForce = GetComponent<MovementScript>().jumpForce;
        playerRB = GetComponent<Rigidbody>();
        inputActions = new InputActions();
        grabDrop = inputActions.Actions.GrabDrop;
        throwObj = inputActions.Actions.PrimaryFire;
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Prevent object Clipping through wall
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabAreaRange, isWall | isFloor)) {
            grabArea.transform.localPosition = new Vector3(0,0,rayHit.distance);
        } else {
            grabArea.transform.localPosition = new Vector3(0,0,grabAreaRange);
        }
        //Grab or drop object when player presses e
        if (grabDrop.triggered) {
            if (obj == null) {
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabRange, isObject)) {
                    obj = rayHit.transform.gameObject;
                    GrabObject();
                } else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabRange, isWeaponPickup)) {
                    GetComponent<GunScript>().gunObject = rayHit.collider.gameObject.GetComponent<WeaponPickupScript>().obj;
                    GetComponent<GunScript>().gunObject.ammoLeft = GetComponent<GunScript>().gunObject.magSize;
                    GetComponent<GunScript>().weaponSprite.SetActive(false);
                    GetComponent<GunScript>().weaponSprite = GetComponent<GunScript>().weaponSprites[rayHit.collider.gameObject.GetComponent<WeaponPickupScript>().obj.sprite];
                    GetComponent<GunScript>().weaponSprite.SetActive(true);
                    GetComponent<GunScript>().weaponSprite.GetComponent<SpriteScript>().ChangeSprite(0);
                    Destroy(rayHit.collider.gameObject); 
                } else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, grabRange, isHealthPickup)) {
                    if (GetComponent<PlayerStats>().getHealth() < GetComponent<PlayerStats>().getMaxHealth()) {
                        Destroy(rayHit.collider.gameObject);
                    }  
                }
            } else {
                DropObject();
                obj = null;
            }
        }
        //Moves object that was grabbed
        if(obj != null) {
            //Throws the grabbed object if the user left clicks
            if (throwObj.triggered) {
                ThrowObject();
                obj = null;
            }
            //Temporary solution for when a heavy object gets destroyed
        } else {
            GetComponent<MovementScript>().maxSpeed = playerMaxSpeed;
            GetComponent<MovementScript>().jumpForce = playerJumpForce;
        }

    }

    void FixedUpdate() {
        if(obj != null) {
            MoveObject();
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
            objRB.AddForce(moveDirection * grabForce * 10, ForceMode.Acceleration);
        }
    }

    // Throws the object
    void ThrowObject() {
        DropObject();
        objRB.AddForce(cam.transform.forward * throwForce, ForceMode.Impulse);
        if (obj.GetComponent<ObjectDamageScript>() != null) {
            obj.GetComponent<ObjectDamageScript>().EnableDamage();
        }
    }

    // Grabs the object
    void GrabObject() {
        Color tempColor = obj.GetComponent<Renderer>().material.color;
        tempColor.a = 0.5f;
        obj.GetComponent<Renderer>().material.color = tempColor;
        objRB = obj.GetComponent<Rigidbody>();
        if (objRB.mass >= 20) {
            GetComponent<MovementScript>().maxSpeed = playerMaxSpeed/2;
            GetComponent<MovementScript>().jumpForce = playerJumpForce/2;
            objRB.drag = grabDrag * 1.5f;
        } else {
            objRB.drag = grabDrag;
        }
        
        objRB.useGravity = false;
        objRB.constraints = RigidbodyConstraints.FreezeRotationZ;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
    }

    // Drops the object
    void DropObject() {
        Color tempColor = obj.GetComponent<Renderer>().material.color;
        tempColor.a = 1f;
        obj.GetComponent<Renderer>().material.color = tempColor;
        objRB.angularVelocity = objRB.angularVelocity / 2;
        objRB.velocity = objRB.velocity / 2;
        objRB.angularVelocity += playerRB.angularVelocity;
        objRB.velocity += playerRB.velocity;
        objRB.useGravity = true;
        objRB.drag = 1;
        objRB.constraints = RigidbodyConstraints.None;  
        GetComponent<MovementScript>().maxSpeed = playerMaxSpeed;
        GetComponent<MovementScript>().jumpForce = playerJumpForce;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        if (obj.GetComponent<ObjectDamageScript>() != null) {
            obj.GetComponent<ObjectDamageScript>().DisableDamage();
        }
    }

    // Drops the object if the player does something to lose it
    void LoseObject() {
        Color tempColor = obj.GetComponent<Renderer>().material.color;
        tempColor.a = 1f;
        obj.GetComponent<Renderer>().material.color = tempColor;
        objRB.angularVelocity = Vector3.zero;
        objRB.velocity = Vector3.zero;
        objRB.useGravity = true;
        objRB.drag = 1;
        objRB.constraints = RigidbodyConstraints.None;
        
        GetComponent<MovementScript>().maxSpeed = playerMaxSpeed;
        GetComponent<MovementScript>().jumpForce = playerJumpForce;
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        if (obj.GetComponent<ObjectDamageScript>() != null) {
            obj.GetComponent<ObjectDamageScript>().DisableDamage();
        }
    }


    // Return if grabbing a object
    public bool isGrabbed()
    {
        if (obj == null)
        {
            return false;
        }
        return true;
    }

    
}
