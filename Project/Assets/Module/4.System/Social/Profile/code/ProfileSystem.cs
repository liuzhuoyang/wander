using System;
using UnityEngine;

public class ProfileSystem : Singleton<ProfileSystem>
{
    const string DEFAULT_AVATAR = "avatar_001";
    const string DEFAULT_AVATAR_FRAME = "avatar_frame_01";

    UserProfile userProfile;
    string selectedAvatar;
    string selectedAvatarFrame;

    Action onComplete;
    public void Init()
    {
        userProfile = GameData.userData.userProfile;
        if (string.IsNullOrEmpty(userProfile.userName))
        {
            string userName = UtilityLocalization.GetLocalization("mapping/character/mapping_character_main");
            //判断id是否超过四位 取后四位
            int userID = GameData.userData.userAccount.userID;
            if (userID > 9999)
            {
                userName = userName + userID.ToString().Substring(userID.ToString().Length - 4);
            }
            else
            {
                //未达到四位，补齐
                userName = userName + userID.ToString().PadLeft(4, '0');
            }

            userProfile.userName = userName;
        }

        //保护
        if (string.IsNullOrEmpty(userProfile.userAvatar))
        {
            userProfile.userAvatar = DEFAULT_AVATAR;
        }
        else if (!AllAvatar.GetPlayerAvatarNameList().Contains(userProfile.userAvatar))
        {
            userProfile.userAvatar = DEFAULT_AVATAR;
        }

        if (string.IsNullOrEmpty(userProfile.userFrame))
        {
            userProfile.userFrame = DEFAULT_AVATAR_FRAME;
        }

        selectedAvatar = userProfile.userAvatar;
        selectedAvatarFrame = userProfile.userFrame;
    }

    public async void Open()
    {
        await UIMain.Instance.OpenUI("profile", UIPageType.Normal);
        RefreshUI();
    }

    //ProfileID是第一次录入名字，这部分用于Plot的Seq，要处理一下完成情况
    public void OnCloseProfileID()
    {
        onComplete?.Invoke();
        onComplete = null;
    }

    public async void OpenProfileID(Action onComplete)
    {
        this.onComplete = onComplete;
        await UIMain.Instance.OpenUI("profile_id", UIPageType.Normal);
        EventManager.TriggerEvent<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_INIT_UI, new UIProfileArgs
        {
            userName = userProfile.userName,
            userAvatar = userProfile.userAvatar,
        });
    }

    void RefreshUI()
    {
        EventManager.TriggerEvent<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_REFRESH_UI, new UIProfileArgs
        {
            userName = userProfile.userName,
            userAvatar = userProfile.userAvatar,
        });
    }

    public void SetUserName(string userName)
    {
        userProfile.userName = userName;
        HeaderControl.OnRefreshProfile();
        RefreshUI();
        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.ChangeProfile,
        });
    }

    public void OnSelectAvatar(string avatarName)
    {
        selectedAvatar = avatarName;
    }

    public void SetUserAvatar(Action onConfirm)
    {
        if (!AvatarSystem.Instance.IsAvatarUnlocked(selectedAvatar))
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("generic/locked"));
            return;
        }

        onConfirm?.Invoke();
        userProfile.userAvatar = selectedAvatar;
        HeaderControl.OnRefreshProfile();
        RefreshUI();

        EventManager.TriggerEvent<UIProfileArgs>(EventNameProfile.EVENT_PROFILE_STARTUP_REFRESH_UI, new UIProfileArgs
        {
            userAvatar = selectedAvatar,
        });

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.ChangeProfile,
        });
    }

    public void OnSelectAvatarFrame(string avatarFrameName)
    {
        selectedAvatarFrame = avatarFrameName;
    }

    public void SetUserAvatarFrame(Action onConfirm)
    {
        if (!AvatarSystem.Instance.IsAvatarFrameUnlocked(selectedAvatarFrame))
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("generic/locked"));
            return;
        }

        onConfirm?.Invoke();

        userProfile.userFrame = selectedAvatarFrame;
        HeaderControl.OnRefreshProfile();
        RefreshUI();

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.ChangeProfile,
        });
    }
}
