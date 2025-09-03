using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;

public class FinalScoreTest : MonoBehaviour
{
    [Header("--Objects--")]

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI starText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] RectTransform panelRectTransform;
    [SerializeField] RectTransform pointImageRectTransform;
    [SerializeField] TMP_Text HighScoreText;

    Player player;

    public LocalizedString LocalizedScore;
    public LocalizedString LocalizedTime;
    public LocalizedString LocalizedStar;
    public LocalizedString LocalizedHighScore;


    [Header("--Variables--")]

    public float duration = 1f;
    public float tweenDuration = 0.5f;
    private bool completedTweenStar = false;
    private bool completedTweenTimer = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private IEnumerator WaitForSec(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void AnimateFinalScore(int timer, int stars, int finalScore, int highScore)
    {
        LeanTween.cancel(starText.gameObject);
        LeanTween.cancel(timerText.gameObject);
        LeanTween.cancel(scoreText.gameObject);
        LeanTween.cancel(HighScoreText.gameObject);
        WaitForSec(1f);

        if (stars == 0 || timer == 0)
        {
            if (stars > 0 && timer > 0)
            {
                finalScore = (stars * timer);
            }
            else if (timer > 0)
            {
                finalScore = timer * 1;
            }
            else
            {
                finalScore = 0;
            }
        }
        else
        {
            finalScore = (stars * timer);
        }

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
        }

        LocalizedScore.Arguments = new object[] { finalScore };
        LocalizedStar.Arguments = new object[] { stars };
        LocalizedTime.Arguments = new object[] { timer };
        LocalizedHighScore.Arguments = new object[] { highScore };
        scoreText.text = LocalizedScore.GetLocalizedString();
        timerText.text = LocalizedTime.GetLocalizedString();
        starText.text = LocalizedStar.GetLocalizedString();
        HighScoreText.text = LocalizedHighScore.GetLocalizedString();

        Debug.Log($"Timer: {timer} \n Stars: {stars}");
        Debug.Log($"Duration: {duration}");

        int initialStars = stars;
        int initialTimer = timer;
        
        AnimatePointText(initialStars, duration);
        AnimateTimerText(initialTimer, duration);

        AnimateScoreText(finalScore, duration + tweenDuration, highScore);
    }

    private void AnimateScoreText(float score, float duration, float highScore)
    {
        LeanTween.value(scoreText.gameObject, 0f, score, duration).setOnUpdate((float val) =>
        {
            int newScore = Mathf.FloorToInt(val);
            LocalizedScore.Arguments[0] = newScore;
            scoreText.text = LocalizedScore.GetLocalizedString();

            if (newScore >= highScore)
            {
                LocalizedHighScore.Arguments[0] = newScore;
                HighScoreText.text = LocalizedHighScore.GetLocalizedString();
            }

            if (completedTweenStar && completedTweenTimer)
            {
                ScalePanel();
            }
        })
        .setEase(LeanTweenType.linear)
        .setIgnoreTimeScale(true);
    }

    private void AnimateTimerText(float timer, float duration)
    {
        LeanTween.value(timerText.gameObject, timer, 0f, duration).setOnUpdate((float val) =>
        {
            int newTimer = Mathf.FloorToInt(val);

            LocalizedTime.Arguments[0] = newTimer;
            timerText.text = LocalizedTime.GetLocalizedString();
        })
        .setEase(LeanTweenType.linear)
        .setOnComplete(() => 
        { 
            timerText.gameObject.SetActive(false); 
            completedTweenTimer = true; 
        })
        .setIgnoreTimeScale(true);
    }

    private void AnimatePointText(float point, float duration)
    {
        LeanTween.value(starText.gameObject, point, 0f, duration).setOnUpdate((float val) =>
        {
            int newPoint = Mathf.FloorToInt(val);
            LocalizedStar.Arguments[0] = newPoint;
            starText.text = LocalizedStar.GetLocalizedString();
        })
        .setEase(LeanTweenType.linear)
        .setOnComplete(() => 
        { 
            starText.gameObject.SetActive(false); 
            completedTweenStar = true;
        })
        .setIgnoreTimeScale(true);
    }

    void ScalePanel()
    {
        LeanTween.moveY(panelRectTransform, panelRectTransform.anchoredPosition.y + 65, tweenDuration)
        .setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
        LeanTween.scale(pointImageRectTransform, new Vector3(12, 12, 1), tweenDuration).setIgnoreTimeScale(true);
        LeanTween.moveLocalX(scoreText.gameObject, scoreText.rectTransform.anchoredPosition.x + 30, tweenDuration).setIgnoreTimeScale(true);
    }

    public void UnScalePanel()
    {
        LeanTween.moveY(panelRectTransform, panelRectTransform.anchoredPosition.y - 150, tweenDuration)
        .setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
        LeanTween.scale(pointImageRectTransform, new Vector3(0,0,1), tweenDuration).setIgnoreTimeScale(true);
        LeanTween.moveLocalX(scoreText.gameObject, scoreText.rectTransform.anchoredPosition.x - 100, tweenDuration).setIgnoreTimeScale(true);
    }

    public void MakeTextBoxActive()
    {
        timerText.gameObject.SetActive(true);
        starText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.rectTransform.localScale = Vector3.one;
        starText.rectTransform.localScale = Vector3.one;
        scoreText.rectTransform.localScale = Vector3.one;
    }

    public void SetTextBoxesActive()
    {
        //LeanTween.cancelAll();
        timerText.gameObject.SetActive(true);
        starText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerText.rectTransform.localScale = Vector3.one;
        starText.rectTransform.localScale = Vector3.one;
        scoreText.rectTransform.localScale = Vector3.one;
        UnScalePanel();
    }

    public void ShowFinalScore()
    {
        int timer = (int)GameStats.GetElapsedTime();
        int stars = player.Score;
        int highScore = PlayerPrefs.GetInt("HighScore");
        int finalScore = 0;

        timerText.text = timer.ToString();
        starText.text = stars.ToString();
        scoreText.text = finalScore.ToString();

        timerText.gameObject.SetActive(true);
        starText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);

        AnimateFinalScore(timer, stars, finalScore, highScore);
    }
}

