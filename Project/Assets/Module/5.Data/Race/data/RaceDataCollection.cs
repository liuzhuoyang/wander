using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
[CreateAssetMenu(fileName = "all_race", menuName = "OniData/Data/Race/RaceDataCollection", order = 1)]
public class RaceDataCollection : GameDataCollectionBase
{
    [ReadOnly]
    public List<RaceData> listRaceData;

    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {
        base.InitData();
        listRaceData = AssetsFinder.FindAllAssetsOfAllSubFolders<RaceData>(path);
    }
}

public static class AllRace
{
    public static Dictionary<string, RaceData> dictData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, RaceData>();
        RaceDataCollection collection = GameDataControl.Instance.Get("all_race") as RaceDataCollection;
        foreach (RaceData data in collection.listRaceData)
        {
            dictData.Add(data.name, data);
        }
    }
}
