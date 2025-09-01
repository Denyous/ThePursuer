using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text HighScoreText;
    public LocalizedString LocalizedHighScore;
    public LocalizedString LocalizedScore;
    public GameObject UI;
    public GameObject pauseIcon;
    public GameObject reviveButton;
    public TextMeshProUGUI timerScore;
    private int _score;
    private int _highScore;
    AudioManager audioManager;
    public AudioSource musicSource;
    public PlayerReviveSystem reviveSystem;
    private FinalScoreTest finalScore;

    public void Setup(int score, int highscore)
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        finalScore = GameObject.FindGameObjectWithTag("FinalScore").GetComponent<FinalScoreTest>();

        UI.SetActive(false);
        finalScore.MakeTextBoxActive();
        TimerScoreText();

        _score = score;
        _highScore = highscore;

        if (musicSource is not null)
        {
            musicSource.Pause();
        }

        Invoke("SecondStep", 1f);
    }

    private void SecondStep()
    {
        audioManager.SFXPlayOneShot(audioManager.gameOverScreenSFX);

        gameObject.SetActive(true);

        LocalizedHighScore.Arguments = new object[] { _highScore };
        //LocalizedScore.Arguments = new object[] { _score };

        HighScoreText.text = LocalizedHighScore.GetLocalizedString();
        //PointText.text = LocalizedScore.GetLocalizedString();
        finalScore.ShowFinalScore();
    }


    void TimerScoreText()
    {
        int minutes = Mathf.FloorToInt(GameStats.elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(GameStats.elapsedTime % 60f);
        timerScore.text = $"{minutes:00}:{seconds:00}";
    }
}
