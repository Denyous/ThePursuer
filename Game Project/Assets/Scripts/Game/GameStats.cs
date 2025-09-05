using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int deathCount;
    public TextMeshProUGUI timerText; // UI'deki Text nesnesi
    public static float elapsedTime = 0f;
    public static bool isRunning = false;
    public static int adsNumber;

    private void Awake()
    {
        deathCount = PlayerPrefs.GetInt("DeathCount");
        System.Random random = new System.Random();
        adsNumber = PlayerPrefs.GetInt("AdsNumber", 0);
        if (adsNumber == 0)
        {
            adsNumber = random.Next(3, 6);
            PlayerPrefs.SetInt("AdsNumber", adsNumber);
        }
    }

    private void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    public int UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        return seconds;
    }

    public static void AddDeath()
    {
        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);
        StopTimer();
        Debug.Log("Death Count: " + deathCount);
        Debug.Log("Ads Number: " + adsNumber);
        if (deathCount >= adsNumber)
        {
            deathCount = 0;
            adsNumber = 0;
            PlayerPrefs.SetInt("AdsNumber", adsNumber);
            PlayerPrefs.SetInt("DeathCount", deathCount);
            AdsManager.instance.ShowInterstitialAd();
        }
    }

    public static void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public static void StopTimer()
    {
        isRunning = false;
    }

    public static void ResumeTimer()
    {
        isRunning = true;
    }

    public static float GetElapsedTime()
    {
        return elapsedTime;
    }

    public static void ResetDeathCount()
    {
        deathCount = 0;
        PlayerPrefs.SetInt("DeathCount", deathCount);

        System.Random random = new System.Random();
        adsNumber = PlayerPrefs.GetInt("AdsNumber", 0);
        if (adsNumber == 0)
        {
            adsNumber = random.Next(3, 6);
            PlayerPrefs.SetInt("AdsNumber", adsNumber);
        }
    }
}
