using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

//数据编辑器使用，这个scriptableObject包含所有遗物数据的List，游戏开始的时候先加载这个资源，后续转换成AllItem来使用
[Serializable]
[CreateAssetMenu(fileName = "all_item", menuName = "OniData/Generic/VFX/ItemDataCollection", order = 1)]
public class ItemDataCollection : GameDataCollectionBase
{
    //单个资源的路径
    //资源列表
    [ReadOnly]
    public List<ItemData> listItemData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listItemData = FileFinder.FindAllAssetsOfAllSubFolders<ItemData>(path);
    }
#endif
}

public static class AllItem
{
    //数据游戏中使用
    public static Dictionary<string, ItemData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, ItemData>();
        ItemDataCollection collection = GameDataControl.Instance.Get("all_item") as ItemDataCollection;
        foreach (ItemData data in collection.listItemData)
        {
            dictData.Add(data.name, data);
        }
    }
}