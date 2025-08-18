using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private Unity.Services.LevelPlay.LevelPlayInterstitialAd interstitialAd;
    private Unity.Services.LevelPlay.LevelPlayRewardedAd rewardedVideoAd;

    public static AdsManager instance;
    private bool adsEnabled = true;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (adsEnabled)
        {
            InitializeAds();
        }
    }
    
    public void RemoveAds()
    {
        adsEnabled = false;
        DestroyAds();
    }

    private void DestroyAds()
    {
        interstitialAd?.DestroyAd();
        rewardedVideoAd?.DestroyAd();
    }

    private void InitializeAds()
    {
        Unity.Services.LevelPlay.LevelPlay.ValidateIntegration();

        Unity.Services.LevelPlay.LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        Unity.Services.LevelPlay.LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        //Unity.Services.LevelPlay.LevelPlay.SetMetaData("is_test_suite", "enable");
        //Unity.Services.LevelPlay.LevelPlay.SetConsent(true);
        Unity.Services.LevelPlay.LevelPlay.Init(AdConfig.AppKey);
    }

    private void EnableAds()
    {
        Unity.Services.LevelPlay.LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        rewardedVideoAd = new Unity.Services.LevelPlay.LevelPlayRewardedAd(AdConfig.RewardedVideoAdUnitId);
        interstitialAd = new Unity.Services.LevelPlay.LevelPlayInterstitialAd(AdConfig.InterstitalAdUnitId);

        // Register Rewarded Events
        rewardedVideoAd.OnAdLoaded += RewardedVideoOnLoadedEvent;
        rewardedVideoAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
        rewardedVideoAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
        rewardedVideoAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayedFailedEvent;
        rewardedVideoAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
        rewardedVideoAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
        rewardedVideoAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
        rewardedVideoAd.OnAdInfoChanged += RewardedVideoOnAdInfoChangedEvent;

        // Register Interstitial Events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        LoadAds();
    }

    #region Show Ads Functions
    public void ShowInterstitialAd()
    {
        if (adsEnabled)
        {
            if (interstitialAd.IsAdReady())
            {
                interstitialAd.ShowAd();
            }
            else
            {
                Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Interstitial Ad.");
            }
        }
    }
    
    public void ShowRewardedAd()
    {
        if (adsEnabled)
        {
            if (rewardedVideoAd.IsAdReady())
            {
                rewardedVideoAd.ShowAd();
            }
            else
            {
                //Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Rewarded Ad.");
            }
        }
        else
        {
            PlayerReviveSystem.instance.RevivePlayer();
        }
    }
    #endregion

    #region Initialization Events

    private void SdkInitializationCompletedEvent(Unity.Services.LevelPlay.LevelPlayConfiguration config)
    {
        EnableAds();
    }
    void SdkInitializationFailedEvent(Unity.Services.LevelPlay.LevelPlayInitError error)
    {
    }

    #endregion

    #region Rewarded Events
    void RewardedVideoOnLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        if (PlayerReviveSystem.instance is not null)
        {
            PlayerReviveSystem.instance.ShowReviveButton();
        }
    }
    void RewardedVideoOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
        if (PlayerReviveSystem.instance is not null)
        {
            PlayerReviveSystem.instance.HideReviveButton();
        }
    }
    void RewardedVideoOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
#pragma warning disable 0618
    void RewardedVideoOnAdDisplayedFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError error)
    {
    }
#pragma warning restore 0618
    void RewardedVideoOnAdRewardedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo, Unity.Services.LevelPlay.LevelPlayReward reward)
    {
        rewardedVideoAd.LoadAd();
        PlayerReviveSystem.instance.RevivePlayer();
    }
    void RewardedVideoOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdClosedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdInfoChangedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    #endregion

    #region Interstitial Events
    void InterstitialOnAdLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }

    void InterstitialOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
    }

    void InterstitialOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        interstitialAd.LoadAd();
    }
#pragma warning disable 0618
    void InterstitialOnAdDisplayFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError infoError)
    {
    }
#pragma warning restore 0618
    void InterstitialOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void InterstitialOnAdClosedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        interstitialAd.LoadAd();
    }
    void InterstitialOnAdInfoChangedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    #endregion

    #region Impression Data Event
    void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
    {
    }

    #endregion

    private void OnDisable()
    {
        interstitialAd?.DestroyAd();
        rewardedVideoAd?.DestroyAd();
    }

    public void LoadAds()
    {
        interstitialAd.LoadAd();
        rewardedVideoAd.LoadAd();
    }
}
