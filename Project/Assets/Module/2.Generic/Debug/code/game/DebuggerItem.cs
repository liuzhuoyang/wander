using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerItem : MonoBehaviour
{
    public DebuggerItemGroup itemGroup;
    public void OnDebugOpenItemList()
    {
        itemGroup.Init();
    }

    public void OnDebugItemInfo()
    {
        Debugger.Instance.OnCloseDebug();
        
        PopupItemInfoArgs args = new PopupItemInfoArgs();
        args.popupName = "popup_item_info";
        args.itemName = "item_currency_coin";
        PopupManager.Instance.OnPopup(args);
    }
}
