using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;

//游戏进入大厅，回到大厅时候触发的一些列模块，
//因为可能存在多个需要顺序触发的功能，所以需要一个系统来管理
//其他地方不需要用到这个系统
public class SequenceTaskSystem : Singleton<SequenceTaskSystem>
{
    //bool isTriggerOn;
    public void Init()
    {
        /*
        //listSeq = new List<SequenceTaskArgs>();
        foreach (var plotSeq in GameData.allPlotSequence.dictPlotSeqData)
        {
            if (plotSeq.Value.isDeleteOnRelease)
            {
                GameData.userData.userSequenceTask.listSeqTask.RemoveAll(x => x.plotSeqID == plotSeq.Key);
            }
        }*/
    }

    private void OnDestroy()
    {

    }

    public void AddPlotSeq(string plotSeqID)
    {
        Debug.Log("=== SequenceTaskSystem: AddPlotSeq: " + plotSeqID + " ===");
        if (AllPlotSequence.dictData.ContainsKey(plotSeqID))
        {
            GameData.userData.userSequenceTask.OnAddSeqTask(new SequenceTaskArgs()
            {
                seqTaskType = SequenceTaskType.PlotSequence,
                plotSeqID = plotSeqID,
            });
        }
    }

    public void AddPlot(string plotSeqID)
    {
        Debug.Log("=== SequenceTaskSystem: AddPlot: " + plotSeqID + " ===");
        GameData.userData.userSequenceTask.OnAddSeqTask(new SequenceTaskArgs()
        {
            seqTaskType = SequenceTaskType.Plot,
            plotSeqID = plotSeqID,
        });
    }

    public void AddFeatureSeq(FeatureType featureType)
    {
        Debug.Log("=== SequenceTaskSystem: AddFeatureSeq: " + featureType + " ===");
        GameData.userData.userSequenceTask.OnAddSeqTask(new SequenceTaskArgs()
        {
            seqTaskType = SequenceTaskType.Feature,
            unlockFeatureType = featureType,
        });
    }

    public void AddPopupSeq(string popupName)
    {
        if (GameData.userData.userSequenceTask.listSeqTask.Any(x => x.popupName == popupName))
        {
            return;
        }
        Debug.Log("=== SequenceTaskSystem: OnAddPopup: " + " ===");
        GameData.userData.userSequenceTask.OnAddSeqTask(new SequenceTaskArgs()
        {
            seqTaskType = SequenceTaskType.Popup,
            popupName = popupName,
        });
    }

    public async void OnStarTriggerSeq()
    {
        List<SequenceTaskArgs> listSeq = GameData.userData.userSequenceTask.listSeqTask;
        if (listSeq.Count <= 0) return;

        Debug.Log("=== SequenceTaskSystem: 存在序列，进入序列播放, OnStarTriggerSeq: " + listSeq.Count + " ===");
        //isTriggerOn = true;
        ActingSystem.Instance.OnActing(this.name);
        int delayTime = GameData.userData.userSequenceTask.listSeqTask[0].seqTaskType == SequenceTaskType.Feature ? 3000 : 1000;
        OnTriggerSeq();

        await UniTask.Delay(delayTime);

        ActingSystem.Instance.StopActing(this.name);
    }

    //序列播放完成
    void OnDoneTriggerSeq()
    {
        //发送事件进行保存
        EventManager.TriggerEvent(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
        {
            action = ActionType.OnSeqTaskComplete,
        });
    }

    void OnTriggerSeq()
    {
        List<SequenceTaskArgs> listSeq = GameData.userData.userSequenceTask.listSeqTask;

        Debug.Log("=== SequenceTaskSystem: OnTriggerSeq: " + listSeq.Count + " ===");
        //能进这里，必定是现有序列，没有序列前面OnStarTriggerSeq就返回了，这里条目小于0，说明后面递归完成播放了。这里跳出就要进入OnDoneTriggerSeq
        if (listSeq.Count <= 0)
        {
            // OnDoneTriggerSeq();
            return;
        }

        SequenceTaskArgs seq = listSeq[0];
        Debug.Log($"=== SequenceTaskSystem: OnTriggerSeq:  {seq.seqTaskType} _ {seq.plotSeqID}  ===");

        switch (seq.seqTaskType)
        {
            case SequenceTaskType.PlotSequence:
                //PlotSystem.Instance.OnTriggerPlot(seq.plotSeqData.plotSeqID, null, true);
                PlotSequenceSystem.Instance.OnStartPlotSeq(seq.plotSeqID, OnTriggerSeq);//递归调用，播放下一个任务
                break;
            case SequenceTaskType.Plot:
                PlotSystem.Instance.OnTriggerPlot(seq.plotSeqID, OnTriggerSeq);
                break;
            case SequenceTaskType.Function:
                //UIMain.Instance.OpenUI(seq.pageName, UIPageType.Normal);
                break;
            case SequenceTaskType.Popup:
                PopupManager.Instance.OnPopup(new PopupArgs() { popupName = seq.popupName, callback = OnTriggerSeq });
                break;
            case SequenceTaskType.Feature:
                FeatureSystem.Instance.OnDisplayFeatureUnlock(seq.unlockFeatureType, OnTriggerSeq);
                break;
        }

        GameData.userData.userSequenceTask.listSeqTask.RemoveAt(0);
        OnDoneTriggerSeq();
    }
}
