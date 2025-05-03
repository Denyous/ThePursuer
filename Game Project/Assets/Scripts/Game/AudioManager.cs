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
    public Slider musicVolumeSlider;    // Ses seviyesi Slider'ı
    public Slider sfxVolumeSlider;


    [Header("----Audio Sources----")]
    [SerializeField] private AudioSource playerShipAudioSource;


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
        float volume = musicVolumeSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = sfxVolumeSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        if (musicVolumeSlider != null && sfxVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("Music");
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFX");
        }
    }

    public void SFXPlayOneShot(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}