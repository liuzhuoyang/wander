
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllAchievement来使用
[Serializable]
[CreateAssetMenu(fileName = "all_plot_seq", menuName = "OniData/System/Plot/PlotSequence/PlotSequenceDataCollection", order = 1)]
public class PlotSequenceDataCollection : GameDataCollectionBase
{

    [ReadOnly]
    public List<PlotSequenceData> listSequenceTaskData;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listSequenceTaskData = FileFinder.FindAllAssets<PlotSequenceData>(path);
    }
#endif
}

public static class AllPlotSequence
{
    //数据游戏中使用
    public static Dictionary<string, PlotSequenceData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, PlotSequenceData>();
        PlotSequenceDataCollection collection = GameDataControl.Instance.Get("all_plot_seq") as PlotSequenceDataCollection;
        foreach (PlotSequenceData data in collection.listSequenceTaskData)
        {
            dictData.Add(data.name, data);
        }
    }
}
