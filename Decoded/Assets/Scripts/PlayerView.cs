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

public class PlayerView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cam; // Camera
    [SerializeField] Transform orientation; // Orientation
    [SerializeField] PlayerWallRun wallrun; // WallRun Camera

    [Header("Camera Settings")]
    [SerializeField] private float SensibilityX = 180f; // Sensibility - X axis
    [SerializeField] private float SensibilityY = 120f; // Sensibility - Y axis

    float mouseX;
    float mouseY;
    float multiplier = 0.01f;
    float xRotation;
    float yRotation;

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        xRotation -= mouseY * SensibilityY * multiplier;
        yRotation += mouseX * SensibilityX * multiplier;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallrun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
