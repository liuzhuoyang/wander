using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "all_loot", menuName = "OniData/System/Meta/Loot/LootDataList", order = 1)]
public class LootDataCollection : GameDataCollectionBase
{
    [BoxGroup("info", LabelText =  "基础信息")]
    //资源列表
    [ReadOnly]
    public List<LootData> listLootData;

#if UNITY_EDITOR
    [BoxGroup("Action", LabelText =  "初始化")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {
        base.InitData();
        listLootData = FileFinder.FindAllAssets<LootData>(path);
    }
#endif
}

public static class AllLoot
{
    //数据游戏中使用
    public static Dictionary<int, LootData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<int, LootData>();
        LootDataCollection collection = GameDataControl.Instance.Get("all_loot") as LootDataCollection;
        foreach (LootData data in collection.listLootData)
        {
            dictData.Add(data.lootIndex, data);
        }
    }
}