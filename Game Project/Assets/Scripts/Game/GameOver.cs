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
    private int _score;
    private int _highScore;
    AudioManager audioManager;
    public AudioSource musicSource;
    
    public void Setup(int score, int highscore)
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        UI.SetActive(false);

        _score = score;
        _highScore = highscore;

        if (musicSource is not null)
        {
            musicSource.Pause();
        }

        Invoke("Wait1Second", 1f);
    }

    void Wait1Second()
    {
        audioManager.SFXPlayOneShot(audioManager.gameOverScreenSFX);
        
        gameObject.SetActive(true);


        LocalizedHighScore.Arguments = new object[] { _highScore };
        LocalizedScore.Arguments = new object[] { _score };

        HighScoreText.text = LocalizedHighScore.GetLocalizedString();
        PointText.text = LocalizedScore.GetLocalizedString();
    }
}
