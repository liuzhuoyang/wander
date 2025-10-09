using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//只负责管理创建和对象查找
public class UIPopup : MonoBehaviour
{
    //Dictionary<string, GameObject> dict;

    void Start()
    {
        //dict = new Dictionary<string, GameObject>();
        EventManager.StartListening<PopupArgs>(EventNamePopup.EVENT_POPUP_UI, OnPopup);
        EventManager.StartListening<PopupArgs>(EventNamePopup.EVENT_CLOSE_ALL_POPUP_UI, CloseAllPopup);
        EventManager.StartListening<PopupArgs>(EventNamePopup.EVENT_CLOSE_BY_NAME, CloseByName);
    }

    void OnDestroy()
    {
        EventManager.StopListening<PopupArgs>(EventNamePopup.EVENT_POPUP_UI, OnPopup);
        EventManager.StopListening<PopupArgs>(EventNamePopup.EVENT_CLOSE_ALL_POPUP_UI, CloseAllPopup);
        EventManager.StopListening<PopupArgs>(EventNamePopup.EVENT_CLOSE_BY_NAME, CloseByName);
    }

    async void OnPopup(PopupArgs args)
    {
        GameObject go = Instantiate(await GameAsset.GetPrefabAsync(args.popupName), this.transform);
        go.name = args.popupName;
        PopupBase popup = go.GetComponent<PopupBase>();
        popup.OnOpen(args);
    }

    public void  CloseAllPopup(PopupArgs args)
    {
        foreach (Transform item in transform)
        {
            item.GetComponent<PopupBase>().OnClose();
        }
    }

    public void CloseByName(PopupArgs args)
    {
        var temp = transform.Find(args.popupName);
        if(temp != null)
        {
            temp.GetComponent<PopupBase>().OnClose();
        }
    }
}
