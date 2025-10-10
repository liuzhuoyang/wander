using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AvatarFrameData", menuName = "OniData/Social/Avatar/AvatarFrameData", order = 1)]
public class AvatarFrameData : ScriptableObject
{
    [ReadOnly]
    public string avatarFrameName;

    [LabelText("Unlock Condition Description")]
    [ValueDropdown("GetLocalizationKey")]
    public string unlockConditionDescription;

    [LabelText("Gear Attack Bonus")]
    public float gearAttackBonus;
    [LabelText("Base Health Bonus")]
    public int baseHealthBonus;

    public bool isDefault;

#if UNITY_EDITOR
    [Button("Init Data")]
    public void InitData()
    {
        avatarFrameName = this.name;
    }

    public List<string> GetLocalizationKey()
    {
        List<string> listKey = new List<string>();
        string path = GameDataControl.GetLocPath("avatar_frame");
        List<LocalizationData> listAssets = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in listAssets)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
#endif
}