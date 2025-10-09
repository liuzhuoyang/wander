using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdControlDev : AdControl
{
    public override void LoadAD()
    {

    }

    public override void OnVideoAd(AdData adData, Action callbackSucceed, Action callbackInterrupted)
    {
        ResetRewaredAd();
        this.callbackRewaredSucceed = callbackSucceed;
        this.callbackRewaredInterrupted = callbackInterrupted;
        this.selectedAdType = adData.adType;
        //Debug.Log("=== DevAdsManager OnVideoAd:" + adData.adType + "&&" + adData.tgNode + " ===");

        MessageManager.Instance.OnLoading();
        StartCoroutine(TimerTick.StartRealtime(0.5f, () =>
        {
            MessageManager.Instance.CloseLoading();
            AddAdCount(adData.adType);
            OnAdCompleted(false, "android", 0);
        }));
    }

    public override void OnAdInterstitial(AdData adData, Action callbackDone, Action callbackFailed)
    {
        callbackInterstitialDone = callbackDone;
        callbackInterstitialFailed = callbackFailed;
        
        //Debug.Log("=== DevAdsManager OnInterstitialAd:" + adData.adType + "&&" + adData.tgNode + " ===");
        OnAdInterstitialCompleted();
    }

    public override bool OnCheckIsInterestitalReady()
    {
        return true;
    }

    public override void OnBannerAd(AdData adData, Action callback)
    {

    }

    public override void OnBannerAdHide()
    {

    }
}