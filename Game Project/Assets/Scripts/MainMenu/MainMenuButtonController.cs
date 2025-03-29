using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public GameObject languagePanel;
    public GameObject tutorialPanel;
    public GameObject audioPanel;
    public new AudioManager audio;
    

    [SerializeField] private UnityEngine.UI.Button StartButton;
    [SerializeField] private UnityEngine.UI.Button SettingsButton;
    [SerializeField] private UnityEngine.UI.Button ExitButton;
    [SerializeField] private UnityEngine.UI.Button CreditsButton;

    private RectTransform StartBtnRectTransform;
    private RectTransform SettingsBtnRectTransform;
    private RectTransform ExitBtnRectTransform;
    private Vector3 StartBtnOriginalScale;
    private Vector3 SettingsBtnOriginalScale;
    private Vector3 ExitBtnOriginalScale;

    private void Start()
    {
        Application.targetFrameRate = 60;
        StartBtnRectTransform = StartButton.GetComponent<RectTransform>();
        SettingsBtnRectTransform = SettingsButton.GetComponent<RectTransform>();
        ExitBtnRectTransform = ExitButton.GetComponent<RectTransform>();
        audio = GameObject.FindGameObjectWithTag("MainAudio").GetComponent<AudioManager>();
    }

    public void StartButtonClick()
    {
        
        StartBtnOriginalScale = StartButton.GetComponent<RectTransform>().localScale;

        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.scale(StartBtnRectTransform, StartBtnOriginalScale * 0.8f, 0.08f)
    .setEase(LeanTweenType.easeInQuart));
        sequence.append(LeanTween.scale(StartBtnRectTransform, StartBtnOriginalScale, 0.08f)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(OnStartBtnAnimationComplete));
        ClickSound();
       
    }
    private void OnStartBtnAnimationComplete()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButtonClick()
    {
        ExitBtnOriginalScale = ExitButton.GetComponent<RectTransform>().localScale;

        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.scale(ExitBtnRectTransform, ExitBtnOriginalScale * 0.8f, 0.08f)
    .setEase(LeanTweenType.easeInQuart));
        sequence.append(LeanTween.scale(ExitBtnRectTransform, ExitBtnOriginalScale, 0.08f)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(ExitGame));
        ClickSound();
    }
    private void ExitGame()
    {
        Application.Quit();
    }


    public void SettingsButtonClick()
    {
        
        settingsPanel.SetActive(true);
        ClickSound();

    }

    public void TestButtonClick()
    {
       

    }
    public void CreditsButtonClick()
    {
        creditsPanel.SetActive(true);
        ClickSound();
    }

    public void LanguageButtonClick()
    {
        languagePanel.SetActive(true);
        ClickSound();
    }

    public void TutorialButtonClick()
    {
        tutorialPanel.SetActive(true);
        ClickSound();
    }

    public void AudioButtonClick()
    {
        audioPanel.SetActive(true);
        ClickSound();
    }


    public void LanguageBackButton()
    {
        languagePanel.SetActive(false);
        ClickSound();
    }

    public void TutorialBackButton()
    {
        tutorialPanel.SetActive(false);
        ClickSound();
    }

    public void AudioBackButton()
    {
        audioPanel.SetActive(false);
        ClickSound();
    }

    public void BackToMenuFromSettings()
    {
        settingsPanel.SetActive(false);
        ClickSound();
    }

    public void BackToMenuFromCredits()
    {
        creditsPanel.SetActive(false);
        ClickSound();
    }

    public void BackToMainMenu()
    {
        ClickSound();
        Invoke("Waitforseconds", 0.2f);
                       
    }

    public void ClickSound()
    {
        audio.SFXPlay(audio.clickSound);
    }

    void Waitforseconds()
    {
        SceneManager.LoadScene(0);
    }
}
