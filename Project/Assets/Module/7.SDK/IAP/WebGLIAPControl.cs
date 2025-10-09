#if UNITY_WEBGL
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WebGLIAPControl : IAPControl
{
    public override void Init()
    {

    }

    #region 购买
    public override void OnPurchaseConsumable(string productID, Action callbackSucceed, Action callbackFailed)
    {
        // int tokenWechat = GameData.allIap.dictIapData[productID].tokenWechat;
        // WXSDKManager.Instance.Deduct(tokenWechat,productID, callbackSucceed, callbackFailed);
    }
    #endregion

    #region 获取本地货币价格
    public override string GetLocalPriceString(string productID)
    {
        return "(US $0)";
#if UNITY_EDITOR
        return "(US $0)";
#endif
        // return GameData.AllIAP[productID].tokenWechat.ToString();
    }
    #endregion
}
#endif