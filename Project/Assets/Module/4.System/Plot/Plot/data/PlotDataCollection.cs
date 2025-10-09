using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllAchievement来使用
[Serializable]
[CreateAssetMenu(fileName = "all_plot", menuName = "OniData/System/Plot/Plot/PlotDataCollection", order = 1)]

public class PlotDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<PlotData> listPlotData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listPlotData = AssetsFinder.FindAllAssets<PlotData>(path);
    }
#endif
}

public static class AllPlot
{
    //数据游戏中使用
    public static Dictionary<string, PlotData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, PlotData>();
        PlotDataCollection collection = GameDataControl.Instance.Get("all_plot") as PlotDataCollection;
        foreach (PlotData data in collection.listPlotData)
        {
            dictData.Add(data.name, data);
        }
    }
}