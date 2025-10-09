#if UNITY_ANDROID && !TGAME_AMAZON
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TGame;

public class AdControlAndroid : AdControl
{
    public override void LoadAD()
    {
        /* TGame
        Debug.Log("=== AndroidAdsManager LoadAD ===");
        TGOPSItem item = TGOPSMgr.Instance.GetOPSItem("ad_load");
        if (item != null)
        {
            item.ActionShow();
            item.ActionClick();
        }*/
    }

    public override void OnVideoAd(AdData adData, Action callbackSucceed, Action callbackInterrupted)
    {
        ResetRewaredAd();
        this.callbackRewaredSucceed = callbackSucceed;
        this.callbackRewaredInterrupted = callbackInterrupted;
        this.selectedAdType = adData.adType;
       
        
        /*
        Debug.Log("=== AndroidAdsManager OnVideoAd:" + adData.adType + "&&" + adData.tgNode + " ===");
        //TGame播放广告
        TGameSDK.AdVideoShow(adData.tgNode.ToString(), (bool isSuccess) =>
        {
            if (isSuccess)
            {
                //TODO 需要传入广告商名称，本次广告收入
                AddAdCount(adData.adType);
                OnAdCompleted(false, "android", 0);
            }
            else
            {
                OnAdFailed();
            }
        });*/
    }

    public override void OnAdInterstitial(AdData adData, Action callbackDone, Action callbackFailed)
    {
        callbackInterstitialDone = callbackDone;
        callbackInterstitialFailed = callbackFailed;
        /*
        Debug.Log("=== AndroidAdsManager OnInterstitialAd:" + adData.adType + "&&" + adData.tgNode + " ===");
        TGameSDK.AdNgsShow(adData.tgNode.ToString(), (bool success) =>
        {
            if (success)
            {
                //播放插屏广告成功
                OnAdInterstitialCompleted();
            }
            else
            {
                //播放插屏广告失败
                // TipManager.Instance.OnTip(GameData.AllLocalization["tip/ad_fail"]);
                OnAdInterstitialFailed();
            }
        });*/
    }

    public override bool OnCheckIsInterestitalReady()
    {
        return true;
    }
}
#endif