using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarFrameSlotView : MonoBehaviour
{
    [SerializeField] Image iconAvatarFrame;
    [SerializeField] GameObject objSelected;
    [SerializeField] GameObject objLock;

    [SerializeField] TextMeshProUGUI textUnlockConditionDescription;
    [SerializeField] TextMeshProUGUI textGearAttackBonus;
    [SerializeField] TextMeshProUGUI textBaseHealthBonus;

    AvatarFrameData avatarFrameData;
    PopupChangeAvatarFrame instance;

    public void Init(AvatarFrameData avatarFrameData, PopupChangeAvatarFrame instance)
    {
        this.avatarFrameData = avatarFrameData;
        this.instance = instance;

        objSelected.SetActive(GameData.userData.userProfile.userFrame == avatarFrameData.avatarFrameName);
        GameAssetControl.AssignSpriteUI("ui_" + avatarFrameData.avatarFrameName, iconAvatarFrame);

        objLock.SetActive(!AvatarSystem.Instance.IsAvatarFrameUnlocked(avatarFrameData.avatarFrameName));

        textUnlockConditionDescription.text = UtilityLocalization.GetLocalization(avatarFrameData.unlockConditionDescription);
        textGearAttackBonus.text = "+ " + (int)(avatarFrameData.gearAttackBonus * 100) + "%";
        textBaseHealthBonus.text = "+" + avatarFrameData.baseHealthBonus;
    }

    public void ResetSelected()
    {
        objSelected.SetActive(false);
    }

    public void OnClick()
    {
        instance.OnResetSelected();
        ProfileSystem.Instance.OnSelectAvatarFrame(avatarFrameData.avatarFrameName);
        objSelected.SetActive(true);
    }
}
