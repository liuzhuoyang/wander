using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopupManager : Singleton<PopupManager>
{
    public void Init()
    {

    }
    public void CloseAll()
    {
        EventManager.TriggerEvent<PopupArgs>(EventNamePopup.EVENT_CLOSE_ALL_POPUP_UI, new PopupArgs() { });
    }

    #region
    public void OnPopupRate()
    {
        PopupArgs args = new PopupArgs() { popupName = "popup_rate" };
        OnPopup(args);
    }

    #endregion

    public void OnPopup<T>(T args) where T : PopupArgs
    {
        // 触发事件，弹出相应的UI
        EventManager.TriggerEvent<T>(EventNamePopup.EVENT_POPUP_UI, args);
    }

    #region 插屏预告
    /*
    public void OnPopupAdBreak(PopupAdBreakArgs args)
    {
        EventManager.TriggerEvent<UIPopupArgs>(EventName.EVENT_POPUP_UI, new UIPopupArgs()
        {
            popupName = "ad_break",
            callback = (target) =>
            {
                target.GetComponent<PopupAdBreak>().OnOpen(args);
            },
        });
    }*/

    #endregion

    #region 使用广告券跳过广告弹窗

    public void OnPopupAdTicket(PopupAdTicketArgs args)
    {
        args.popupName = "popup_ad_ticket";
        OnPopup(args);
    }

    #endregion
}
