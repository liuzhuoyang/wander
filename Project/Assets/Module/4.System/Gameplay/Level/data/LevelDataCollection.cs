
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataCollection", menuName = "OniData/Data/Level/LevelDataCollection", order = 1)]
public class LevelDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<LevelData> listLevelData;
#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {   
        base.InitData();
        listLevelData = FileFinder.FindAllAssetsOfAllSubFolders<LevelData>(path);
    }
#endif
}

public static class AllLevel
{
    //数据游戏中使用
    public static Dictionary<string, LevelData> dictData;
    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, LevelData>();
        LevelDataCollection collection = GameDataControl.Instance.Get("all_level") as LevelDataCollection;
        foreach (LevelData data in collection.listLevelData)
        {
            dictData.Add(data.name, data);
        }
    }
}