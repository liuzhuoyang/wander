using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "all_gear", menuName = "OniData/System/Meta/Gear/GearDataList", order = 1)]
public class GearDataCollection : GameDataCollectionBase
{
    [BoxGroup("info", LabelText = "基础信息")]
    //资源列表
    [ReadOnly]
    public List<GearData> listGearData;

#if UNITY_EDITOR
    [BoxGroup("Action", LabelText = "初始化")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {
        base.InitData();
        listGearData = AssetsFinder.FindAllAssets<GearData>(path);
    }
#endif
}

public static class AllGear
{
    //数据游戏中使用
    public static Dictionary<string, GearData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, GearData>();
        GearDataCollection collection = GameDataControl.Instance.Get("all_gear") as GearDataCollection;
        foreach (GearData data in collection.listGearData)
        {
            dictData.Add(data.gearIndex, data);
        }
    }
}