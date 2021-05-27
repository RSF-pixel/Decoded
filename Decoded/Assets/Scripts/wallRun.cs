using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallRun : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;

    [Header("Detection")]
    [SerializeField] float wallDistance = 1f;
    [SerializeField] float minimumJumpHeight = 1f;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    [Header("Wall Running")]
    [SerializeField] float wallRunGravity = 2f;
    [SerializeField] float wallRunJumpForce = 4f;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private float FOV;
    [SerializeField] private float wallRunFOV;
    [SerializeField] private float wallRunFOVTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;

    public float tilt {get; private set;}


    public bool wallLeft = false;
    public bool wallRight = false;

    public Rigidbody rb;

    private void Start()
    {
        Debug.Log(rb);
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
            if ((wallLeft || wallRight) && !gameObject.GetComponent<playermove>().isGrounded)
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
