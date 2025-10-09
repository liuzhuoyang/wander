using UnityEngine;
using System;

public class ApplovinMaxManager : Singleton<ApplovinMaxManager>
{
    /*
    #if UNITY_IOS
    string adUnitId = "«iOS-ad-unit-ID»";
    #else // UNITY_ANDROID
    string adUnitId = "d72c4aaa9c9ff018";
    #endif

    int retryAttempt;

    public void Start()
    {
        MaxSdk.InitializeSdk();
        InitializeRewardedAds();
    }

    Action callbackSucceed;
    Action callbackInterrupted;
    public void OnPlayRewardedAd(Action callbackSucceed, Action callbackInterrupted)
    {
        callbackSucceed = null;
        callbackInterrupted = null;
        if (MaxSdk.IsRewardedAdReady(adUnitId))
        {
            TipManager.Instance.OnTip("Ads is ready");
            this.callbackSucceed = callbackSucceed;
            this.callbackInterrupted = callbackInterrupted;
            MaxSdk.ShowRewardedAd(adUnitId);
        }else
        {
            TipManager.Instance.OnTip("Ads is not ready");
        }
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdk.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo)
    {
    // Rewarded ad failed to load
    // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float) retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdk.AdInfo adInfo) {}

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdk.ErrorInfo errorInfo, MaxSdk.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdk.AdInfo adInfo) {}

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdk.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdk.AdInfo adInfo)
    {
        AdsManager.Instance.OnAdCompleted();
        callbackSucceed = null;
        callbackInterrupted = null;
        TipManager.Instance.OnTip("Ads is received reward");
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdk.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
    */
}
