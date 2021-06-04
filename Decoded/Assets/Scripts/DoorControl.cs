using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("Max Distance - RaycastHit")]
    public float MaxDistance = 6;
    [Header("Keybinds")]
    [SerializeField] KeyCode openDoorKey = KeyCode.F;
    private Animator anim;
    [Header("Sound")]
    public AudioSource sound;
    private bool opened = false;
    RaycastHit doorhit;
    public void Update()
    {
        if (Input.GetKeyDown(openDoorKey))
        {
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out doorhit, MaxDistance))
            {
                if (doorhit.transform.tag == "Door")
                {
                    Close();
                    Invoke("Close", 3);
                }
            }
        }
    }

    void Close()
    {
        sound.Play();
        opened = !opened;
        anim = doorhit.transform.GetComponentInParent<Animator>();
        anim.SetBool("Opened", opened);
    }
}

