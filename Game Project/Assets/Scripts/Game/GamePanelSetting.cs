using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanelSetting : MonoBehaviour
{
    public GameObject pauseIcon;
    public GameObject pauseScreen;
    public GameObject UI;

    public TextMeshProUGUI countdownText;

    public Button resumeButton;
    public Button restartButton;
    public Button mainButton;
    public AudioSource musicSource;
    public AudioManager audioManager;
    
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OpenPausePanel()
    {
        ClickSound();
        pauseIconFalse();
        Time.timeScale = 0f;
        musicSource.Pause();
        pauseScreen.SetActive(true);
        UI.SetActive(false);
    }

    public void Resume()
    {
        ClickSound();
        StartCoroutine(ResumeWithCountdown());

    }

    public void Replay()
    {
        ClickSound();

        Time.timeScale = 1f;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Tamamen siyah oldu
        color.a = 1f;
        fadeImage.color = color;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        ClickSound();

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void pauseIconFalse()
    {

        pauseIcon.SetActive(false);

    }

    void pauseIconTrue()
    {
        pauseIcon.SetActive(true);
    }

    IEnumerator ResumeWithCountdown()
    {

        resumeButton.interactable = false;
        mainButton.interactable = false;
        restartButton.interactable = false;

        countdownText.gameObject.SetActive(true); // Geri sayımı göster

        for (int i = 2; i > 0; i--)
        {
            audioManager.SFXPlayOneShot(audioManager.pauseScreenClockTickSFX);
            countdownText.text = i.ToString(); // Geri sayım metnini güncelle
            yield return new WaitForSecondsRealtime(1f); // 1 saniye bekle (Time.timeScale = 0 olsa bile çalışır)
        }

        pauseScreen.SetActive(false);

        countdownText.gameObject.SetActive(false); // Geri sayımı gizle
                                                   // Pause menüsünü kapat
        Time.timeScale = 1f; // Oyunu devam ettir

        musicSource.UnPause();

        pauseIconTrue();


        resumeButton.interactable = true;
        mainButton.interactable = true;
        restartButton.interactable = true;

        UI.SetActive(true);
    }

    void ClickSound()
    {
        audioManager.SFXPlayOneShot(audioManager.buttonClickSFX);
    }
}
