using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//数据编辑器使用，这个scriptableObject包含所有遗物数据的List，游戏开始的时候先加载这个资源，后续转换成AllRelic来使用
[Serializable]
[CreateAssetMenu(fileName = "all_iap", menuName = "OniData/Monetization/IAP/IapDataCollection", order = 1)]
public class IAPDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<IAPData> listIapData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listIapData = AssetsFinder.FindAllAssetsOfAllSubFolders<IAPData>(path);
    }
#endif
}

public static class AllIap
{
    //数据游戏中使用
    public static Dictionary<string, IAPData> dictData;

    //初始化数据，从资源中加载
    public static void Init()
    {
        dictData = new Dictionary<string, IAPData>();
        IAPDataCollection collection = GameDataControl.Instance.Get("all_iap") as IAPDataCollection;
        foreach (IAPData data in collection.listIapData)
        {
            dictData.Add(data.name, data);
        }
    }
}