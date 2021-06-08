using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("hey");
        foreach (Sound s in sounds)
        {
            Debug.Log("hey1");
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
        
    }
    public void Play (string name)
    {
        Debug.Log("hey2");
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
