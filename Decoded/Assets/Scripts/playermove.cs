using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermove : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 10f;
    float groundMovementMultiplier = 5f;
    float airMovementMultiplier = 0.2f;
    float wallMovementMultiplier = 0.6f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float sprintSpeed = 14f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 8f;

    [Header("Drag")]
    float groundDrag = 5f;
    float airDrag = 0.5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    float groundDistance = 0.4f;
    public bool isGrounded;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    public Rigidbody rb;

    RaycastHit slopeHit;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale = new Vector3(1, 1f, 1);
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    bool crouching;

    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

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
        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (!gameObject.GetComponent<wallRun>().wallLeft && !gameObject.GetComponent<wallRun>().wallRight)
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("AAA");
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
            Debug.Log("1");
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * groundMovementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded && gameObject.GetComponent<wallRun>().wallLeft || gameObject.GetComponent<wallRun>().wallRight && !crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * wallMovementMultiplier, ForceMode.Acceleration);
        } 
        else if (!isGrounded && !gameObject.GetComponent<wallRun>().wallRight || !gameObject.GetComponent<wallRun>().wallRight && !crouching)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
        }


        //Slow down sliding
        else if (isGrounded && crouching && !OnSlope())
        {
            Debug.Log("2");
            rb.AddForce(moveDirection.normalized * moveSpeed * slideCounterMovement, ForceMode.Acceleration);
        }
        else if (isGrounded && crouching && OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 20f, ForceMode.Acceleration);
            Debug.Log("3");
        }
    }
}
