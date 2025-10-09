
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "plot_seq_", menuName = "OniData/System/Plot/PlotSequence/PlotSequenceData", order = 1)]

public class PlotSequenceData : ScriptableObject
{
    [ReadOnly]
    public string plotSeqID;

    public float delay;

    [LabelText("上线是否删除")]
    public bool isDeleteOnRelease;

    [TableList]
    [HideLabel]
    public List<PlotSequenceItem> listSequenceTaskItem;

#if UNITY_EDITOR
    [Button("Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        plotSeqID = this.name;

        foreach (PlotSequenceItem item in listSequenceTaskItem)
        {
            item.OnInitData();
        }
    }
#endif
}

[Serializable]
public class PlotSequenceItem
{
    [BoxGroup("Main", ShowLabel = false)]
    [VerticalGroup("Main/Ver")]
    public PlotSequenceType seqItemType;

    [ReadOnly]
    [VerticalGroup("Main/Ver")]
    public string seqItemName;

#if UNITY_EDITOR
    [BoxGroup("Asset", ShowLabel = false)]
    [VerticalGroup("Asset/Ver")]
    [ShowIf("seqItemType", PlotSequenceType.Plot)]
    public PlotData plotData;

    [ShowIf("seqItemType", PlotSequenceType.Tut)]
    [VerticalGroup("Asset/Ver")]
    public TutData tutData;

    public void OnInitData()
    {
        try
        {
            switch (seqItemType)
            {
                case PlotSequenceType.Plot:
                    seqItemName = plotData.plotName;
                    break;
                case PlotSequenceType.Tut:
                    seqItemName = tutData.tutName;
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"有资源没有配置，初始化数据失败 {e.Message}");
            return;
        }
    }
#endif
}

