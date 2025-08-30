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

    Player player;

    public LocalizedString LocalizedScore;
    public LocalizedString LocalizedTime;
    public LocalizedString LocalizedStar;

    [Header("--Variables--")]
    
    int stars;
    int timer;
    int finalScore;

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

    public void AnimateFinalScore(int startTimer, int startStars)
    {
        LeanTween.cancelAll();
        WaitForSec(1f);
        timer = startTimer;
        stars = startStars;
        int lastTimer = timer;

        finalScore = 0;
        LocalizedScore.Arguments = new object[] { finalScore };
        LocalizedStar.Arguments = new object[] { stars };
        LocalizedTime.Arguments = new object[] { timer };
        scoreText.text = LocalizedScore.GetLocalizedString();
        timerText.text = LocalizedTime.GetLocalizedString();
        starText.text = LocalizedStar.GetLocalizedString();

        Debug.Log($"Timer: {timer} \n Stars: {stars}");

        duration = (float)(stars / timer);

        if (duration < 2)
        {
            duration = 2;
        }
        Debug.Log($"Duration: {duration}");

        int initialStars = stars;
        int initialTimer = timer;

        if (initialStars > 0)
        {
            LeanTween.cancel(starText.rectTransform);
            LeanTween.value(gameObject, initialStars, 0, duration).setOnUpdate((float val) =>
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

                    if (stars <= 0)
                    {
                        completedTweenStar = true;
                        LeanTween.cancel(starText.rectTransform);
                        LeanTween.scale(starText.rectTransform, Vector3.zero, 0.2f)
                            .setEase(LeanTweenType.easeInBack)
                            .setOnComplete(() => starText.gameObject.SetActive(false));
                    }
                }
            }).setEase(LeanTweenType.linear);
        }
        else
        {
            completedTweenStar = true;
            LeanTween.cancel(starText.rectTransform);
            LeanTween.scale(starText.rectTransform, Vector3.zero, 0.2f)
                .setEase(LeanTweenType.easeInBack)
                .setOnComplete(() => starText.gameObject.SetActive(false));
        }

        if (initialTimer > 0)
        {
            LeanTween.cancel(timerText.rectTransform);
            LeanTween.value(gameObject, initialTimer, 0, duration).setOnUpdate((float val) =>
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

                    if (newTimer <= 0)
                    {
                        completedTweenTimer = true;
                        LeanTween.cancel(timerText.rectTransform);
                        LeanTween.scale(timerText.rectTransform, Vector3.zero, 0.2f)
                            .setEase(LeanTweenType.easeInBack)
                            .setOnComplete(() => timerText.gameObject.SetActive(false));

                        if (completedTweenStar && completedTweenTimer)
                        {
                            ScalePanel();
                        }
                    }
                }
            }).setEase(LeanTweenType.linear);
        }
    }

    void ScalePanel()
    {
        LeanTween.cancel(panelRectTransform.gameObject);
        LeanTween.cancel(pointImageRectTransform.gameObject);
        LeanTween.cancel(scoreText.gameObject);
        LeanTween.moveY(panelRectTransform, panelRectTransform.anchoredPosition.y + 150, tweenDuration)
        .setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(pointImageRectTransform, new Vector3(10, 10, 1), tweenDuration);
        LeanTween.moveLocalX(scoreText.gameObject, scoreText.rectTransform.anchoredPosition.x + 50, tweenDuration);
    }

    public void UnScalePanel()
    {
        LeanTween.cancel(panelRectTransform.gameObject);
        LeanTween.cancel(pointImageRectTransform.gameObject);
        LeanTween.cancel(scoreText.gameObject);
        LeanTween.moveY(panelRectTransform, panelRectTransform.anchoredPosition.y - 150, tweenDuration)
        .setEase(LeanTweenType.easeOutBack);
        LeanTween.scale(pointImageRectTransform, new Vector3(0,0,1), tweenDuration);
        LeanTween.moveLocalX(scoreText.gameObject, scoreText.rectTransform.anchoredPosition.x - 50, tweenDuration);
    }

    public void SetTextBoxesActive()
    {
        timerText.gameObject.gameObject.SetActive(true);
        starText.gameObject.gameObject.SetActive(true);
        UnScalePanel();
    }

    public void ShowFinalScore()
    {
        timer = (int)GameStats.GetElapsedTime();
        stars = player.Score;
        finalScore = 0;

        timerText.text = timer.ToString();
        starText.text = stars.ToString();
        scoreText.text = finalScore.ToString();

        AnimateFinalScore(timer, stars);
    }
}

