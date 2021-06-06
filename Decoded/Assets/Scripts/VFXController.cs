using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject vfx;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Se a velocidade for mais que 3 as particulas começam  
        if((rb.velocity.x >3 || rb.velocity.y > 3 || rb.velocity.z > 3 ) || (rb.velocity.x < -3 || rb.velocity.y < -3 || rb.velocity.z < -3))
        {
            Debug.Log(rb.velocity);
            vfx.SetActive(true);
             
        }
        else
        {
            vfx.SetActive(false);
        }
        
    }
}
