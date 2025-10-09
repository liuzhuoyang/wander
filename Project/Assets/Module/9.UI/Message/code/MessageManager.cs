using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageManager : Singleton<MessageManager>
{
    public void Init()
    {
    }

    public void OnLoading()
    {
        Debug.Log("=== MessageManager: on loading ===");
        EventManager.TriggerEvent<MsgArgs>(EventNameMsg.EVENT_MESSAGE_UI, new MsgArgs { target = "msg_loading" });
    }

    public void CloseLoading()
    {
        EventManager.TriggerEvent<MsgArgs>(EventNameMsg.EVENT_MESSAGE_CLOSE_LOADING_UI, null);
    }

    public void OnGenericMessage(string contentKey)
    {
        MsgArgs args = new MsgArgs() { target = "msg_generic", content = GetContentText(contentKey) };
        MessageControl.OnShowMessage<MsgArgs>(args);
    }

    public void OnConfrim(string contentKey, Action onConfirm)
    {
        MsgArgs args = new MsgArgs() { target = "msg_confirm", content = GetContentText(contentKey), onConfrim = onConfirm };
        MessageControl.OnShowMessage<MsgArgs>(args);
    }

    public void OnTimeout()
    {
        MsgArgs args = new MsgArgs() { target = "msg_timeout" };
        MessageControl.OnShowMessage<MsgArgs>(args);
    }

    string GetContentText(string contentKey)
    {
        string content = "";
        // 如果需要本地化内容，使用contentKey获取对应的本地化文本
        if (!string.IsNullOrEmpty(contentKey))
        {
            content = UtilityLocalization.GetLocalization(contentKey);
        }
        return content;
    }

    public void OnIAPSucceed()
    {
        //CloseLoading();
    }

    public void OnIAPFailed()
    {
        CloseLoading();
        OnGenericMessage("msg/iap_failure");
    }

    /*
        EventManager.TriggerEvent<MsgUIUpdateArgs>(EventNameMsg.EVENT_MESSAGE_UI, new MsgUIUpdateArgs
        {
            target = "msg_update",
            currentVersion = currentVersion,
            targetVersion = targetVersion
        });*/

    /*
    #region 账户登录的弹窗信息反馈
    //成功连接账户
    public void OnAccountLinked()
    {
        MessageManager.Instance.CloseLoading();

        GameSaver.Instance.isStopSaving = true; //停止存档的执行
        Debug.Log("=== Account: linked ===");
        string content = GameData.AllLocalization["ui/account/linked"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
            callback = ()=>
            {
                Game.Instance.Restart();
            }
        });
    }

    //连接账户失败
    public void OnAccountLinkFailed()
    {
        Debug.Log("=== Account: failed to link ===");
        MessageManager.Instance.CloseLoading();

        string content = GameData.AllLocalization["ui/account/link_fail"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
        });
    }

    //上传进度
    public void OnAccountUploadDataSucceed()
    {
        Debug.Log("=== Account: data uploaded succeed ===");
        MessageManager.Instance.CloseLoading();

        string content = GameData.AllLocalization["ui/account/upload_data"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
        });
    }

    public void OnAccountUploadDataFailed()
    {
        Debug.Log("=== Account: data failed to upload ===");
        MessageManager.Instance.CloseLoading();
        
        string content = GameData.AllLocalization["ui/account/upload_data_fail"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
        });
    }
    #endregion
    */

    /*
    public void OnRestoreSucceed()
    {
        string content = GameData.AllLocalization["msg/restore_succeed"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
        });
    }

    public void OnRestoreIAPFailed()
    {
        string content = GameData.AllLocalization["msg/restore_failed"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs
        {
            msgName = "msg_generic",
            text = content,
        });
    }
    
     */

    /*
    #region 弹窗
    //购买弹窗
    public void PurchaseCurrency(string purchaseItemUsercase, string purchaseItemTarget, string[] itemArgs, int gemCost, Action action)
    {
        //int totalGemCost = Inventory.Instance.GetItemTotalGemValue(itemArgs);
        EventManagerCustom.TriggerEvent<MsgPurchaseItemUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgPurchaseItemUIArgs
        {
            msgName = "msg_purchase_item",
            callback = action,
            usercase = purchaseItemUsercase,
            traget = purchaseItemTarget,
            itemArgs = itemArgs,
            isPurchaseMissingItems = false,
            gemCost = gemCost
        });
    }*/


    /*
    /// <summary>
    /// 购买缺少物品弹窗
    /// </summary>
    /// <param name="purchaseItemUsercase"></param>
    /// <param name="purchaseItemTarget"></param>
    /// <param name="itemArgs"></param>
    /// <param name="action"></param>
    public void PurchaseItems(string purchaseItemUsercase, string purchaseItemTarget, string[] itemArgs, Action action)
    {
        EventManagerCustom.TriggerEvent<MsgPurchaseItemUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgPurchaseItemUIArgs
        {
            msgName = "msg_purchase_item",
            callback = action,
            usercase = purchaseItemUsercase,
            traget = purchaseItemTarget,
            itemArgs = itemArgs,
            isPurchaseMissingItems = true,
        });
    }*/


    /*
    /// <summary>
    /// 链接超时
    /// </summary>
    public void OnTimeout()
    {
        Debug.LogWarning("timeout");
        //EventManager.TriggerEvent(EventName.EVENT_MESSAGE_UI, new EventArgs { name = "timeout" });
        EventManagerCustom.TriggerEvent<MsgUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgUIArgs { msgName = "msg_timeout" });
    }

    /*
    public void OnTipMessage(string tip)
    {
        EventManager.TriggerEvent(EventName.EVENT_TIPS, new EventArgs { stringValue = tip });
    }

    //所有timeout消息都公用
    public void OnTipMessageTimeout()
    {
        string content = GameData.AllLocalization[Constant.TIP_TIMEOUT];
        EventManager.TriggerEvent(EventName.EVENT_TIPS, new EventArgs { stringValue = content });
    }*/



    /// <summary>
    /// 广告弹窗
    /// </summary>
    /// <param name="target">奖励物件</param>
    /// <param name="rewardNum">奖励数量</param>
    /// <param name="action">回调</param>
    /// <param name="remain">剩余次数</param>
    /// <param name="limit">观看限制</param>
    /*
    public void OnADRequest(AdsType type, string target, int rewardNum, Action action, int remain = -1, int limit = -1)
    {
        EventManagerCustom.TriggerEvent<MsgADUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgADUIArgs
        {
            type = type,
            msgName = "msg_ad",
            adRewardTarget = target,
            adrewardNum = rewardNum,
            adRemain = remain,
            adLimit = limit,
            callback = action
        }) ;
    }*/

    #region Generic
    /*
    public void OnNextDay()
    {
        string contest = GameData.AllLocalization["msg/new_day"];
        EventManagerCustom.TriggerEvent<MsgGenericUIArgs>(EventName.EVENT_MESSAGE_UI, new MsgGenericUIArgs {
            msgName = "msg_generic", text = contest,
            callback = ()=> {
                Utility.isHotReboost = true;
                Game.Instance.Restart();
            } });
    }*/
    #endregion

    /*
    public void OnFatalError(string content = "")
    {
        EventManager.TriggerEvent<MsgUIFatalErrorArgs>(EventNameMsg.EVENT_MESSAGE_UI, new MsgUIFatalErrorArgs
        {
            targetName = "msg_fatal_error",
            content = content
        });
    }*/
}
