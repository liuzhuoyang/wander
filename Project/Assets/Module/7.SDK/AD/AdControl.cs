using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class AdControl : Singleton<AdControl>
{
    protected Action callbackRewaredSucceed;
    protected Action callbackRewaredInterrupted;
    protected Action callbackInterstitialDone;
    protected Action callbackInterstitialFailed;
    protected AdType selectedAdType;

    // public Dictionary<AdType, int> adLimitDict;

    public void Init()
    {
        //TGAME广告需求
        LoadAD();
    }

    //如果有广告票则弹窗
    public void OnVideoAdSkippable(AdData adData, Action callbackSucceed, Action callbackInterrupted)
    {
        bool hasTicket = ItemSystem.Instance.GetItemNum(ConstantItem.TOKEN_TICKET_AD) > 0;
        callbackRewaredSucceed = callbackSucceed;
        callbackRewaredInterrupted = callbackInterrupted;

        if (hasTicket)
        {
            PopupManager.Instance.OnPopupAdTicket(new PopupAdTicketArgs
            {
                onTicket = () =>
                {
                    ItemSystem.Instance.UseItem(ConstantItem.TOKEN_TICKET_AD, 1, () =>
                    {
                        callbackSucceed?.Invoke();
                        AddAdCount(adData.adType);
                        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.UseItem });
                    });
                },
                onAd = () =>
                {
                    OnVideoAd(adData, callbackSucceed, callbackInterrupted);
                },
                onClose = () =>
                {
                    callbackInterrupted?.Invoke();
                }
            });
        }
        else
        {
            OnVideoAd(adData, callbackSucceed, callbackInterrupted);
        }
    }

    public virtual void OnVideoAd(AdData adData, Action callbackSucceed, Action callbackInterrupted) { }

    //SDK内部调用
    public void OnAdCompleted(bool isSkip = false, string provider = "debug", float revenue = 0)
    {

        AnalyticsControl.Instance.OnLogAdCompleted(selectedAdType, provider, revenue);

        callbackRewaredSucceed?.Invoke();

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.CompleteAD, target = selectedAdType.ToString() });
        ResetRewaredAd();

        Debug.Log("=== OnAdCompleted:" + provider + "__" + selectedAdType + " ===");
    }

    //SDK内部调用
    //播放广告失败
    protected void OnAdFailed()
    {

        AnalyticsControl.Instance.OnLogAdFailed(selectedAdType);
        callbackRewaredInterrupted?.Invoke();
        //点击播放广告失败才会调用，出提示
        TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/ad_fail"));

        Debug.Log("=== OnAdFailed:" + selectedAdType + " ===");
    }


    #region 插屏广告
    //请求插屏广告
    public virtual void OnAdInterstitial(AdData adData, Action callbackDone, Action callbackFailed)
    {
        AnalyticsControl.Instance.OnLogAdInterstitialShow(); //播放插屏广告打点
        callbackInterstitialDone = callbackDone;
        callbackInterstitialFailed = callbackFailed;
        //Debug.Log("=== AdsManager OnInterstitialAd:" + adData.adType + "&&" + adData.tgNode + " ===");
    }

    //SDK里面自行调用
    //插屏广告播放完成，回到游戏
    protected void OnAdInterstitialCompleted()
    {
        AnalyticsControl.Instance.OnLogAdInterstitialCompleted(); //完成插屏广告
        callbackInterstitialDone?.Invoke();
        callbackInterstitialDone = null;
    }

    protected void OnAdInterstitialFailed()
    {
        callbackInterstitialFailed?.Invoke();
        callbackInterstitialFailed = null;
    }

    //检查插屏广告是否可用
    public abstract bool OnCheckIsInterestitalReady();
    #endregion

    //注意不要随意调用，可能会提前设置回调为null导致无法领取奖励
    protected void ResetRewaredAd()
    {
        callbackRewaredSucceed = null;
        callbackRewaredInterrupted = null;
        selectedAdType = AdType.None;
    }

    #region Banner

    public virtual void OnBannerAd(AdData adData, Action callback) { }

    public virtual void OnBannerAdHide() { }

    #endregion

    #region helper
    //ios需要加载广告换存
    public virtual void LoadAD() { }
    #endregion

    #region 记录广告次数
    public void AddAdCount(AdType adType)
    {
        if (!AllAd.dictData.ContainsKey(adType))
        {
            return;
        }
        var adData = AllAd.dictData[adType];
        if (!adData.isDailyReset)
        {
            return;
        }
        if (GameData.userData.userAd.dictAdCount.ContainsKey(adType))
        {
            GameData.userData.userAd.dictAdCount[adType]++;
        }
        else
        {
            GameData.userData.userAd.dictAdCount[adType] = 1;
        }
    }

    public int GetRemainAdCount(AdType adType)
    {
        if (!AllAd.dictData.ContainsKey(adType))
        {
            return 0;
        }
        var adData = AllAd.dictData[adType];
        if (!adData.isDailyReset)
        {
            return 0;
        }
        if (GameData.userData.userAd.dictAdCount.ContainsKey(adType))
        {
            return adData.dailyLimit - GameData.userData.userAd.dictAdCount[adType];
        }
        else
        {
            return adData.dailyLimit;
        }
    }
    #endregion

}
