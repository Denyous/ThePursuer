using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    public TMP_Text PointText;
    public TMP_Text HighScoreText;
    public GameObject UI;
    public GameObject pauseIcon;
    private int _score;
    private int _highScore;
    AudioManager audioManager;
    public AudioSource musicSource;

    
    public void Setup(int score, int highscore)
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        
        UI.SetActive(false);
        pauseIcon.SetActive(false);

        _score = score;
        _highScore = highscore;
        
        musicSource.Pause();
        Invoke("Wait1Second", 1f);
    }

    void Wait1Second()
    {
        audioManager.SFXPlayOneShot(audioManager.gameOverScreenSFX);
        
        gameObject.SetActive(true);
        PointText.text = _score.ToString() + " Points";
        HighScoreText.text = "High Score: " + _highScore.ToString() + " Points";
    }
}
