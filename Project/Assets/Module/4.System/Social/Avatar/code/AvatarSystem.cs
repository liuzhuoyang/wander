using UnityEngine;

public class AvatarSystem : Singleton<AvatarSystem>
{
    UserAvatar userAvatar;
    public void Init()
    {
        userAvatar = GameData.userData.userAvatar;

        //解锁默认头像
        foreach (AvatarData avatarData in AllAvatar.GetPlayerAvatarList())
        {
            if (!userAvatar.listUnlockedAvatar.Contains(avatarData.avatarName))
            {
                UnlockAvatar(avatarData.avatarName);
            }
        }
    }

    public void UnlockAvatar(string avatarName)
    {
        if (!userAvatar.listUnlockedAvatar.Contains(avatarName))
        {
            userAvatar.listUnlockedAvatar.Add(avatarName);
        }
    }

    public bool IsAvatarUnlocked(string avatarName)
    {
        return userAvatar.listUnlockedAvatar.Contains(avatarName);
    }

    public void UnlockAvatarFrame(string avatarFrameName)
    {
        if (!userAvatar.listUnlockedAvatarFrame.Contains(avatarFrameName))
        {
            userAvatar.listUnlockedAvatarFrame.Add(avatarFrameName);
        }
    }

    public bool IsAvatarFrameUnlocked(string avatarFrameName)
    {
        return userAvatar.listUnlockedAvatarFrame.Contains(avatarFrameName);
    }    
}
