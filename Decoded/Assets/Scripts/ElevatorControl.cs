using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorControl : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("Max Distance - RaycastHit")]
    public float MaxDistance = 4;
    [Header("Keybinds")]
    [SerializeField] KeyCode openDoorKey = KeyCode.F;
    private Animator anim;
    private bool elevatorUp = false;
    RaycastHit elevator;
    public void Update()
    {
        if (Input.GetKeyDown(openDoorKey))
        {
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out elevator, MaxDistance))
            {
                if (elevator.transform.tag == "Elevator")
                {
                    elevatorUp = !elevatorUp;
                    anim = elevator.transform.GetComponentInParent<Animator>();
                    anim.SetBool("ElevatorUp", elevatorUp);
                }
            }
        }
    }

}
