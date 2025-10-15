
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

[Serializable]
[CreateAssetMenu(fileName = "LevelDataCollection", menuName = "OniData/Data/Level/LevelDataCollection", order = 1)]
public class LevelDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<LevelData> listLevelData;

    [ReadOnly]
    public List<LevelData> listLevelDungeonData;

#if UNITY_EDITOR

    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {   
        base.InitData();
        listLevelData = FileFinder.FindAllAssetsOfAllSubFolders<LevelData>(path + "main/");
        listLevelDungeonData =  FileFinder.FindAllAssetsOfAllSubFolders<LevelData>(path + "dungeon/");

        foreach (LevelData data in listLevelData)
        {
            data.InitData();
        }
    }

    public void OnInitLevelIndex()
    {
        for (int i = 0; i < listLevelData.Count; i++)
        {
            listLevelData[i].levelIndex = i;
        }
        for (int i = 0; i < listLevelDungeonData.Count; i++)
        {
            listLevelDungeonData[i].levelIndex = i;
        }
    }
#endif
}

public static class AllLevel
{
    //数据游戏中使用
    public static Dictionary<string, LevelData> dictData;
    public static Dictionary<int, LevelData> dictMainLevelData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, LevelData>();
        LevelDataCollection collection = GameDataControl.Instance.Get("all_level") as LevelDataCollection;
        foreach (LevelData data in collection.listLevelData)
        {
            dictData.Add(data.levelName, data);
            data.Init();
        }

        foreach (LevelData data in collection.listLevelDungeonData)
        {
            dictData.Add(data.levelName, data);
            data.Init();
        }

        dictMainLevelData = new Dictionary<int, LevelData>();
        foreach(LevelData data in collection.listLevelData)
        {
            if(data.levelType == LevelType.Main)
            {
                dictMainLevelData.Add(data.levelIndex, data);
            }
        }
    }
}