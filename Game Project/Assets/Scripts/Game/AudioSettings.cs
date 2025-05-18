using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("Music", 0.75f);
        float dbMusic = Mathf.Log10(Mathf.Clamp(musicVolume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Music", dbMusic);
        
        float soundVolume = PlayerPrefs.GetFloat("SFX", 0.75f);
        float dbSFX = Mathf.Log10(Mathf.Clamp(soundVolume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFX", dbSFX);
    }
}
