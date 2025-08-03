using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class TutorialHandler : MonoBehaviour
{
    [Header("Tutorial Field")]
    
    [SerializeField] Image tutorialImage;
    [SerializeField] List<Sprite> tutorialSprites = new List<Sprite>();
    [SerializeField] List<string> tutorialTexts = new List<string>();

    [Header("Game Objects")]

    [SerializeField] GameObject tutorialFinished;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] GameObject tutorialSkip;
    [SerializeField] GameObject tutorialTemplate;
    [SerializeField] GameObject tutorialWelcome;
    private int count = 0;

    [Header("TextMeshs")]

    [SerializeField] TextMeshProUGUI text0;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    [SerializeField] TextMeshProUGUI text3;
    [SerializeField] TextMeshProUGUI text4;
    [SerializeField] TextMeshProUGUI text5;
    [SerializeField] TextMeshProUGUI text6;
    [SerializeField] TextMeshProUGUI text7;
    [SerializeField] TextMeshProUGUI text8;
    [SerializeField] TextMeshProUGUI text9;

    public void NextOnClicked()
    {
        if (count >= 0)
        {
            TutorialTextReplace();                    
            
            tutorialImage.sprite = tutorialSprites[count];
            tutorialText.text = tutorialTexts[count];

            if (count == 9)
            {
                tutorialFinished.SetActive(true);
            }
            count++;
        }
    }

    public void PrevOnClicked()
    {
        if (count > 0)
        {
            count--;
            tutorialImage.sprite = tutorialSprites[(count)];
            tutorialText.text = tutorialTexts[count];
        }
    }

    public void OkButtonClicked()
    {
        Invoke("GoToMenu", 0.1f);
    }

    void GoToMenu()
    {
        SceneManager.LoadScene("Game");
        GameStats.StartTimer();
    }

    public void SkipButtonOnClicked()
    {
        tutorialSkip.SetActive(true);
    }

    public void YesButtonClicked()
    {
        SceneManager.LoadScene("Game");
        GameStats.StartTimer();
    }

    public void NoButtonClicked()
    {
        tutorialSkip.SetActive(false);
    }

    public void GoToTutorialButtonClicked()
    {
        tutorialWelcome.SetActive(false);
        tutorialTemplate.SetActive(true);
        
        tutorialImage.sprite = tutorialSprites[count];
        TutorialTextReplace();
        tutorialText.text = tutorialTexts[count];
    }

    void TutorialTextReplace()
    {
        switch (count)
        {
            case 0: tutorialTexts[0] = text0.text; break;
            case 1: tutorialTexts[1] = text1.text; break;
            case 2: tutorialTexts[2] = text2.text; break;
            case 3: tutorialTexts[3] = text3.text; break;
            case 4: tutorialTexts[4] = text4.text; break;
            case 5: tutorialTexts[5] = text5.text; break;
            case 6: tutorialTexts[6] = text6.text; break;
            case 7: tutorialTexts[7] = text7.text; break;
            case 8: tutorialTexts[8] = text8.text; break;
            case 9: tutorialTexts[9] = text9.text; break;

            default: break;
        }
    }
}