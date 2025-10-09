using UnityEngine;
using UnityEngine.UI;

public class UIBackground : UIBase
{
    public GameObject objMain;
    public Image imgBG;

    void Start()
    {
        EventManager.StartListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_SHOW, OnShowUIBackground);
        EventManager.StartListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_HIDE, OnHideUIBackground);
        EventManager.StartListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_REFRESH, OnRefreshUIBackground);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_SHOW, OnShowUIBackground);
        EventManager.StopListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_HIDE, OnHideUIBackground);
        EventManager.StopListening<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_REFRESH, OnRefreshUIBackground);
    }

    void OnShowUIBackground(UIBackgroundArgs args)
    {
        objMain.SetActive(true);
    }

    void OnHideUIBackground(UIBackgroundArgs args)
    {
        objMain.SetActive(false);
    }

    void OnRefreshUIBackground(UIBackgroundArgs args)
    {
        GameAssetControl.AssignPicture("ui_bg_" + args.themeName, imgBG);
    }
}
