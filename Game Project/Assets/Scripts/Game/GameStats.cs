using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int deathCount;
    public TextMeshProUGUI timerText; // UI'deki Text nesnesi
    public float elapsedTime = 0f;
    public bool isRunning = false;
    public static GameStats instance;
    public int adsNumber;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        deathCount = 0;
        System.Random random = new System.Random();
        adsNumber = random.Next(3, 6);
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

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void AddDeath()
    {
        deathCount++;
        StopTimer();     

        if (deathCount == adsNumber)
        {
            deathCount = 0;
            AdsManager.instance.ShowInterstitialAd();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void ResetDeathCount()
    {
        deathCount = 0;
        PlayerPrefs.SetInt("DeathCount", deathCount);
    }
}
