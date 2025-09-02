using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    int stars;
    int timer;
    int finalScore;
    int highScore;

    public float duration = 5f;
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

    public void AnimateFinalScore(int startTimer, int startStars, int highScore)
    {
        LeanTween.cancel(starText.gameObject);
        LeanTween.cancel(timerText.gameObject);
        WaitForSec(1f);
        timer = startTimer;
        stars = startStars;
        this.highScore = highScore;
        int lastTimer = timer;

        finalScore = 0;
        LocalizedScore.Arguments = new object[] { finalScore };
        LocalizedStar.Arguments = new object[] { stars };
        LocalizedTime.Arguments = new object[] { timer };
        LocalizedHighScore.Arguments = new object[] { highScore };
        scoreText.text = LocalizedScore.GetLocalizedString();
        timerText.text = LocalizedTime.GetLocalizedString();
        starText.text = LocalizedStar.GetLocalizedString();
        HighScoreText.text = LocalizedHighScore.GetLocalizedString();

        Debug.Log($"Timer: {timer} \n Stars: {stars}");

        duration = (float)(stars / timer);

        if (duration < 1)
        {
            duration = 1;
        }
        Debug.Log($"Duration: {duration}");

        int initialStars = stars;
        int initialTimer = timer;

        if (initialStars > 0)
        {
            LeanTween.value(starText.gameObject, initialStars, 0f, duration).setOnUpdate((float val) =>
            {
                int newStars = Mathf.RoundToInt(val);
                int decrease = initialStars - newStars;

                if (newStars < stars)
                {
                    int step = stars - newStars;
                    finalScore += step * timer;
                    stars = newStars;
                    LocalizedStar.Arguments[0] = stars;
                    LocalizedScore.Arguments[0] = Convert.ToInt32(finalScore);

                    starText.text = LocalizedStar.GetLocalizedString();
                    scoreText.text = LocalizedScore.GetLocalizedString();

                    if (finalScore > highScore)
                    {
                        LocalizedHighScore.Arguments[0] = finalScore;
                        HighScoreText.text = LocalizedHighScore.GetLocalizedString();
                        PlayerPrefs.SetInt("HighScore", finalScore);
                    }

                    if (stars <= 0)
                    {
                        completedTweenStar = true;
                        LeanTween.cancel(starText.gameObject);
                        LeanTween.scale(starText.gameObject, Vector3.zero, 0.2f)
                            .setEase(LeanTweenType.easeInBack)
                            .setOnComplete(() => starText.gameObject.SetActive(false)).setIgnoreTimeScale(true);
                    }
                }
            }).setEase(LeanTweenType.linear).setIgnoreTimeScale(true);
        }
        else
        {
            completedTweenStar = true;
            LeanTween.cancel(starText.gameObject);
            LeanTween.scale(starText.gameObject, Vector3.zero, 0.2f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => starText.gameObject.SetActive(false)).setIgnoreTimeScale(true);
        }

        if (initialTimer > 0)
        {
            LeanTween.value(timerText.gameObject, initialTimer, 0f, duration).setOnUpdate((float val) =>
            {
                int newTimer = Mathf.FloorToInt(val);
                if (newTimer < lastTimer)
                {
                    int decrease = lastTimer - newTimer;

                    if (stars > 0)
                    {
                        finalScore += decrease * stars;
                        LocalizedScore.Arguments[0] = Convert.ToInt32(finalScore);
                    }
                    else
                    {
                        finalScore += decrease;
                        LocalizedScore.Arguments[0] = Convert.ToInt32(finalScore);
                    }

                    lastTimer = newTimer;
                    timer = newTimer;
                    LocalizedTime.Arguments[0] = timer;

                    timerText.text = LocalizedTime.GetLocalizedString();
                    scoreText.text = LocalizedScore.GetLocalizedString();

                    if (finalScore > highScore)
                    {
                        LocalizedHighScore.Arguments[0] = finalScore;
                        HighScoreText.text = LocalizedHighScore.GetLocalizedString();
                        PlayerPrefs.SetInt("HighScore", finalScore);
                    }

                    if (newTimer <= 0)
                    {
                        completedTweenTimer = true;
                        LeanTween.cancel(timerText.gameObject);
                        LeanTween.scale(timerText.gameObject, Vector3.zero, 0.2f)
                            .setEase(LeanTweenType.easeInBack)
                            .setOnComplete(() => timerText.gameObject.SetActive(false)).setIgnoreTimeScale(true);

                        if (completedTweenStar && completedTweenTimer)
                        {
                            ScalePanel();
                        }
                    }
                }
            }).setEase(LeanTweenType.linear).setIgnoreTimeScale(true);
        }
    }

    void ScalePanel()
    {
        LeanTween.moveY(panelRectTransform, panelRectTransform.anchoredPosition.y + 150, tweenDuration)
        .setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
        LeanTween.scale(pointImageRectTransform, new Vector3(10, 10, 1), tweenDuration).setIgnoreTimeScale(true);
        LeanTween.moveLocalX(scoreText.gameObject, scoreText.rectTransform.anchoredPosition.x + 100, tweenDuration).setIgnoreTimeScale(true);
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
        timer = (int)GameStats.GetElapsedTime();
        stars = player.Score;
        this.highScore = PlayerPrefs.GetInt("HighScore");
        finalScore = 0;

        timerText.text = timer.ToString();
        starText.text = stars.ToString();
        scoreText.text = finalScore.ToString();

        timerText.gameObject.SetActive(true);
        starText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);

        AnimateFinalScore(timer, stars, highScore);
    }
}

