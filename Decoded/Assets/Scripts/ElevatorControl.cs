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
    [Header("Sound")]
    public AudioSource elevatorSound;
    public AudioSource bingSound;
    private Animator anim;
    private bool elevatorUp = false;
    private bool flag = false;
    RaycastHit elevator;
    public void Update()
    {
        if (Input.GetKeyDown(openDoorKey))
        {
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out elevator, MaxDistance))
            {
                if (elevator.transform.tag == "Elevator" && !flag)
                {
                    flag = !flag;
                    elevatorSound.Play();
                    elevatorUp = !elevatorUp;
                    anim = elevator.transform.GetComponentInParent<Animator>();
                    anim.SetBool("ElevatorUp", elevatorUp);
                    Invoke("Bing", 3);
                }
            }
        }
    }
    
    void Bing()
    {
        bingSound.Play();
        flag = !flag;
    }

}
