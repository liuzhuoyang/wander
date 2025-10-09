using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIProfileStartup : UIBase
{
    // [SerializeField] TMP_InputField inputField;

    // [SerializeField] AvatarSlotView avatarSlotView;
    [SerializeField] Image imgAvatar;

    [SerializeField] TextMeshProUGUI textName, textUserID, textSpecies, textBorn, textServer;

    string userAvatar;

    void Awake()
    {
        EventManager.StartListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_INIT_UI, OnInitUI);
        EventManager.StartListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_REFRESH_UI, OnRefreshUI);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_INIT_UI, OnInitUI);
        EventManager.StopListening<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_REFRESH_UI, OnRefreshUI);
    }

    void OnInitUI(UIProfileArgs args)
    {
        textName.text = GameData.userData.userProfile.userName;
        int uid = GameData.userData.userAccount.userID;
        textUserID.text = "UID: " + (uid < 0 ? "U" + Math.Abs(uid) : uid.ToString());
        int serverID = GameData.userData.userServer.serverID;
        textServer.text = UtilityLocalization.GetLocalization("page/profile/page_profile_galaxy"
            , serverID < 0 ? "U" + Math.Abs(serverID) : serverID.ToString());
        userAvatar = args.userAvatar;
        RefreshAvatar();
        HeaderControl.OnHide();
    }

    void OnRefreshUI(UIProfileArgs args)
    {
        userAvatar = args.userAvatar;
        RefreshAvatar();
    }

    void RefreshAvatar()
    {
        AvatarData avatarData = AllAvatar.dictData[userAvatar];
        GameAssetControl.AssignIcon(avatarData.avatarName, imgAvatar);
    }

    public void OnChangeAvatar()
    {
        PopupChangeAvatarArgs args = new PopupChangeAvatarArgs();
        args.popupName = "popup_change_avatar";
        PopupManager.Instance.OnPopup<PopupChangeAvatarArgs>(args);
    }


    //ProfileID,首次出现的ID页面的确认按钮
    public void OnProfileIDConfirm()
    {
        ProfileSystem.Instance.SetUserName(textName.text);
        CloseUI();

        HeaderControl.OnShow();

        ProfileSystem.Instance.OnCloseProfileID();
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
        textName.text = name;
    }
}
