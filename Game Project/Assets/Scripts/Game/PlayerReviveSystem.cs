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
        finalScoreTest = GameObject.FindGameObjectWithTag("FinalScore").GetComponent<FinalScoreTest>();
    }

    public void OnReviveButtonPressed()
    {
        AdsManager.instance.ShowRewardedAd();
    }

    public void RevivePlayer()
    {
        finalScoreTest.SetTextBoxesActive();
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

        Time.timeScale = 1f;
        EnemySpawner spawner = player.GetComponentInChildren<EnemySpawner>();
        spawner.ExplodeEnemies();
    }

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


