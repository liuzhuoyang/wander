
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// 计算宝箱物品时候使用
public class ChestArgs
{
    public string itemName;
    public int num;
    public Rarity rarity;
}

[Serializable]
[CreateAssetMenu(fileName = "chest_data", menuName = "OniData/Data/Chest/ChestData", order = 1)]
public class ChestData : ScriptableObject
{
    [PreviewField(55)]
    [BoxGroup("Info", LabelText = "基础信息", Order = 0)]
    public Texture2D icon;

    [ReadOnly]
    [BoxGroup("Info")]
    public string chestName;

    [BoxGroup("Info")]
    [TableColumnWidth(100, Resizable = false)]
    [ValueDropdown("GetLocalizationKeyList")]
    public string displayName;

    [BoxGroup("Reward")]
    [TableList]
    public List<ItemDataSlot> listRewardSlot;

#if UNITY_EDITOR
    [BoxGroup("Action")]
    [Button("初始化文件 Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        chestName = this.name;
    }

    public List<string> GetLocalizationKeyList()
    {
        List<string> listKey = new List<string>();
        string path = GameDataControl.GetLocPath("all_chest");
        List<LocalizationData> list = FileFinder.FindAllAssetsOfAllSubFolders<LocalizationData>(path);
        foreach (LocalizationData data in list)
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

   
