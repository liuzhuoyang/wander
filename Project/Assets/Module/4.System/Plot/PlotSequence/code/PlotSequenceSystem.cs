using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlotSequenceSystem : Singleton<PlotSequenceSystem>
{
    int seqIndex = 0;
    PlotSequenceData currentPlotSeqData;
    Action onComplete;
    public void Init()
    {

    }

    //开始播放剧情序列
    public async void OnStartPlotSeq(string plotSeqID, Action onComplete = null)
    {
        this.onComplete = onComplete;
        seqIndex = 0;

        currentPlotSeqData = AllPlotSequence.dictData[plotSeqID];

        //如果延迟大于0，则等待延迟时间
        if (currentPlotSeqData.delay > 0)
        {
            await UniTask.Delay((int)(currentPlotSeqData.delay * 1000));
        }

        PlaySeq();
    }

    //播放剧情序列中的单个任务
    void PlaySeq()
    {
        PlotSequenceItem item = currentPlotSeqData.listSequenceTaskItem[seqIndex];
        //区分不同的类型，触发模块
        switch (item.seqItemType)
        {
            case PlotSequenceType.Plot:
                PlotSystem.Instance.OnTriggerPlot(item.seqItemName, OnNextSeq);
                break;
            case PlotSequenceType.Tut:
                TutSystem.Instance.OnTriggerTut(item.seqItemName, null, false, OnNextSeq);
                break;
            case PlotSequenceType.PageProfileID:
                ProfileSystem.Instance.OpenProfileID(OnNextSeq);
                break;
        }

    }

    //播放下一个任务
    void OnNextSeq()
    {
        seqIndex++;
        if (seqIndex < currentPlotSeqData.listSequenceTaskItem.Count)
        {
            PlaySeq();
        }
        else
        {
            OnCompleteSeq();
        }
    }

    //完成了剧情序列
    void OnCompleteSeq()
    {
        //ActingSystem.Instance.StopActing();
        //触发完成回调
        onComplete?.Invoke();
        onComplete = null;
    }
}
