using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public void OpenPausePanel()
    {
        pauseIconFalse();
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        UI.SetActive(false);
    }

    public void Resume()
    {
        
        StartCoroutine(ResumeWithCountdown());
        
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
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
            countdownText.text= i.ToString(); // Geri sayım metnini güncelle
            yield return new WaitForSecondsRealtime(1f); // 1 saniye bekle (Time.timeScale = 0 olsa bile çalışır)
        }
        
        pauseScreen.SetActive(false);
        
        countdownText.gameObject.SetActive(false); // Geri sayımı gizle
         // Pause menüsünü kapat
        Time.timeScale = 1f; // Oyunu devam ettir
        
        pauseIconTrue();
        

        resumeButton.interactable = true;
        mainButton.interactable = true;
        restartButton.interactable = true;
       
        UI.SetActive(true);
    }
}
