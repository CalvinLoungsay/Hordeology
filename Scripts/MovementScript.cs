using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    //Player move speed
    public float moveSpeed = 90f;
    //Player max speed
    public float maxSpeed = 9f;
    //Player orientation
    public Transform playerOrientation;

    //Height of player character
    public float playerHeight = 2f;
    //The ground
    public LayerMask ground;
    //If the player is on the ground
    bool grounded;
    //Movement drag when player is on the ground
    public float groundDrag = 5f;

    //Force of jump
    public float jumpForce = 20f;
    //Move speed multiplier when in the air
    public float airResist = 0.2f;
    //If can jump
    bool canJump;
    //If can double jump
    // bool canDoubleJump;

    //Horizontal and vertical input  
    float hInput;
    float vInput;

    //Direction of movement
    Vector3 moveDirection;
    //Player rigidbody
    Rigidbody rb;
    //Reference to playerModel
    public GameObject playerModel;
    //Checks whats under the player
    public RaycastHit rayHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        //Check if user is on the ground
        
        /*
        if (Physics.Raycast(transform.position, Vector3.down, out rayHit, playerHeight * 0.5f + 0.2f)) {
            GameObject obj = rayHit.transform.gameObject;
            grounded = !(Physics.GetIgnoreCollision(obj.GetComponent<Collider>(), playerModel.GetComponent<Collider>()));
        } else {
            grounded = false;
        }
        */
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        //Get movement inputs
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        MaxSpeed();

        //When user is grounded
        if (grounded) {
            rb.drag = groundDrag;
            canJump = true;
            // canDoubleJump = true;
        }
        else {
            rb.drag = 0;
        }

        //Triggers double jump if player can double jump
        // if(Input.GetKeyDown("space") && canDoubleJump && !grounded) { 
        //     canDoubleJump = false;
        //     DoubleJump();
        // }
    }

    void FixedUpdate() {
        Move();

        if(Input.GetKey("space") && canJump && grounded) {
            Jump();
        }
    }

    //Sets max movespeed
    void MaxSpeed() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVel.magnitude > maxSpeed) {
            Vector3 limitedVel = flatVel.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    //Double jump action
    // void DoubleJump() {
    //     rb.velocity = new Vector3(0f, 0f, 0f);
    //     rb.AddForce(transform.up * jumpForce + (moveDirection * moveSpeed), ForceMode.Impulse);
    // }

    //Jump action
    void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        canJump = false;
    }

    //Movement action
    void Move() {
        moveDirection = playerOrientation.forward * vInput + playerOrientation.right * hInput;
        if (grounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        } else {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airResist, ForceMode.Force);
        }
    }
}
