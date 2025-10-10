using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum TagType
{
    RaceTag,
    UnitTag,
    GearTag,
    PawnTag,
}

[Serializable]
[CreateAssetMenu(fileName = "TagData", menuName = "OniData/Generic/Tag/TagData", order = 1)]
public class TagData : ScriptableObject
{
    [ReadOnly]
    public string tagKey;

    public TagType tagType;

    [LabelText("标签名称")]
    [ValueDropdown("GetLocalizationKey")]
    public string displayName;

    [LabelText("标签描述")]
    [ValueDropdown("GetLocalizationKey")]
    public string contentKey;

    [LabelText("标签颜色")]
    public Color color;

    [ShowIf("tagType", TagType.RaceTag)]
    [LabelText("抗性")]
    public List<Resistance> resistanceList;
#if UNITY_EDITOR

    [Button("Init Data")]
    public void InitData()
    {
        tagKey = this.name;
    }

    public List<string> GetLocalizationKey()
    {
        List<string> listKey = new List<string>();
        List<LocalizationData> listAssets = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(EditorPathUtility.localizationPath + "/ui/tag");
        foreach (LocalizationData asset in listAssets)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
#endif
}

[Serializable]
public struct Resistance
{
    public DamageType damageType;
    public float resistances; //抗性强度，例如 热熔 +0.5, 表示收到的热能伤害-50%, -0.5表示受到的热能伤害+50%

    [ValueDropdown("GetLocalizationKeyListInfo")]
    public string resistanceInfo;

#if UNITY_EDITOR

    public List<string> GetLocalizationKeyListInfo()
    {
        List<string> listKey = new List<string>();
        listKey.Add("");
        List<LocalizationData> listAssets = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(EditorPathUtility.localizationPath + "/ui/mapping/loc_mapping_unit");
        foreach (LocalizationData asset in listAssets)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }
#endif
}