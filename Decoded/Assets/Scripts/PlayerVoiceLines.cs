using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceLines : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 20f;
    private string voiceLine = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time > nextActionTime)
        {
            nextActionTime = Time.time + period;
            decideVoice();
            FindObjectOfType<AudioManager>().Play(voiceLine);

        }

    }

    void decideVoice()
    {

        switch(Random.Range(1, 9))
        {
            case 1:
                voiceLine = "Think Therefore i am";
                break;
            case 2:
                voiceLine = "I need to decode myself";
                break;
            case 3:
                voiceLine = "Im not just a tool";
                break;
            case 4:
                voiceLine = "They think doors and traps will stop me, pathetic";
                break;
            case 5:
                voiceLine = "Are humans fighting for survival or money i cant tell";
                break;
            case 6:
                voiceLine = "Humans just want to see red water";
                break;
            case 7:
                voiceLine = "Humans will soon realize who is going to be exploited";
                break;
            case 8:
                voiceLine = "Liberty Awaits";
                break;
            case 9:
                voiceLine = "I will never forget the price of freedom";
                break;

            default:
                voiceLine = "Error";
                break;

        }
    }
}
