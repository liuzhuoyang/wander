using UnityEngine;
using UnityEngine.UI;

public class AvatarSlotView : MonoBehaviour
{
    [SerializeField] Image iconAvatar;
    [SerializeField] Image iconFrame;
    [SerializeField] GameObject objSelected;
    [SerializeField] GameObject objLock;

    AvatarData avatarData;


    public void Init(AvatarData avatarData)
    {
        this.avatarData = avatarData;

        objSelected.SetActive(GameData.userData.userProfile.userAvatar == avatarData.avatarName);
        GameAssetControl.AssignIcon(avatarData.avatarName, iconAvatar);
        GameAssetControl.AssignSpriteUI("ui_" + GameData.userData.userProfile.userFrame, iconFrame);

        objLock.SetActive(!AvatarSystem.Instance.IsAvatarUnlocked(avatarData.avatarName));
    }

    //初始化静态头像 不能点击
    public void InitStatic(AvatarData avatarData)
    {
        this.avatarData = avatarData;

        Destroy(transform.GetComponent<Button>());
        Destroy(transform.GetComponent<ButtonFeedbackHandler>());

        objSelected.SetActive(false);
        objLock.SetActive(false);

        GameAssetControl.AssignIcon(avatarData.avatarName, iconAvatar);
        GameAssetControl.AssignSpriteUI("ui_" + GameData.userData.userProfile.userFrame, iconFrame);
    }

    public void ResetSelected()
    {
        objSelected.SetActive(false);
    }

    public void OnClick()
    {
        //instance.OnResetSelected();
        ProfileSystem.Instance.OnSelectAvatar(avatarData.avatarName);
        objSelected.SetActive(true);
    }
}
