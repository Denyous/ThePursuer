using GoogleMobileAds.Api;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;
    private bool adsEnabled = true;
    private bool _showRewardedAd = false;

    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;
    private NativeOverlayAd nativeOverlayAd;

#if UNITY_ANDROID
    //private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    //private string interestitialAdUnitId = "ca-app-pub-4834092355268168/4277082895\r\n";
    private string testInterestitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    //private string rewardedAdUnitId = "ca-app-pub-4834092355268168/1004052292\r\n";
    private string testRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    //private string nativeAdUnitId = "ca-app-pub-3940256099942544/2247696110";
#elif UNITY_IPHONE
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
    private string interestitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private string nativeAdUnitId = "ca-app-pub-3940256099942544/3986624511";
#else
    private string bannerAdUnitId = "unused";
    private string interestitialAdUnitId = "unused";
    private string rewardedAdUnitId = "unused";
    private string nativeAdUnitId = "unused";
    
#endif

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RemoveAds()
    {
        adsEnabled = false;
        DestroyAds();
    }

    private void DestroyAds()
    {
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }

    public async Task InitializeAds()
    {
        if (adsEnabled)
        {
            MobileAds.Initialize(initStatus =>
            {
                Debug.Log("Admob SDK Initialized");
                LoadInterstitialAd();
                LoadRewardedAd();
            });
        }
    }

    private void LoadRewardedAd()
    {
        // Clean up any existing rewarded ad
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        // Load a new rewarded ad
        RewardedAd.Load(testRewardedAdUnitId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load:" + error?.GetMessage());
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded ad loaded.");

            // Register ad events
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.LogError("Rewarded ad closed.");
                LoadRewardedAd();
                _showRewardedAd = true;
            };
            rewardedAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.LogError("Rewarded ad failed to show:" + error.GetMessage());
            };
        });
    }

    private void Update()
    {
        if (_showRewardedAd && adsEnabled)
        {
            PlayerReviveSystem.instance.ResumeGame();
            _showRewardedAd = false;
        }
    }

    private void LoadInterstitialAd()
    {
        // Clean up any existing interstitial ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        // Load a new interstitial ad
        InterstitialAd.Load(testInterestitialAdUnitId, new AdRequest(), (InterstitialAd ad, LoadAdError error) => {

            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load: " + error?.GetMessage());
                return;
            };

            interstitialAd = ad;
            Debug.Log("Interstitial ad loaded");

            // Register ad events
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial ad closed");
                LoadInterstitialAd();
            };

            interstitialAd.OnAdFullScreenContentFailed += (error) =>
            {
                Debug.Log("Interstitial ad failed to show: " + error.GetMessage());
            };
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready");
        }
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd() && adsEnabled)
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log($"User earned reward: {reward.Amount} {reward.Type}");
            });
        }
        else
        {
            Debug.Log("Rewarded ad not ready");
        }
    }

    private void OnDisable()
    {
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }

    public void LoadAds()
    {
        //interstitialAd.LoadAd();
        //rewardedVideoAd.LoadAd();
    }
}
