using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.LevelPlay;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private Unity.Services.LevelPlay.LevelPlayBannerAd bannerAd;
    private Unity.Services.LevelPlay.LevelPlayInterstitialAd interstitialAd;
    private Unity.Services.LevelPlay.LevelPlayRewardedAd rewardedVideoAd;
    bool isAdsEnabled = false;

    public static AdsManager instance;

    private const string BannerAdUnitId = "b1mimusvm1ezh4dl"; 
    private const string InterstitialAdUnitId = "7gfojay1ka2sjetm"; 
    private const string RewardedAdUnitId = "q62bk3sr6ki7vauw"; 
    private const string AppKey = "227bad085";

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
        Debug.Log("[LevelPlaySample] LevelPlay.ValidateIntegration");
        LevelPlay.ValidateIntegration();

        Debug.Log($"[LevelPlaySample] Unity version {LevelPlay.UnityVersion}");
        Debug.Log("[LevelPlaySample] Register initialization callbacks");

        var initTask = new TaskCompletionSource<bool>();

        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;

        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        Debug.Log("[LevelPlaySample] LevelPlay SDK initialization");
        LevelPlay.SetMetaData("is_test_suite", "enable");
        LevelPlay.SetConsent(true);
        LevelPlay.Init(AppKey);

        // Wait for initialization to complete
        bool success = await initTask.Task;

        if (success)
        {
            await EnableAdsAsync();
        }
        else
        {
            Debug.LogError("Ad initialization failed, not enabling ads.");
        }
    }

    private async Task EnableAdsAsync()
    {
        Debug.Log("[LevelPlaySample] Enabling Ads");

        LevelPlay.OnImpressionDataReady += ImpressionDataReadyEvent;

        rewardedVideoAd = new LevelPlayRewardedAd(RewardedAdUnitId);
        bannerAd = new LevelPlayBannerAd(BannerAdUnitId);
        interstitialAd = new LevelPlayInterstitialAd(InterstitialAdUnitId);

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

        await Task.CompletedTask; // Keep it async-compatible
    }

    #region Show Ads Functions

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsAdReady())
        {
            Debug.Log("[LevelPlaySample] Show Interstitial Ad");
            interstitialAd.ShowAd();
        }
        else
        {
            Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Interstitial Ad.");
        }
    }
    public void ShowBannerAd()
    {
        if (isAdsEnabled)
        {
            Debug.Log("[LevelPlaySample] Show Banner Ad");
            bannerAd.ShowAd();
        }
        else
        {
            Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Interstitial Ad.");
        }
    }
    public void ShowRewardedAd()
    {
        if (rewardedVideoAd.IsAdReady())
        {
            Debug.Log("[LevelPlaySample] Show Rewarded Ad");
            rewardedVideoAd.ShowAd();
        }
        else
        {
            Debug.LogWarning("[LevelPlaySample] Ads are not enabled yet. Cannot show Rewarded Ad.");
        }
    }

    #endregion

    #region Initialization Events

    private async void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        Debug.Log($"[LevelPlaySample] Received SdkInitializationCompletedEvent with Config: {config}");
        //EnableAds();
        await EnableAdsAsync();
        isAdsEnabled = true;
        ShowBannerAd();
    }

    void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.Log($"[LevelPlaySample] Received SdkInitializationFailedEvent with Error: {error}");
    }

    #endregion

    #region Rewarded Events
    void RewardedVideoOnLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnLoadedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdLoadFailedEvent With Error: {error}");
    }

    void RewardedVideoOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedEvent With AdInfo: {adInfo}");
    }
#pragma warning disable 0618
    void RewardedVideoOnAdDisplayedFailedEvent(LevelPlayAdDisplayInfoError error)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdDisplayedFailedEvent With Error: {error}");
    }
#pragma warning restore 0618
    void RewardedVideoOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward reward)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdRewardedEvent With AdInfo: {adInfo} and Reward: {reward}");
        rewardedVideoAd.LoadAd();
        PlayerReviveSystem.instance.RevivePlayer();
    }

    void RewardedVideoOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdClosedEvent With AdInfo: {adInfo}");
    }

    void RewardedVideoOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received RewardedVideoOnAdInfoChangedEvent With AdInfo {adInfo}");
    }

    #endregion

    #region Interstitial Events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdLoadedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdLoadFailedEvent With Error: {error}");
    }

    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdDisplayedEvent With AdInfo: {adInfo}");
        //interstitialAd.LoadAd();
    }
#pragma warning disable 0618
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError infoError)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdDisplayFailedEvent With InfoError: {infoError}");
    }
#pragma warning restore 0618
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdClosedEvent With AdInfo: {adInfo}");
    }

    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received InterstitialOnAdInfoChangedEvent With AdInfo: {adInfo}");
    }

    #endregion

    #region Banner Events
    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdLoadedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdLoadFailedEvent With Error: {error}");
    }

    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdClickedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdDisplayedEvent With AdInfo: {adInfo}");
        //bannerAd.LoadAd();
    }
#pragma warning disable 0618
    void BannerOnAdDisplayFailedEvent(LevelPlayAdDisplayInfoError adInfoError)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdDisplayFailedEvent With AdInfoError: {adInfoError}");
    }
#pragma warning restore 0618
    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdCollapsedEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdLeftApplicationEvent With AdInfo: {adInfo}");
    }

    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log($"[LevelPlaySample] Received BannerOnAdExpandedEvent With AdInfo: {adInfo}");
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
}
