/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityLevelPlayState
{
    //静态参数，用于检查是否已经初始化过，每次启动游戏只要启动一次，游戏中重启不需要再次初始化
    public static bool isIronSourceInitialized = false;
}

public class UnityLevelPlay : Singleton<UnityLevelPlay>
{
    public string appKey = "1eae4b35d";

    public void Init()
    {
        InitIronsource();
    }

    void OnDestroy()
    {
        Debug.Log("=== UnityLevelPlay: unregister ironsource events ===");
        IronSourceEvents.onSdkInitializationCompletedEvent  -= OnIronSourceInitializationCompleted;
        IronSourceRewardedVideoEvents.onAdOpenedEvent       -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent       -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent    -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent  -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent   -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent     -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent   -= RewardedVideoAdShowFailed;
        IronSourceRewardedVideoEvents.onAdReadyEvent        -= RewardedVideOnAdReadyEvent;
        //IronSourceRewardedVideoEvents.onAdClickedEvent      -= RewardedVideoOnAdClickedEvent;
    }

    public void InitIronsource()
    {
        if (UnityLevelPlayState.isIronSourceInitialized)
        {
            Debug.Log("=== UnityLevelPlay: ironsource have been initialized already ===");
            return;
        }
            
        IronSourceEvents.onSdkInitializationCompletedEvent  += OnIronSourceInitializationCompleted;
        IronSourceRewardedVideoEvents.onAdOpenedEvent       += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent       += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent    += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent  += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent   += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent     += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdLoadFailedEvent   += RewardedVideoAdShowFailed;
        IronSourceRewardedVideoEvents.onAdReadyEvent        += RewardedVideOnAdReadyEvent;
        //IronSourceRewardedVideoEvents.onAdClickedEvent      += RewardedVideoOnAdClickedEvent; //部分广告平台不支持，官方建议只有全部广告平台都打开才使用
        //IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        /*
        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialOnAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialOnAdLoadFailed;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialOnAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialOnAdClickedEvent;
        //IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialOnAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialOnAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialOnAdClosedEvent;*/

        /*
        IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;
        IronSourceBannerEvents.onAdClickedEvent += BannerOnAdClickedEvent;
        //IronSourceBannerEvents.onAdScreenPresentedEvent += BannerOnAdScreenPresentedEvent;
        //IronSourceBannerEvents.onAdScreenDismissedEvent += BannerOnAdScreenDismissedEvent;
        IronSourceBannerEvents.onAdLeftApplicationEvent += BannerOnAdLeftApplicationEvent;
        

        //开启Level Play广告测试
        if (Game.Instance.isDebugIronsource)
        {
            IronSource.Agent.setMetaData("is_test_suite", "enable");
        }

        //初始化
        IronSource.Agent.init(appKey);

        //验证广告
        IronSource.Agent.validateIntegration();

        Debug.Log("=== UnityLevelPlay: init ironsource, appKey = " + appKey + " ===");
    }

    private void OnIronSourceInitializationCompleted()
    {
        UnityLevelPlayState.isIronSourceInitialized = true;
        Debug.Log("=== UnityLevelPlay： IronSource SDK initialized. ===");
    }


    //获取广告
    public void OnLoadVideoAD()
    {
        IronSource.Agent.loadRewardedVideo();
    }

    
    public void OnLoadIntersitialAD()
    {
        IronSource.Agent.loadInterstitial();
    }

    
    public void OnLoadBannerAD()
    {
        IronSource.Agent.loadBanner();
    }

    //播放广告
    public void OnPlayVideoAD()
    {
        IronSource.Agent.showRewardedVideo();
    }

    //初始化
    public void OnPlayIntersitialAD()
    {
        IronSource.Agent.showInterstitial();
    }

    //视频激励广告
    // 成功读取视频激励广告
    // Indicates that there’s an available ad.
    // The adInfo object includes information about the ad that was loaded successfully
    // This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        Debug.Log("=== UnityLevelPlay: ad loaded ===");
    }

    // 视频广告没有准备好
    // Indicates that no ads are available to be displayed
    // This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        AdsManager.Instance.OnAdNotReady();
        //AdsManager.Instance.OnAdFailed();
        //TipManager.Instance.OnTip("tip/ad_not_ready");
        //AdsManager.Instance.OnAdNotReady();
    }

    //开启视频广告，切到后台
    // The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.OnAdOpen(adInfo.adNetwork, adInfo.adUnit);
    }

    //视频广告被关闭，切回前台
    // The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.OnAdClose();
    }

    //完成了广告，给予奖励
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        //LogManager.Instance.OnLogAdCompleted(adInfo.)
        //AnalyticsControl.Instance.OnLogAdCompleted(AdType.VideoDailyEnergy, adInfo.adNetwork);
        AdsManager.Instance.OnAdCompleted(false, adInfo.adNetwork, (float)adInfo.revenue, adInfo.adUnit);
    }

    //调用了视频广告，但不能播放广告
    // Called when loading the ad fails (e.g., network error)
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        AdsManager.Instance.OnAdFailed();
    }

    //广告播放失败
    //Called when attempting to show the ad fails (e.g., no ad available)
    void RewardedVideoAdShowFailed(IronSourceError error)
    {

    }

    void RewardedVideOnAdReadyEvent(IronSourceAdInfo adInfo)
    {

    }

    /*
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
    {

    }

    /*
    // Invoked when the video ad was clicked.
    // This callback is not supported by all networks, and we recommend using it only if
    // it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
    }*/

    /************* 插屏广告 *************/

    /*
    // 成功读取插屏广告
    void InterstitialOnAdReadyEvent(IronSourceAdInfo adInfo)
    {

    }

    // 读取插屏广告失败
    void InterstitialOnAdLoadFailed(IronSourceError ironSourceError)
    {

    }

    // 插屏广告开启，算一个Impression
    void InterstitialOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {

    }

    // 用户点击了插屏广告内容
    void InterstitialOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
    }

    // 插屏广告播放失败
    void InterstitialOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
    {

    }

    // 关闭了插屏广告，用户回到游戏
    void InterstitialOnAdClosedEvent(IronSourceAdInfo adInfo)
    {

    }*/

    /*
    // Invoked before the interstitial ad was opened, and before the InterstitialOnAdOpenedEvent is reported.
    // This callback is not supported by all networks, and we recommend using it only if  
    // it's supported by all networks you included in your build. 
    void InterstitialOnAdShowSucceededEvent(IronSourceAdInfo adInfo)
    {
    }*/

    /*
    /************* 横幅广告 *************/
    //文档 https://developers.is.com/ironsource-mobile/unity/banner-integration-unity/#step-1

    /*
    // 成功读取横幅广告
    void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
    {

    }

    // 读取横幅广告失败
    void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
    {

    }

    // 用户点击了横幅广告
    void BannerOnAdClickedEvent(IronSourceAdInfo adInfo)
    {
        
    }

    /*
    // Notifies the presentation of a full screen content following user click
    void BannerOnAdScreenPresentedEvent(IronSourceAdInfo adInfo)
    {

    }

    //Notifies the presented screen has been dismissed
    void BannerOnAdScreenDismissedEvent(IronSourceAdInfo adInfo)
    {

    }

    //用户离开游戏
    void BannerOnAdLeftApplicationEvent(IronSourceAdInfo adInfo)
    {

    }
}

*/