using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    // Based on a tutorial from Brackeys :
    // https://www.youtube.com/watch?v=6OT43pvUyfY

    public Sound[] sounds;

    
    void Awake()
    {
        
        foreach (Sound s in sounds)
        {
          
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

        }
        
    }
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
