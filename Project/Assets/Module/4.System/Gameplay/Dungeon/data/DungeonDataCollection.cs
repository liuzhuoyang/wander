
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_dungeon", menuName = "OniData/System/Gameplay/Dungeon/DungeonDataCollection", order = 1)]
public class DungeonDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<DungeonData> listDungeonData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listDungeonData = AssetsFinder.FindAllAssets<DungeonData>(path);
    }
#endif
}

public static class AllDungeon
{
    //数据游戏中使用
    public static Dictionary<string, DungeonData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, DungeonData>();
        DungeonDataCollection dungeonDataAll = GameDataControl.Instance.Get("all_dungeon") as DungeonDataCollection;
        foreach (DungeonData dungeonData in dungeonDataAll.listDungeonData)
        {
            dictData.Add(dungeonData.dungeonName, dungeonData);
        }
    }
}