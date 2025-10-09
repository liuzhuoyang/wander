using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

//数据编辑器使用，这个scriptableObject包含所有遗物数据的List，游戏开始的时候先加载这个资源，后续转换成AllRelic来使用
[Serializable]
[CreateAssetMenu(fileName = "all_sku", menuName = "OniData/Monetization/SKU/SKUDataCollection", order = 1)]
public class SKUDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<SKUData> listSKUData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listSKUData = AssetsFinder.FindAllAssets<SKUData>(path);
    }
#endif
}

public static class AllSKU
{
    public static Dictionary<string, SKUData> dictData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, SKUData>();
        SKUDataCollection collection = GameDataControl.Instance.Get("all_sku") as SKUDataCollection;
        foreach (SKUData data in collection.listSKUData)
        {
            dictData.Add(data.skuID, data);
        }
    }
}
