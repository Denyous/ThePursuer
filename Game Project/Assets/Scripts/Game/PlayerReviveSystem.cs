using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerReviveSystem : MonoBehaviour
{
    private GameObject player;
    public GameObject gameOverScreen;
    public GameObject reviveButton;
    public GameObject UI;
    public FinalScoreTest finalScoreTest;

    [SerializeField] private TMPro.TextMeshProUGUI countdownText;

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
        AdsManager.Instance.ShowRewardedAd();
    }

    public void RevivePlayer()
    {
        finalScoreTest.SetTextBoxesActive();
        player.SetActive(true);
        player.transform.position = deathPosition;
        player.gameObject.GetComponent<Player>().StartShieldCoroutine();

        UI.SetActive(true);
        UISettings();
        GameStats.ResumeTimer();

        Time.timeScale = 1f;
        EnemySpawner spawner = player.GetComponentInChildren<EnemySpawner>();
        spawner.ExplodeEnemies();
        AudioManager.instance.FadeInMusic(2f);
    }

    public void ResumeAfterCountdown()
    {
        StartCoroutine(ResumeWithCountdown());
    }

    public IEnumerator ResumeWithCountdown()
    {
        countdownText.gameObject.SetActive(true);
        gameOverScreen.SetActive(false);

        for (int i = 2; i > 0; i--)
        {
            countdownText.text = i.ToString();
            AudioManager.instance.SFXPlayOneShot(AudioManager.instance.clockTickSFX);
            yield return new WaitForSeconds(0.5f);
            AudioManager.instance.SFXPlayOneShot(AudioManager.instance.clockTockSFX);
            yield return new WaitForSeconds(0.5f);
        }

        countdownText.gameObject.SetActive(false);
        UI.SetActive(true);
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


