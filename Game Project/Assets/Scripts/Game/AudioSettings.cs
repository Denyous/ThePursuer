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
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f); // varsayılan %75
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);

        float soundVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        audioMixer.SetFloat("SFX", Mathf.Log10(soundVolume) * 20);
    }
}
