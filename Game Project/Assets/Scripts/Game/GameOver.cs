using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameOver : MonoBehaviour
{
    public TMP_Text PointText;
    public GameObject UI;
    public GameObject pauseIcon;
    private int score_;
    AudioManager audioManager;

    
    public void Setup(int score)
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
        UI.SetActive(false);
        pauseIcon.SetActive(false);

        score_ = score;
        

        Invoke("Wait1Second", 1f);
        
    }

    void Wait1Second()
    {
        audioManager.SFXPlay(audioManager.gameoverScreen);
        gameObject.SetActive(true);
        PointText.text = score_.ToString() + " Stars";

    }
   
}
