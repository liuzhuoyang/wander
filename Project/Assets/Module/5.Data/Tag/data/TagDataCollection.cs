
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

[Serializable]
[CreateAssetMenu(fileName = "all_tal", menuName = "OniData/Data/Tag/TagDataCollection", order = 1)]
public class TagDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<TagData> listTagData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listTagData = FileFinder.FindAllAssetsOfAllSubFolders<TagData>(path);
    }
#endif
}

public static class AllTag
{
    //数据游戏中使用
    public static Dictionary<string, TagData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, TagData>();
        TagDataCollection collection = GameDataControl.Instance.Get("all_tag") as TagDataCollection;
        foreach (TagData data in collection.listTagData)
        {
            dictData.Add(data.name, data);
        }
    }
}