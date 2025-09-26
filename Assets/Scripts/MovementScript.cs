using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    //Player move speed
    public float moveSpeed = 70f;
    //Player max speed
    public float maxSpeed = 7f;
    //Player orientation
    public Transform playerOrientation;

    //Height of player character
    public float playerHeight = 2f;
    //The ground
    public LayerMask ground;
    //If the player is on the ground
    bool grounded;
    //If the player is moving
    float old_pos;
    //Movement drag when player is on the ground
    public float groundDrag = 5f;

    //Force of jump
    public float jumpForce = 40f;
    //Move speed multiplier when in the air
    public float airResist = 0.2f;
    //If can jump
    bool canJump;
    //If can double jump
    // bool canDoubleJump;

    //Horizontal and vertical input  
    float hInput;
    float vInput;

    //Input Actions
    private InputActions inputActions;
    private InputAction walk;
    private InputAction jump;

    //Direction of movement
    Vector3 moveDirection;
    //Player rigidbody
    Rigidbody rb;
    //Reference to player
    public GameObject player;
    //Checks whats under the player
    public RaycastHit rayHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new InputActions();
        walk = inputActions.Movement.Walk;
        jump = inputActions.Movement.Jump;
        inputActions.Enable();
        old_pos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if user is on the ground
        if (Physics.Raycast(transform.position, Vector3.down, out rayHit, playerHeight * 0.5f + 0.2f)) {
            GameObject obj = rayHit.transform.gameObject;
            if (obj.GetComponent<Collider>() != null) {
                grounded = !(Physics.GetIgnoreCollision(obj.GetComponent<Collider>(), player.GetComponent<Collider>()));
            } else {
                grounded = false;
            }
        } else {
            grounded = false;
        }
        //Get movement inputs
        Vector2 v2 = walk.ReadValue<Vector2>();
        hInput = v2.x;
        vInput = v2.y;

        MaxSpeed();

        //When user is grounded
        if (grounded) {
            rb.drag = groundDrag;
            canJump = true;
          
        }
        else {
            rb.drag = 0;
            
        }

        // Check if player is moving or stopped
        if(old_pos != transform.position.x && grounded)
        {
            if(!AudioController.aCtrl.GetSound("playerWalking").isPlaying)
            {
                AudioController.aCtrl.GetSound("playerWalking").volume = AudioController.aCtrl.GetVolume() / 10;
                AudioController.aCtrl.GetSound("playerWalking").Play();
            }
        } else

        {
            AudioController.aCtrl.GetSound("playerWalking").Pause();
        }
        
        old_pos = transform.position.x;
    }

    void FixedUpdate() {
        Move();
        if (canJump && grounded) {
            jump.started += Jump;
           
        } else {
            jump.started -= Jump;  
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

    //Jump action
    void Jump(InputAction.CallbackContext obj) {
        // Play Jump sound
        AudioController.aCtrl.GetSound("playerWalking").Stop();
        AudioController.aCtrl.GetSound("playerJumping").Play();

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
