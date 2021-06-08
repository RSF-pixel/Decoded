using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private bool countTime = false;
    private float timer = 0f;
    [SerializeField] Text clock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Start") && !countTime)
        {
            countTime = true;
        } 
        else if (other.CompareTag("Finish") && countTime)
        {
            Interface.showPanel = true;
            Interface.showText = "DEMO COMPLETE";
            countTime = false;
        }
    }

    void Update()
    {
        ShowTime(timer);
        if (countTime)
        {
            timer += Time.deltaTime;
        }
    }

    void ShowTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = Mathf.FloorToInt((time*1000)%1000);
        clock.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
