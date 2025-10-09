using System;
using UnityEngine;

public enum SequenceTaskType
{
    //不要修改，用户数据用到，需要保持一致
    PlotSequence, //剧情序列
    Plot, //剧情
    Feature, //功能
    Popup, //弹窗
    Function = 901, //功能
}

public class SequenceTaskArgs
{
    //序列类型
    public SequenceTaskType seqTaskType;
    //剧情序列
    public string plotSeqID;
    public string className;
    public string methodName;
    //弹窗名称
    public string popupName;
    //解锁功能的类型
    public FeatureType unlockFeatureType;
}

public class GPTriggerArgs : EventArgs
{

}

public class GPTriggerEventName
{
    public const string EVENT_UI_CLOSED = "EVENT_UI_CLOSED";
    public const string EVENT_TUT_PLOT_COMPLETED = "EVENT_TUT_PLOT_COMPLETED";
}