using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
public class ApplovinMaxControl : Singleton<ApplovinMaxControl>
{
    const string SDK_KEY = "";//"1drfmG-nKRxUZ6Jls7cgS0fWosFfAxTCTJsJZr4vG0Nq6hZypAdGBInz80cJy5VVPMpzBttRuEG-wqxk_T_Brn";
#if UNITY_IOS
    string adRewardUnitId = "";
    string adInterstitialUnitID = "";
#else
    string adRewardUnitId = "";
    string adInterstitialUnitID = "";
#endif

    int retryAttemptDouble;
    int retryAttemptInterstitial;

    public void Init()
    {
        return;

        Debug.Log("=== ApplovinMaxControl: init max, appKey = " + SDK_KEY + " ===");

        MaxSdkCallbacks.OnSdkInitializedEvent += OnSdkInitialized;

        MaxSdk.SetSdkKey(SDK_KEY);
        MaxSdk.InitializeSdk();

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterestitialAdRevenuePaidEvent;
    }

    private void OnDestroy()
    {
#if !UNITY_WEBGL
        MaxSdkCallbacks.OnSdkInitializedEvent -= OnSdkInitialized;

        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedAdRevenuePaidEvent;

        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialAdFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnInterestitialAdRevenuePaidEvent;
#endif

    }
#if !UNITY_WEBGL
    // AppLovin SDK 初始化完成，开始加载广告
    void OnSdkInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration)
    {
        // 加载广告
        LoadRewardedAd();
        LoadInterstitial();
        Debug.Log("=== ApplovinMaxControl： MAX SDK initialized. ===");
    }

    void LoadRewardedAd()
    {
        Debug.Log("=== ApplovinMaxControl： start loading rewarded ad ===");
        MaxSdk.LoadRewardedAd(adRewardUnitId);
    }

    #region 激励视频事件
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        Debug.Log("=== ApplovinMaxControl： rewarded ad loaded ===");
        // Reset retry attempt
        retryAttemptDouble = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttemptDouble++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptDouble));

        Invoke("LoadRewardedAd", (float)retryDelay);

        AdsManager.Instance.OnAdNotReady();
        Debug.Log("=== ApplovinMaxControl: failed to load ad. retry attempt: " + retryAttemptDouble + " ===");
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdsManager.Instance.OnAdOpen(adInfo.NetworkName, adUnitId);
        Debug.Log("=== ApplovinMaxControl： start displaying rewarded ad ===");
    }

    //播放失败
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        AdsManager.Instance.OnAdFailed();
        LoadRewardedAd();
        Debug.Log("=== ApplovinMaxControl： failed to display rewarded ad ===");
    }

    //点击广告
    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    //
    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    //获得奖励
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        AdsManager.Instance.OnAdCompleted(false, adInfo.NetworkName, (float)adInfo.Revenue);
        Debug.Log("=== ApplovinMaxControl： complete rewarded ad ===");
    }

    //收入统计
    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // 收入在上面播放完毕已经进行了打点，这里如果要另外独立统计时再使用
        // Ad revenue paid. Use this callback to track user revenue.
        // AnalyticsControl.Instance.OnLogAdRevenue()
        Debug.Log("=== ApplovinMaxControl： rewarded ad revenue = " + (float)adInfo.Revenue + " ===");
    }

#endregion

    #region 插屏广告事件
    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adInterstitialUnitID);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttemptInterstitial = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttemptInterstitial++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInterstitial));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        AdsManager.Instance.OnAdInterstitialCompleted();
        LoadInterstitial();
    }

    //插屏收入事件
    private void OnInterestitialAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        double revenue = adInfo.Revenue;

        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD"
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
        string networkPlacement = adInfo.NetworkPlacement; // The placement ID from the network that showed the ad

        AdsManager.Instance.OnInterestitialRevenue(networkName, (float)revenue);
    }
    #endregion

#endif
    #region 对外方法
    //播放激励广告
    public void OnPlayVideoAD()
    {
        if (MaxSdk.IsRewardedAdReady(adRewardUnitId))
        {
            MaxSdk.ShowRewardedAd(adRewardUnitId);
        }
    }

    //播放插屏广告
    public void OnPlayInterstitialAd()
    {
        if (MaxSdk.IsInterstitialReady(adInterstitialUnitID))
        {
            MaxSdk.ShowInterstitial(adInterstitialUnitID);
        }
    }

    //检查插屏广告是否可用
    public bool OnCheckIsInterestitalReady()
    {
        if (!MaxSdk.IsInterstitialReady(adInterstitialUnitID))
        {
            LoadInterstitial();
            return false;
        }
        return true;
    }

    public bool IsUserGDPR()
    {
        return MaxSdk.GetSdkConfiguration().ConsentFlowUserGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr;
    }

    public void OnShowCmpForExistingUser()
    {
        Debug.Log("=== ApplovinMaxControl： ShowCmpForExistingUser ===");
        MaxSdk.CmpService.ShowCmpForExistingUser(error =>
        {
            if (null == error)
            {

            }
        });
    }

    public void OnDebugger()
    {
        MaxSdk.ShowCreativeDebugger();
    }

#endregion
}
*/