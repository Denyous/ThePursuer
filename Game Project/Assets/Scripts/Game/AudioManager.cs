using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("----AudioSource----")]

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFX;

    [Header("-----AudioSettings----")]

    public AudioMixer audioMixer;  // Audio Mixer bağlayacağız
    public Slider volumeSlider;    // Ses seviyesi Slider'ı


    [Header("----SFX----")]

    public AudioClip background;
    public AudioClip death;
    public AudioClip explosion;
    public AudioClip shield;
    public AudioClip shieldOff;
    public AudioClip flare;
    public AudioClip laser;
    public AudioClip blackHole;
    public AudioClip blackholePauseScreen;
    public AudioClip kamizkaze;
    public AudioClip speedUp;
    public AudioClip collect;
    public AudioClip gameoverScreen;
    public AudioClip clickSound;
    public AudioClip clockTick;
    public AudioSource enemyShip;
    public AudioClip enemyShipClip;


    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }


    }
    public void SetMusicVolume()
    {
        float volume = volumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void SFXPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}