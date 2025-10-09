using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using TGame;

public class UISetting : UIBase
{
    public GameObject objSecondaryPage;
    public GameObject objMainPage;
    public SettingViewPrivacy viewPrivacy;
    public SettingViewProgress viewProgress;
    public SettingViewLanguage viewLanguage;
    [SerializeField] SettingViewOrder viewOrder;

    public GameObject objSoundOn;
    public GameObject objSoundOff;
    public GameObject objMusicOn;
    public GameObject objMusicOff;
    public GameObject objNotiOn;
    public GameObject objNotiOff;
    public GameObject objHapticOn;
    public GameObject objHapticOff;

    public TextMeshProUGUI textUDID;
    public TextMeshProUGUI textVersion;

    void Awake()
    {
        objSecondaryPage.SetActive(true);

        Reset();
        //EventManager.StartListening<UISettingArgs>(EventName.EVENT_SETTING_OPEN_UI, OpenUI);
        EventManager.StartListening<UISettingArgs>(EventNameSetting.EVENT_SETTING_REFRESH_UI, RefreshUI);
        EventManager.StartListening<UISettingArgs>(EventNameSetting.EVENT_SETTING_CLOSE_UI, OnCloseUI);
    }

    private void OnDestroy()
    {
        //EventManager.StopListening<UISettingArgs>(EventName.EVENT_SETTING_OPEN_UI, OpenUI);
        EventManager.StopListening<UISettingArgs>(EventNameSetting.EVENT_SETTING_REFRESH_UI, RefreshUI);
        EventManager.StopListening<UISettingArgs>(EventNameSetting.EVENT_SETTING_CLOSE_UI, OnCloseUI);
    }

    private void Reset()
    {
        viewLanguage.OnClose();
        viewOrder.OnClose();
        viewProgress.OnClose();
        viewPrivacy.OnClose();
    }


    void RefreshUI(UISettingArgs args)
    {
        Reset();

        objSoundOn.SetActive(args.isSoundOn);
        objSoundOff.SetActive(!args.isSoundOn);
        objMusicOn.SetActive(args.isMusicOn);
        objMusicOff.SetActive(!args.isMusicOn);
        objNotiOn.SetActive(args.isNotiOn);
        objNotiOff.SetActive(!args.isNotiOn);
        objHapticOn.SetActive(args.isHapticOn);
        objHapticOff.SetActive(!args.isHapticOn);

        textUDID.text = "USER ID: " + args.uid;

        // if (TempData.isUnknownUser)
        // {
        //     textUDID.text = "UNKNOWN USER"; //TODO
        // }

        if (GameConfig.main.productMode != ProductMode.DevOffline)
        {
            textVersion.text = "version." + Application.version.ToString(); //+ "." + TGameSDK.OPStrategyVersion();
        }
        else
        {
            textVersion.text = Application.version;
        }
    }

    public void OnPrivacyPage()
    {
        viewPrivacy.OnOpen();
    }

    public void OnSupport()
    {
        SettingManager.Instance.OnSupport();
    }

    public void OnSound()
    {
        SettingManager.Instance.OnSound();
    }

    public void OnMusic()
    {
        SettingManager.Instance.OnMusic();
    }

    public void OnNotification()
    {
        SettingManager.Instance.OnNoti();
    }

    public void OnHaptic()
    {
        SettingManager.Instance.OnHaptic();
    }

    public void OnProgressPage()
    {
        viewProgress.OnOpen();
    }

    public void OnLanguagePage()
    {
        viewLanguage.OnOpen();
    }

    public void OnOrderPage()
    {
        viewOrder.OnOpen();
    }

    public void OnCopy()
    {
        SettingManager.Instance.OnCopyUserID();
    }

    void OnCloseUI(UISettingArgs args)
    {
        base.CloseUI();
        TerminalSystem.Instance.Open();
    }

    public void OnClose()
    {
        base.CloseUI();
        TerminalSystem.Instance.Open();
    }
}
