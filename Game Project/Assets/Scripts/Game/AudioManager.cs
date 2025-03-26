using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("----AudioSource----")]

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFX;

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

    public void SFXPlay(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }
}