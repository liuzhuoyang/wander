using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

[Serializable]
public class GearDataEditor
{
    [LabelText("等级")] public int level;

    [LabelText("属性")] public AttributeType attributeType;

    [LabelText("值")] public float value;
}

[CreateAssetMenu(fileName = "GearData", menuName = "OniData/System/Meta/Gear/GearData", order = 1)]
public class GearData : ScriptableObject
{
    [LabelText("装备名")] public string gearName;

    [LabelText("识别名(编辑器内才会显示)")] public string editorName;

    [LabelText("显示名称"), ValueDropdown("GetLocalizationKeyList")] public string displayName;

    [LabelText("描述"), ValueDropdown("GetLocalizationKeyList")] public string info;

    [LabelText("品质")] public Rarity rarity;

    [LabelText("攻击力")] public int attack;

    [LabelText("触发圈速度")] public int triggerSpeed;

    [LabelText("属性解锁")] public List<GearDataEditor> listData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public void InitData()
    {
        EditorUtility.SetDirty(this);
    }

    private List<string> GetLocalizationKeyList()
    {
        List<string> list = new List<string>();
        list.Add("");
        string path = GameDataControl.GetLocPath("all_gear");
        List<LocalizationData> listLocalizationAsset = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in listLocalizationAsset)
        {
            foreach (LocalizationSerializedItem item in data.list)
            {
                list.Add(item.key);
            }
        }
        return list;
    }
#endif
}