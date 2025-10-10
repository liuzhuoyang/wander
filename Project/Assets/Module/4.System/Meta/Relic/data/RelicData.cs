using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "relic_data", menuName = "OniData/System/Meta/Relic/RelicData", order = 1)]
public class RelicData : ScriptableObject
{   
    [BoxGroup("Info", LabelText = "基础信息")]
    [LabelText("遗物名称")]
    [ReadOnly]
    public string relicName;

#if UNITY_EDITOR
    [BoxGroup("Preview", LabelText = "预览信息")]
    [LabelText("遗物显示名称")]
    [ReadOnly]
    public string previewDisplayName;

    [BoxGroup("Preview")]
    [LabelText("遗物描述")]
    [ReadOnly]
    public string previewInfo;
#endif

    [BoxGroup("Edit", LabelText = "可编辑内容")]
    [LabelText("显示名称")]
    [ValueDropdown("GetLocalizationKey")]
    [OnValueChanged("OnDropDownValueChanged")]
    public string displayName;

    [BoxGroup("Edit")]
    [LabelText("遗物描述")]
    [ValueDropdown("GetLocalizationKey")]
    [OnValueChanged("OnDropDownValueChanged")]
    public string info;

    [BoxGroup("Edit")]
    [LabelText("遗物品质")]
    public Rarity rarity;

    [BoxGroup("Edit")]
    [LabelText("影响伤害类型")]
    public DamageType effectDamageType;

    [BoxGroup("Edit")]
    [LabelText("影响属性")]
    [ValueDropdown("GetFilteredAttribute")]
    public AttributeType effectAttributeType;

    [BoxGroup("Edit")]
    [LabelText("被动属性")]
    public RelicPassiveType passiveType;

    [BoxGroup("Edit")]
    [LabelText("值(百分比)")]
    public float value;

#if UNITY_EDITOR
    [BoxGroup("Action", LabelText = "操作")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public void InitData()
    {
        relicName = this.name;
        EditorUtility.SetDirty(this);
    }

    private List<string> GetLocalizationKey()
    {
        List<string> listKey = new List<string>();
        string path = GameDataControl.GetLocPath("all_relic");
        List<LocalizationData> listAsset = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData asset in listAsset)
        {
            foreach (LocalizationSerializedItem item in asset.list)
            {
                listKey.Add(item.key);
            }
        }
        return listKey;
    }

    private void OnDropDownValueChanged()
    {
        string path = GameDataControl.GetLocPath("all_relic");
        previewDisplayName = LocalizationDataCollection.GetValue(displayName);
        previewInfo = LocalizationDataCollection.GetValue(info);
    }

    private IEnumerable<ValueDropdownItem<AttributeType>> GetFilteredAttribute()
    {
        yield return new ValueDropdownItem<AttributeType>("伤害", AttributeType.Damage);
        yield return new ValueDropdownItem<AttributeType>("暴击率", AttributeType.CritRate);
        yield return new ValueDropdownItem<AttributeType>("暴击伤害", AttributeType.CritDamage);
        yield return new ValueDropdownItem<AttributeType>("攻击速度", AttributeType.AttackSpeed);

    }
#endif
}