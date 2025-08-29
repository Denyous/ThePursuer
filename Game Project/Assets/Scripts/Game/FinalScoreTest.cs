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
        gameStatTimer =GameObject.Find("GameStatsManager").GetComponent<GameStats>();
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
        int t = startTimer;
        int s = startStars;

        while (t > 0 || s > 0)
        {
            if (t > 0)
            {
                t--;
                timerText.text = t.ToString();
                finalScore += startStars; // timer azalınca yıldız etkisi
            }

            if (s > 0)
            {
                s--;
                starText.text = s.ToString();
                finalScore += startTimer; // yıldız azalınca timer etkisi
            }

            scoreText.text = finalScore.ToString();

            yield return new WaitForSeconds(1f); // animasyon hızı
        }

        // Son kesin değer
        finalScore = startTimer * startStars;
        scoreText.text = finalScore.ToString();
    }
}

