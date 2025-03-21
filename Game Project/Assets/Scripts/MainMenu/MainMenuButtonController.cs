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
        StartBtnRectTransform = StartButton.GetComponent<RectTransform>();
        SettingsBtnRectTransform = SettingsButton.GetComponent<RectTransform>();
        ExitBtnRectTransform = ExitButton.GetComponent<RectTransform>();
       
    }

    public void StartButtonClick()
    {
        StartBtnOriginalScale = StartButton.GetComponent<RectTransform>().localScale;

        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.scale(StartBtnRectTransform, StartBtnOriginalScale * 0.8f, 0.08f)
    .setEase(LeanTweenType.easeInQuart));
        sequence.append(LeanTween.scale(StartBtnRectTransform, StartBtnOriginalScale, 0.08f)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(OnStartBtnAnimationComplete));
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
    }
    private void ExitGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }


    public void SettingsButtonClick()
    {
        
        settingsPanel.SetActive(true);
    }

    public void CreditsButtonClick()
    {
        creditsPanel.SetActive(true);
    }

    public void LanguageButtonClick()
    {
        languagePanel.SetActive(true);
    }

    public void TutorialButtonClick()
    {
        tutorialPanel.SetActive(true);
    }

    public void AudioButtonClick()
    {
        audioPanel.SetActive(true);
    }


    public void LanguageBackButton()
    {
        languagePanel.SetActive(false);
    }

    public void TutorialBackButton()
    {
        tutorialPanel.SetActive(false);
    }

    public void AudioBackButton()
    {
        audioPanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

   





}
