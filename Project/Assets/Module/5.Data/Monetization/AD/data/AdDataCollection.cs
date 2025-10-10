
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_ad", menuName = "OniData/Monetization/Ad/AdDataCollection", order = 1)]
public class AdDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<AdData> listAdData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listAdData = FileFinder.FindAllAssets<AdData>(path);
    }
#endif
}

public static class AllAd
{
    public static Dictionary<AdType, AdData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<AdType, AdData>();
        AdDataCollection collection = GameDataControl.Instance.Get("all_ad") as AdDataCollection;
        foreach (AdData adData in collection.listAdData)
        {
            dictData.Add(adData.adType, adData);
        }
    }
}