using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

    private void Start()
    {
        tutorialImage.sprite = tutorialSprites[count];
        tutorialText.text = tutorialTexts[count];
    }

    public void NextOnClicked()
    {
        if (count >= 0)
        {
            count++;
            tutorialImage.sprite = tutorialSprites[count];
            tutorialText.text = tutorialTexts[count];

            if (count == 9)
            {
                tutorialFinished.SetActive(true);
            }
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
    }


    public void SkipButtonOnClicked()
    {
        tutorialSkip.SetActive(true);
    }


    public void YesButtonClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void NoButtonClicked()
    {
        tutorialSkip.SetActive(false);
    }

    public void GoToTutorialButtonClicked()
    {
        tutorialWelcome.SetActive(false);
        tutorialTemplate.SetActive(true);
    }
}