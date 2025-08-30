using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerReviveSystem : MonoBehaviour
{
    private GameObject player;
    public GameObject gameOverScreen;
    public GameObject reviveButton;
    public GameObject UI;
    public FinalScoreTest finalScoreTest;


    public bool reviveUsed = false;
    private Vector3 deathPosition;
    public static PlayerReviveSystem instance;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        finalScoreTest = GameObject.FindGameObjectWithTag("_GM").GetComponent<FinalScoreTest>();
    }

    public void OnReviveButtonPressed()
    {
        AdsManager.instance.ShowRewardedAd();
        finalScoreTest.SetTextBoxesActive();
    }

    public void RevivePlayer()
    {
        player.SetActive(true);
        player.transform.position = deathPosition;

        UI.SetActive(true);
        UISettings();
        GameStats.ResumeTimer();
        AudioListener listener = Camera.main.GetComponent<AudioListener>();

        if (listener is not null)
        {
            listener.enabled = false;
        }
    }

    //public void KillPlayer()
    //{
    //    int highscore = PlayerPrefs.GetInt("HighScore");
    //    int score;
    //    GameOver GameOverScreen = gameOverScreen.GetComponent<GameOver>();
    //    Player playerScript = player.GetComponent<Player>();
    //    score = playerScript.GetScore();

    //    if (highscore < score)
    //    {
    //        highscore = score;
    //        PlayerPrefs.SetInt("HighScore", highscore);
    //    }

    //    GameOverScreen.Setup(score, highscore);
    //    GameStats.AddDeath();
    //    gameOverScreen.SetActive(true);
    //    reviveButton.SetActive(false);
    //    player.SetActive(false);
    //    reviveUsed = false;
    //}

    public void UISettings()
    {
        reviveUsed = true;

        GameStats.ResetDeathCount();

        gameOverScreen.SetActive(false);

        if (!reviveUsed)
        {
            reviveButton.SetActive(true);
        }
        else
        {
            reviveButton.SetActive(false);
        }
    }

    public void ShowReviveButton()
    {
        if (reviveButton != null)
        {
            reviveButton.SetActive(true);
        }
    }

    public void HideReviveButton()
    {
        if (reviveButton != null)
        {
            reviveButton.SetActive(false);
        }
    }

    public void PlayerDiedLocation()
    {
        deathPosition = player.transform.position;
    }
}


