using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllChapter来使用
[Serializable]
[CreateAssetMenu(fileName = "all_chest", menuName = "OniData/Data/Chest/ChestDataCollection", order = 1)]
public class ChestDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<ChestData> listChestData;

    #if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listChestData = FileFinder.FindAllAssets<ChestData>(path);
    }
    #endif
}

public static class AllChest
{
    //数据游戏中使用
    public static Dictionary<string, ChestData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, ChestData>();
        ChestDataCollection collection = GameDataControl.Instance.Get("all_chest") as ChestDataCollection;
        foreach (ChestData data in collection.listChestData)
        {
            dictData.Add(data.chestName, data);
        }
    }
}
