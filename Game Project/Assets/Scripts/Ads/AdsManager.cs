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

    private async void Awake()
    {
        await InitializeAdsAsync();
    }

    private async Task InitializeAdsAsync()
    {
        Unity.Services.LevelPlay.LevelPlay.ValidateIntegration();

        Unity.Services.LevelPlay.LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;

        Unity.Services.LevelPlay.LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        Unity.Services.LevelPlay.LevelPlay.SetMetaData("is_test_suite", "enable");
        Unity.Services.LevelPlay.LevelPlay.SetConsent(true);
        Unity.Services.LevelPlay.LevelPlay.Init(AppKey);
    }

    private async Task EnableAdsAsync()
    {
        Debug.Log("[LevelPlaySample] Enabling Ads");

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
            //Debug.Log("[LevelPlaySample] Show Interstitial Ad");
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
            //Debug.Log("[LevelPlaySample] Show Banner Ad");
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
            //Debug.Log("[LevelPlaySample] Show Rewarded Ad");
            rewardedVideoAd.ShowAd();
        }
        else
        {
            //Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Rewarded Ad.");
        }
    }

    #endregion

    #region Initialization Events

    private async void SdkInitializationCompletedEvent(Unity.Services.LevelPlay.LevelPlayConfiguration config)
    {
        //Debug.Log($"[LevelPlaySample] Received SdkInitializationCompletedEvent with Config: {config}");
        //EnableAds();
        await EnableAdsAsync();
    }

    void SdkInitializationFailedEvent(Unity.Services.LevelPlay.LevelPlayInitError error)
    {
        //Debug.Log($"[LevelPlaySample] Received SdkInitializationFailedEvent with Error: {error}");
    }

    #endregion

    #region Rewarded Events
    void RewardedVideoOnLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnLoadedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdLoadFailedEvent With Error: {error}");
    }

    void RewardedVideoOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedEvent With AdInfo: {adInfo}");
    }
#pragma warning disable 0618
    void RewardedVideoOnAdDisplayedFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError error)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedFailedEvent With Error: {error}");
    }
#pragma warning restore 0618
    void RewardedVideoOnAdRewardedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo, Unity.Services.LevelPlay.LevelPlayReward reward)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdRewardedEvent With AdInfo: {adInfo} and Reward: {reward}");
        rewardedVideoAd.LoadAd();
        PlayerReviveSystem.instance.RevivePlayer();
    }

    void RewardedVideoOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdClosedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdClosedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdInfoChangedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdInfoChangedEvent With AdInfo {adInfo}");
    }

    #endregion

    #region Interstitial Events
    void InterstitialOnAdLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdLoadedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdLoadFailedEvent With Error: {error}");
    }

    void InterstitialOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdDisplayedEvent With AdInfo: {adInfo}");
        //interstitialAd.LoadAd();
    }
#pragma warning disable 0618
    void InterstitialOnAdDisplayFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError infoError)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdDisplayFailedEvent With InfoError: {infoError}");
    }
#pragma warning restore 0618
    void InterstitialOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdClosedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdClosedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdInfoChangedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received InterstitialOnAdInfoChangedEvent With AdInfo: {adInfo}");
    }

    #endregion

    #region Banner Events
    void BannerOnAdLoadedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdLoadedEvent With AdInfo: {adInfo}");
        isAdsEnabled = true;
        ShowBannerAd();
    }

    void BannerOnAdLoadFailedEvent(Unity.Services.LevelPlay.LevelPlayAdError error)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdLoadFailedEvent With Error: {error}");
        isAdsEnabled = false;
    }

    void BannerOnAdClickedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdDisplayedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdDisplayedEvent With AdInfo: {adInfo}");
    }
#pragma warning disable 0618
    void BannerOnAdDisplayFailedEvent(Unity.Services.LevelPlay.LevelPlayAdDisplayInfoError adInfoError)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdDisplayFailedEvent With AdInfoError: {adInfoError}");
        isAdsEnabled = false;
    }
#pragma warning restore 0618
    void BannerOnAdCollapsedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        isAdsEnabled = false;
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdCollapsedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdLeftApplicationEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        isAdsEnabled = false;
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdLeftApplicationEvent With AdInfo: {adInfo}");

    }

    void BannerOnAdExpandedEvent(Unity.Services.LevelPlay.LevelPlayAdInfo adInfo)
    {
        //Debug.Log($"[LevelPlaySample] Received BannerOnAdExpandedEvent With AdInfo: {adInfo}");
    }

    #endregion

    #region Impression Data Event
    void ImpressionDataReadyEvent(LevelPlayImpressionData impressionData)
    {
        Debug.Log($"[LevelPlaySample] Received ImpressionDataReadyEvent ToString(): {impressionData}");
        Debug.Log($"[LevelPlaySample] Received ImpressionDataReadyEvent allData: {impressionData.AllData}");
    }

    #endregion

    private void OnDisable()
    {
        bannerAd?.DestroyAd();
        interstitialAd?.DestroyAd();
        rewardedVideoAd?.DestroyAd();
    }

    public void DestroyBanner()
    {
        bannerAd.HideAd();
        isAdsEnabled = false;
    }

    public void ShowBanner()
    {
        bannerAd.ShowAd();
        isAdsEnabled = true;
    }

    public void CreateBanner()
    {
        bannerAd = new Unity.Services.LevelPlay.LevelPlayBannerAd(BannerAdUnitId);
        bannerAd.LoadAd();
    }
}
