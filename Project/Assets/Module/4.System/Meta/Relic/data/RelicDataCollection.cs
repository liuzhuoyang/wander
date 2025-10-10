using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

//数据编辑器使用，这个scriptableObject包含所有遗物数据的List，游戏开始的时候先加载这个资源，后续转换成AllRelic来使用
[Serializable]
[CreateAssetMenu(fileName = "RelicDataCollection", menuName = "OniData/System/Meta/Relic/RelicDataCollection", order = 1)]
public class RelicDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<RelicData> listRelicData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listRelicData = FileFinder.FindAllAssets<RelicData>(path);
    }
#endif
}

public static class AllRelic
{
    //数据游戏中使用
    public static Dictionary<string, RelicData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, RelicData>();
        RelicDataCollection collection = GameDataControl.Instance.Get("all_relic") as RelicDataCollection;
        foreach (RelicData data in collection.listRelicData)
        {
            dictData.Add(data.name, data);
        }
    }
}