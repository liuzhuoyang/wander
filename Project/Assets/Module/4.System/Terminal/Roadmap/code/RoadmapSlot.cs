using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoadmapSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textName, textUnlock;
    [SerializeField] GameObject objClaimed, objCanClaim;
    [SerializeField] Image imgIcon;

    RoadmapSlotArgs slotArgs;
    public void Init(RoadmapSlotArgs slotArgs)
    {
        this.slotArgs = slotArgs;
        FeatureData featureData = AllFeature.dictData[slotArgs.featureType];
        GameAssetControl.AssignSpriteUI(featureData.iconName, imgIcon);
        textName.text = UtilityLocalization.GetLocalization(featureData.displayName);
        string content = UtilityLocalization.GetLocalization("feature/dynamic/unlock_at_level_x", $"{featureData.unlockLevelID}");
        textUnlock.text = content;
        RefreshBtn();
    }

    public void RefreshBtn()
    {
        objClaimed.SetActive(slotArgs.isClaimed);
        objCanClaim.SetActive(slotArgs.canClaim);
    }

    public void OnClaim()
    {
        RoadmapSystem.Instance.OnClaim(slotArgs.featureType);
    }
}