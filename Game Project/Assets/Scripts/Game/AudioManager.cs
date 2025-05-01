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
    public Slider MusicvolumeSlider;    // Ses seviyesi Slider'ı
    public Slider SFXvolumeSlider;


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
        if (PlayerPrefs.HasKey("Music") && PlayerPrefs.HasKey("SFX"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }
    public void SetMusicVolume()
    {
        float volume = MusicvolumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = SFXvolumeSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        MusicvolumeSlider.value = PlayerPrefs.GetFloat("Music");
        SFXvolumeSlider.value = PlayerPrefs.GetFloat("SFX");
    }

    public void SFXPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}