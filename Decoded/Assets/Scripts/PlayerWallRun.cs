using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    This code is based on:
        Plai-Dev (https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial) Movement + View + WallRun
            "Most of the things are the same and we followed his tutorial on Youtube." (https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u)
        DaniDevy (https://github.com/DaniDevy/FPS_Movement_Rigidbody) Movement
            "We used his crouch and sliding; although he has an Youtube channel, he didn't explained how 
            the code works, so we had to mix those codes and change a few things in order to make it work."
            "We also added a few conditions to make it work in the way we want."
*/

public class PlayerWallRun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] private Camera cam;
    public Rigidbody rb;

    [Header("Detection")]
    [SerializeField] float wallDistance = 1f;
    [SerializeField] float minimumJumpHeight = 1f;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [Header("Wall Running")]
    [SerializeField] float wallRunGravity = 2f;
    [SerializeField] float wallRunJumpForce = 4f;

    [Header("Camera")]
    [SerializeField] private float FOV = 60;
    [SerializeField] private float wallRunFOV = 70;
    [SerializeField] private float wallRunFOVTime = 20;
    [SerializeField] private float camTilt = 10;
    [SerializeField] private float camTiltTime = 20;

    public float tilt {get; private set;}


    public bool wallLeft = false;
    public bool wallRight = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    public void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {
        CheckWall();
        if (CanWallRun())
        {
            if ((wallLeft || wallRight) && !gameObject.GetComponent<PlayerMovement>().isGrounded)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFOV, wallRunFOVTime * Time.deltaTime);

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 80, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 80, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, FOV, wallRunFOVTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
