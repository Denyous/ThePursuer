using System.Threading.Tasks;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private Unity.Services.LevelPlay.LevelPlayBannerAd bannerAd;
    private Unity.Services.LevelPlay.LevelPlayInterstitialAd interstitialAd;
    private Unity.Services.LevelPlay.LevelPlayRewardedAd rewardedVideoAd;
    public bool isAdsEnabled = false;

    public static AdsManager instance;

    private const string BannerAdUnitId = "b1mimusvm1ezh4dl"; 
    private const string InterstitialAdUnitId = "7gfojay1ka2sjetm"; 
    private const string RewardedAdUnitId = "q62bk3sr6ki7vauw"; 
    private const string AppKey = "227bad085";

    public MainMenuController mainMenuController;

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
        InitializeAds();
    }

    private void InitializeAds()
    {
        Unity.Services.LevelPlay.LevelPlay.ValidateIntegration();

        Unity.Services.LevelPlay.LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        Unity.Services.LevelPlay.LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        //Unity.Services.LevelPlay.LevelPlay.SetMetaData("is_test_suite", "enable");
        //Unity.Services.LevelPlay.LevelPlay.SetConsent(true);
        Unity.Services.LevelPlay.LevelPlay.Init(AppKey);
    }

    private void EnableAds()
    {
        Unity.Services.LevelPlay.LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        rewardedVideoAd = new Unity.Services.LevelPlay.LevelPlayRewardedAd(RewardedAdUnitId);
        bannerAd = new Unity.Services.LevelPlay.LevelPlayBannerAd(BannerAdUnitId);
        interstitialAd = new Unity.Services.LevelPlay.LevelPlayInterstitialAd(InterstitialAdUnitId);

        // Register Rewarded Events
        rewardedVideoAd.OnAdLoaded += RewardedVideoOnLoadedEvent;
        rewardedVideoAd.OnAdLoadFailed += RewardedVideoOnAdLoadFailedEvent;
        rewardedVideoAd.OnAdDisplayed += RewardedVideoOnAdDisplayedEvent;
        rewardedVideoAd.OnAdDisplayFailed += RewardedVideoOnAdDisplayedFailedEvent;
        rewardedVideoAd.OnAdRewarded += RewardedVideoOnAdRewardedEvent;
        rewardedVideoAd.OnAdClicked += RewardedVideoOnAdClickedEvent;
        rewardedVideoAd.OnAdClosed += RewardedVideoOnAdClosedEvent;
        rewardedVideoAd.OnAdInfoChanged += RewardedVideoOnAdInfoChangedEvent;

        // Register Banner Events
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;

        // Register Interstitial Events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        // Load all ads
        bannerAd.LoadAd();
        interstitialAd.LoadAd();
        rewardedVideoAd.LoadAd();
    }

    #region Show Ads Functions
    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            //Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Interstitial Ad.");
        }
    }
    public void ShowBannerAd()
    {
        if (isAdsEnabled)
        {
            bannerAd.ShowAd();
            if (mainMenuController is not null)
            {
                mainMenuController.AdjustUIForBannerAds();
            }
        }
        else
        {
            //Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Interstitial Ad.");
        }
    }
    public void ShowRewardedAd()
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
    #endregion

    #region Initialization Events

    private void SdkInitializationCompletedEvent(Unity.Services.LevelPlay.LevelPlayConfiguration config)
    {
        EnableAds();
        isAdsEnabled = true;
    }
    void SdkInitializationFailedEvent(Unity.Services.LevelPlay.LevelPlayInitError error)
    {
    }

    #endregion

    #region Rewarded Events
    void RewardedVideoOnLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void RewardedVideoOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
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
    }
    void InterstitialOnAdInfoChangedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    #endregion

    #region Banner Events
    void BannerOnAdLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void BannerOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
    }
    void BannerOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void BannerOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
#pragma warning disable 0618
    void BannerOnAdDisplayFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError adInfoError)
    {
    }
#pragma warning restore 0618
    void BannerOnAdCollapsedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void BannerOnAdLeftApplicationEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
    }
    void BannerOnAdExpandedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
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
        bannerAd?.DestroyAd();
        interstitialAd?.DestroyAd();
        rewardedVideoAd?.DestroyAd();
    }

    public void HideBanner()
    {
        bannerAd.HideAd();
        Debug.Log("Hide Banner Ad");
    }

    public void ShowBanner()
    {
        bannerAd.ShowAd();
        Debug.Log("Show Banner Ad");
    }
}
