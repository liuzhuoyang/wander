using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
//using TGame;


public abstract class IAPControl : Singleton<IAPControl>
{
    protected Action callbackSucceed;
    protected Action callbackFailed;
    public string selectedProductID;

    public virtual void Init()
    {
        /*
        TGUCallback.Instance.AddListener(TInternalCode.EV_IAP_SUCCESS, (msg) =>
        {
            string[] strings = msg.Split('%');
            string isFinish = strings[0]; // 内购验证是否成功 "true" / "false"
            //string productId = strings[1]; // 商品id
            string sku = strings[1]; // 商品id
            string orderId = strings[2]; // 订单id

            if (isFinish == "true")
            {
                float priceUSD = GameData.allIap.dictIapData[selectedProductID].sku.priceUSD;
                Debug.Log($"=== IAP: 校验成功，商品ID：{selectedProductID}，价格：{priceUSD} ===");
                AnalyticsControl.Instance.OnLogIAPSucceed(sku, selectedProductID, priceUSD); //打点
                //callbackSucceed?.Invoke();
            }
            else
            {
                Debug.Log($"=== IAP: 校验失败，商品ID：{selectedProductID}，订单ID：{orderId} ===");
                //callbackFailed?.Invoke();
            }
        });*/
    }

    public virtual void OnDestroy()
    {
        //TGUCallback.Instance.RemoveListener(TInternalCode.EV_IAP_SUCCESS);
    }

    #region 购买
    public virtual void OnPurchaseConsumable(string sku, string productID, Action callbackSucceed, Action callbackFailed) 
    { 
        selectedProductID = productID;
    }
    #endregion

    #region 获取本地货币价格
    public abstract string GetLocalPriceString(string productID);
    #endregion

    public virtual void OnRestorePurchase() { }

    protected void Reset()
    {
        callbackSucceed = null;
        callbackFailed = null;
    }
}

class PurchaseData
{
    // INAPP_PURCHASE_DATA
    public string inAppPurchaseData;
    // INAPP_DATA_SIGNATURE
    public string inAppDataSignature;

    [System.Serializable]
    private struct PurchaseReceipt
    {
        public string Payload;
    }
    [System.Serializable]
    private struct PurchasePayload
    {
        public string json;
        public string signature;
    }

    public PurchaseData(string receipt)
    {
        try
        {
            var purchaseReceipt = JsonUtility.FromJson<PurchaseReceipt>(receipt);
            var purchasePayload = JsonUtility.FromJson<PurchasePayload>(purchaseReceipt.Payload);
            inAppPurchaseData = purchasePayload.json;
            inAppDataSignature = purchasePayload.signature;
        }
        catch
        {
            Debug.Log("=== IAP: could not parse receipt ===");
            inAppPurchaseData = "";
            inAppDataSignature = "";
        }
    }
}