using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("----AudioSource----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFX;

    [Header("-----AudioSettings----")]
    [SerializeField] AudioMixer audioMixer;  

    [Header("----Audio Clips----")]
    public AudioClip explosionSFX;
    public AudioClip shieldUpSFX;
    public AudioClip shieldImpactSFX;
    public AudioClip flareSFX;
    public AudioClip laserSFX;
    public AudioClip blackHoleInstantiationSFX;
    public AudioClip speedUpSFX;
    public AudioClip speedDownSFX;
    public AudioClip collectStarSFX;
    public AudioClip collectBuffSFX;
    public AudioClip highScoreAnimationSFX;

    public AudioClip buttonClickSFX;
    public AudioClip clockTickSFX;
    public AudioClip clockTockSFX;

    public static AudioManager instance;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableSound()
    {
        musicSource.enabled = true;
        SFX.enabled = true;
    }
    
    public void SetMusicVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("Music", dB);
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();

        if (!musicSource.isPlaying && audioMixer != null)
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

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public void AdjustMusicVolume(float volume)
    {
        musicSource.volume = volume;    
    }

    public IEnumerator FadeInMusicCoroutine(float duration)
    {
        float time = 0f;
        musicSource.volume = 0f;
        musicSource.Play();

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
        musicSource.volume = 1f;
    }

    public IEnumerator FadeOutMusicCoroutine(float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }
}