using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIProfile : UIBase
{
    [SerializeField] TextMeshProUGUI userName, userID;
    [SerializeField] AvatarSlotView avatarSlotView;
    // [SerializeField] GameObject objHubVip;
    [SerializeField] TextMeshProUGUI textVipLevel;
    [SerializeField] TextMeshProUGUI textServer;

    void Awake()
    {
        EventManager.StartListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_REFRESH_UI, OnRefreshUI);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_REFRESH_UI, OnRefreshUI);
    }

    void OnRefreshUI(UIProfileArgs args)
    {
        // if (GameConfig.main.productMode == ProductMode.DevOffline)
        // {
        //     objHubVip.SetActive(true);
        //     textVipLevel.text = "VipLevel: " + VipSystem.Instance.GetVipLevel();
        // }
        // else
        // {
        //     objHubVip.SetActive(false);
        // }
        userName.text = args.userName;
        int uid = GameData.userData.userAccount.userID;
        userID.text = "UID: " + (uid < 0 ? "U" + Math.Abs(uid) : uid.ToString());
        int serverID = GameData.userData.userServer.serverID;
        textServer.text = UtilityLocalization.GetLocalization("page/profile/page_profile_galaxy"
            , serverID < 0 ? "U" + Math.Abs(serverID) : serverID.ToString());
        AvatarData avatarData = AllAvatar.dictData[args.userAvatar];
        avatarSlotView.InitStatic(avatarData);
    }

    public void OnChangeAvatar()
    {
        Debug.Log("OnChangeAvatar");
        PopupChangeAvatarArgs args = new PopupChangeAvatarArgs();
        args.popupName = "popup_change_avatar";
        PopupManager.Instance.OnPopup<PopupChangeAvatarArgs>(args);
    }

    public void OnChangeAvatarFrame()
    {
        PopupChangeAvatarFrameArgs args = new PopupChangeAvatarFrameArgs();
        args.popupName = "popup_change_avatar_frame";
        PopupManager.Instance.OnPopup<PopupChangeAvatarFrameArgs>(args);
    }

    public void OnChangeName()
    {
        PopupChangeNameArgs args = new PopupChangeNameArgs();
        args.popupName = "popup_change_name";
        args.onConfirm = OnChangeNameConfirm;
        PopupManager.Instance.OnPopup<PopupChangeNameArgs>(args);
    }

    void OnChangeNameConfirm(string name)
    {
        ProfileSystem.Instance.SetUserName(name);
    }
}
