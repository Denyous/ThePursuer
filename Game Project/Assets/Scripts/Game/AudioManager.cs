using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    [Header("----AudioSource----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFX;

    [Header("-----AudioSettings----")]
    public AudioMixer audioMixer;  

    [Header("----Audio Clips----")]
    public AudioClip explosionSFX;
    public AudioClip shieldUpSFX;
    public AudioClip shieldImpactSFX;
    public AudioClip flareSFX;
    public AudioClip laserSFX;
    public AudioClip blackHoleInstantiationSFX;
    public AudioClip enemiesSwallowedByBlackHoleSFX;
    public AudioClip speedUpSFX;
    public AudioClip speedDownSFX;
    public AudioClip collectStarSFX;
    public AudioClip collectBuffSFX;

    public AudioClip gameOverScreenSFX;
    public AudioClip buttonClickSFX;
    public AudioClip pauseScreenClockTickSFX;
    
    public void SetMusicVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Music", dB);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFX", dB);
        PlayerPrefs.SetFloat("SFX", volume);
        PlayerPrefs.Save();
    }

    public void SFXPlayOneShot(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}