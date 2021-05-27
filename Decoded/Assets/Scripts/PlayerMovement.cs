using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This code is based on:
        Plai-Dev (https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial) Movement + View + WallRun
            "Most of the things are the same and we followed his tutorial on Youtube." (https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u)
        DaniDevy (https://github.com/DaniDevy/FPS_Movement_Rigidbody/blob/master/PlayerMovement.cs) Movement
            "We used his crouch and sliding; although he has an Youtube channel, he didn't explained how 
            the code works, so we had to mix those codes and change a few things in order to make it work."
            "We also added a few conditions to make it work in the way we want."
*/

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [Header("References")]
    [SerializeField] Transform orientation; // Orientation
    [SerializeField] Transform groundCheck; // Ground Checking
    public Rigidbody rb; // Rigidbody

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 10f; // Default player movement speed 
    [SerializeField] float groundMovementMultiplier = 5f; // Multiplier while on ground
    [SerializeField] float airMovementMultiplier = 0.2f; // Multiplier while jumping/falling
    [SerializeField] float wallMovementMultiplier = 0.6f; // Multiplier while wall running

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 10f; // Minimum
    [SerializeField] float sprintSpeed = 14f; // Maximum
    [SerializeField] float acceleration = 10f; // Acceleration when sprinting

    [Header("Jumping")]
    [SerializeField] public float jumpForce = 8f;

    [Header("Drag")]
    [SerializeField] float groundDrag = 5f;
    [SerializeField] float airDrag = 0.5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space; // Jump
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift; // Sprint
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl; // Crouch

    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;
    public bool isGrounded;

    float horizontalMovement;
    float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    RaycastHit slopeHit;

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1f, 1);
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    bool crouching;

    public void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (isGrounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    public void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 1.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        return false;
    }

    public void Start()
    {
        rb.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        crouching = Input.GetKey(KeyCode.LeftControl);
        if (Input.GetKeyDown(crouchKey))
        {
            StartCrouch();
        } 
        else if (Input.GetKeyUp(crouchKey))
        {
            StopCrouch();
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (!gameObject.GetComponent<PlayerWallRun>().wallLeft && !gameObject.GetComponent<PlayerWallRun>().wallRight)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(transform.up * (jumpForce/2), ForceMode.Impulse);
            }
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey))
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        } 
        else
        {
            rb.drag = airDrag;
        }
    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {   
        if (isGrounded && !OnSlope() && !crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * groundMovementMultiplier, ForceMode.Acceleration);
        } 
        else if (isGrounded && OnSlope() && !crouching)
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * groundMovementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded && gameObject.GetComponent<PlayerWallRun>().wallLeft || gameObject.GetComponent<PlayerWallRun>().wallRight && !crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * wallMovementMultiplier, ForceMode.Acceleration);
        } 
        else if (!isGrounded && !gameObject.GetComponent<PlayerWallRun>().wallRight || !gameObject.GetComponent<PlayerWallRun>().wallRight && !crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && crouching && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * slideCounterMovement, ForceMode.Acceleration);
        }
        else if (isGrounded && crouching && OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f, ForceMode.Acceleration);
        }
    }
}
