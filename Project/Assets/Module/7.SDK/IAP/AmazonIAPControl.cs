#if UNITY_ANDROID && TGAME_AMAZON
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGame;
using System;
public class AmazonIAPControl : IAPControl
{
    public override void Init()
    {

    }

    public override void OnPurchaseConsumable(string productID, Action callbackSucceed, Action callbackFailed)
    {
        float priceUSD = GameData.allIap.dictIapData[productID].priceUSD;

        TGameSDK.IapBuy(priceUSD.ToString(), TGCOPSStyle.inApp.ToString(), (TGResultIapType type) =>
        {
            if (type == TGResultIapType.completed)
            {
                GameData.userData.userTag.isPayUser = true;
                GameData.userData.userAnalytics.iapCount++;
                GameData.userData.userAnalytics.iapValue += priceUSD;

                callbackSucceed?.Invoke();
                
                EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
                {
                    action = ActionType.IAPComplete,
                    target = productID
                });

                //TDG打点
                // AnalyticsControl.Instance.TGD_storeSuccess();
            }
            else
            {
                callbackFailed?.Invoke();
            }
        });

        //TDG打点
        int priceInt = Mathf.RoundToInt(priceUSD * 100f);
        string storeID = productID + "_" + priceInt.ToString("D4");
        // AnalyticsControl.Instance.TGD_onStoreClick(storeID);
    }

    public void OnPurchaseUnConsume(string productID, Action callbackSucceed, Action callbackFailed)
    {
        float priceUSD = GameData.allIap.dictIapData[productID].priceUSD;

        TGameSDK.IapBuy(priceUSD.ToString(), TGCOPSStyle.inAppUnConsume.ToString(), (TGResultIapType type) =>
        {
            if (type == TGResultIapType.completed)
            {
                GameData.userData.userTag.isPayUser = true;
                GameData.userData.userAnalytics.iapCount++;
                GameData.userData.userAnalytics.iapValue += priceUSD;

                callbackSucceed?.Invoke();
                // GameSaver.Instance.OnSave();

                //TDG打点
                // AnalyticsControl.Instance.TGD_storeSuccess();
            }
            else
            {
                callbackFailed?.Invoke();
            }
        });

        //TDG打点
        int priceInt = Mathf.RoundToInt(priceUSD * 100f);
        string storeID = productID + "_" + priceInt.ToString("D4");
        // AnalyticsControl.Instance.TGD_onStoreClick(storeID);
    }

    public bool UserHasRemoveAds()
    {
        return TGameSDK.UserHasRemoveAds();
    }

    public override string GetLocalPriceString(string productID)
    {
        float priceUSD = GameData.allIap.dictIapData[productID].priceUSD;
        TGSeri_IAPPrice iapPrice = TGameSDK.IapGetPrice(priceUSD.ToString(), TGCOPSStyle.inApp.ToString(), 0);

        return iapPrice.price;
    }

    public override void OnRestorePurchase()
    {
        TGameSDK.IapRestore((bool callbackSucceed) =>
        {
            // TipManager.Instance.OnTip(GameData.AllLocalization["setting/setting_restore_purchase_success"]);
        });
    }
}
#endif