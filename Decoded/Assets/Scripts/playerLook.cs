using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLook : MonoBehaviour
{
    [SerializeField] wallRun wr;

    [SerializeField] private float sensX = 90f;
    [SerializeField] private float sensY = 90f;

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRotation;
    float yRotation;

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MyInput();

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wr.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void MyInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        xRotation -= mouseY * sensY * multiplier;
        yRotation += mouseX * sensX * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
