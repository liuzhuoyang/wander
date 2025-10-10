
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_feature", menuName = "OniData/Generic/Feature/FeatureDataCollection", order = 1)]
public class FeatureDataCollection : GameDataCollectionBase
{
    [ReadOnly]
    public const string LOC_PATH = "Assets/Module/2.Generic/Feature/loc/";

    //资源列表
    [ReadOnly]
    public List<FeatureData> listFeatureData;
    
#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listFeatureData = new List<FeatureData>();
        List<FeatureData> list = FileFinder.FindAllAssetsOfAllSubFolders<FeatureData>(path);
        foreach (FeatureData featureData in list)
        {
            listFeatureData.Add(featureData);
        }
    }

#endif
}

public static class AllFeature
{
    public static Dictionary<FeatureType, FeatureData> dictData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<FeatureType, FeatureData>();
        FeatureDataCollection collection = GameDataControl.Instance.Get("all_feature") as FeatureDataCollection;
        foreach (FeatureData data in collection.listFeatureData)
        {
            dictData.Add(data.featureType, data);
        }
    }
}