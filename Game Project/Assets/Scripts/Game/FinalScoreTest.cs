using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScoreTest : MonoBehaviour
{
    [Header("--Objects--")] 

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI starText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] GameStats gameStatTimer;
    [SerializeField] Player starCount;

    [Header("--Variables--")]
    
    int stars;
    int timer;
    int finalScore;
    
    void Start()
    {
        gameStatTimer = GameObject.Find("GameStatsManager").GetComponent<GameStats>();
        starCount = GameObject.Find("Player").GetComponent<Player>();
    }

    public void ShowFinalScore()
    {
        // Ölüm anındaki değerleri kopyala (snapshot)
        timer = gameStatTimer.UpdateTimerUI();
        stars = starCount.Score;
        finalScore = 0;

        // UI’yı başlat
        timerText.text = timer.ToString();
        starText.text = stars.ToString();
        scoreText.text = finalScore.ToString();

        // Coroutine başlat
        StartCoroutine(AnimateScore(timer, stars));
    }

    private IEnumerator AnimateScore(int startTimer, int startStars)
    {
        int time = startTimer;
        int starCount = startStars;
        float interval;

        if (starCount > 0 && time > 0)
        {
            interval = startStars / startTimer;
        }
        else
        {
            interval = 0.5f;
        }

        starText.text = starCount.ToString();
        timerText.text = time.ToString();
        Debug.Log($"Final Score--------{time * starCount}");

        if (interval < 0.8f)
        {
            interval = 0.8f;
        }

        while (time > 0 || starCount > 0)
        {
            yield return new WaitForSeconds(interval);

            if (startStars != 0)
            {
                if (starCount > 0)
                {
                    starCount--;
                    if (time > 0)
                    {
                        time -= startTimer;
                        timerText.text = time.ToString();
                    }
                    starText.text = starCount.ToString();
                    finalScore += startTimer;
                }
            }
            else
            {
                if (time > 0)
                {
                    time -= 2;
                    timerText.text = time.ToString();
                }
            }
            scoreText.text = finalScore.ToString();
        }
        Debug.Log($"Calculated Final Score: {finalScore}");
    }
}

