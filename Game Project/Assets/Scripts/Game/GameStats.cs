using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static int deathCount = 0;
    [SerializeField] GameObject interstitalAds;
    
    public TextMeshProUGUI timerText; // UI'deki Text nesnesi
    public static float elapsedTime = 0f;
    public static bool isRunning = false;


    private void Start()
    {
        StartTimer();
        
    }

    
    private void Update()
    {
        if (deathCount == 3)
        {
            deathCount = 0;
            interstitalAds.SetActive(true);
        }

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

    public static void AddDeath()
    {
        deathCount++;
        StopTimer();     
        Debug.Log("Ölüm Sayısı: " + deathCount);
    }

  

    public void SkipButton()
    {
        interstitalAds.SetActive(false);
    }

    public void StartTimer()
    {
        isRunning = true;
        elapsedTime = 0f;
    }

    public static void StopTimer()
    {
        isRunning = false;
    }

    public static float GetElapsedTime()
    {
        return elapsedTime;
    }
}
