using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int deathCount;
    System.Random random = new System.Random();
    public static GameStats instance;

    int number = 0;

    private void Awake()
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
        PlayerPrefs.SetInt("DeathCount", 0);
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        number = random.Next(3, 7);
    }

    public void AddDeath()
    {
        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);

        if (deathCount == number)
        {
            deathCount = 0;
            PlayerPrefs.SetInt("DeathCount", deathCount);
            AdsManager.instance.ShowInterstitialAd();
        }
    }

    public void ResetDeathCount()
    {
        deathCount = 0;
        PlayerPrefs.SetInt("DeathCount", deathCount);
    }
}
