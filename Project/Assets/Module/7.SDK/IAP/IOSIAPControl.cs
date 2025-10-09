#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGame;
using System;
public class IOSIAPControl : IAPControl
{
    public override void Init()
    {
        base.Init();
    }

     public override void OnPurchaseConsumable(string sku, string productID, Action callbackSucceed, Action callbackFailed)
    {
        float priceUSD = GameData.allIap.dictIapData[productID].sku.priceUSD;
        AnalyticsControl.Instance.OnLogIAPSelect(sku, productID, priceUSD);
        
        //更新继承的selectedProductID，后续用于校验成功后打点
        selectedProductID = productID;
        //TGame内购
        TGameSDK.IapBuy(priceUSD.ToString(), TGCOPSStyle.inApp.ToString(), (TGResultIapType type) =>
        {
            //内购成功
            if (type == TGResultIapType.completed)
            {
                GameData.userData.userTag.isPayUser = true; //成功购买后设置为付费用户
                GameData.userData.userAnalytics.iapCount++; //购买次数+1
                GameData.userData.userAnalytics.iapValue += priceUSD; //增加总付费金额

                callbackSucceed?.Invoke(); //回调成功

                AnalyticsControl.Instance.OnLogTGameIAPSucceed();
                //AnalyticsControl.Instance.OnLogIAPSucceed(productID, priceUSD); //打点
                //发送内购成功事件
                EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
                {
                    action = ActionType.IAPComplete, //内购成功动作
                    target = productID //内购商品ID
                });
            }
            else
            {
                //内购失败
                callbackFailed?.Invoke();
            }
        });
    }

    public bool UserHasRemoveAds()
    {
        return TGameSDK.UserHasRemoveAds();
    }

    public override string GetLocalPriceString(string productID)
    {
        float priceUSD = GameData.allIap.dictIapData[productID].sku.priceUSD;
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