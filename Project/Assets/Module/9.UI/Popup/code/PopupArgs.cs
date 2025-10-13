using System;
//using UnityEngine;

public class EventNamePopup
{
    public const string EVENT_POPUP_UI = "OnPopupUI";
    public const string EVENT_CLOSE_ALL_POPUP_UI = "OnCloseAllPopupUI";
}

public class PopupArgs : EventArgs
{
    public string popupName;
    //public Action<GameObject> callback; //返回Popup对象到调用的地方，进行更新
    public Action callback;//任务序列执行完成后的回调
}