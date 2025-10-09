using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class PlotSystem : Singleton<PlotSystem>
{
    PlotData currentPlotData;
    PlotItem currentPlotItem;
    int currentStepIndex;
    Action onComplete;

    bool isOnRewardStep; //标签记录是否进入奖励步骤
    public void Init()
    {

    }

    //触发剧情
    public void OnTriggerPlot(string plotName, Action onComplete = null)
    {
        isOnRewardStep = false; //初始化奖励步骤标签

        this.onComplete = onComplete;

        //保护 如果当前剧情与触发剧情相同，则不触发
        if (currentPlotData != null)
        {
            if (currentPlotData.plotName == plotName)
            {
                return;
            }
        }

        currentPlotData = AllPlot.dictData[plotName];
        Debug.Log("=== PlotSystem: Trigger Plot " + plotName + " ===");

        currentStepIndex = 0;

        OnOpenPlotUI();
    }

    async void OnOpenPlotUI()
    {
        //打开剧情UI
        await UIMain.Instance.OpenUI("plot", UIPageType.Overlay);
        EventManager.TriggerEvent<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_UI, new UIPlotArgs()
        {
            sceneType = currentPlotData.sceneType,
        });
        PlotUtil.InitPlotCustomStep(currentPlotData.plotName);
        OnNextStep();
    }

    //进入下一条目
    public void OnNextStep()
    {
        //如果已经进入奖励标签，再继续就直接结束
        //只有大厅对话且配置了剧情会进入奖励标签
        if (isOnRewardStep)
        {
            DonePlot();
            return;
        }

        currentStepIndex++;

        bool isDialogEnd = CheckIsDialogEnd();
        if (isDialogEnd)
        {
            if (!string.IsNullOrEmpty(currentPlotData.rewardName))
            {
                //如果有奖励，则展示奖励条目，不直接关闭
                OnRewardStep();
                return;
            }
            DonePlot();
            return;
        }

        currentPlotItem = currentPlotData.listPlotItem[currentStepIndex - 1];

        EventManager.TriggerEvent<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_NEXT_STEP_UI, new UIPlotArgs()
        {
            currentPlotItem = currentPlotItem,
            sceneType = currentPlotData.sceneType,
        });
    }

    //所有结束了展示奖励条目
    public void OnRewardStep()
    {
        isOnRewardStep = true; //设置奖励步骤标签
        EventManager.TriggerEvent<UIPlotLobbyRewardArgs>(PlotEventName.EVENT_ON_PLOT_LOBBY_REWARD_UI, new UIPlotLobbyRewardArgs()
        {
            rewardName = currentPlotData.rewardName,
            rewardNum = currentPlotData.rewardNum,
        });

        ItemSystem.Instance.GainItem(currentPlotData.rewardName, currentPlotData.rewardNum);
    }

    bool CheckIsDialogEnd()
    {
        if (currentStepIndex > currentPlotData.listPlotItem.Count)
        {
            return true;
        }
        return false;
    }

    public bool CheckIsPlotEnd()
    {
        if (!string.IsNullOrEmpty(currentPlotData.rewardName))
        {
            if (isOnRewardStep)
            {
                return true;
            }
            return false;
        }
        if (currentStepIndex >= currentPlotData.listPlotItem.Count)
        {
            return true;
        }
        return false;
    }

    public void OnSkipPlot()
    {
        if (currentPlotData.sceneType == PlotSceneType.BattleBubble)
        {
            return;
        }
        if (currentPlotData.sceneType == PlotSceneType.Battle)
        {
            DonePlot();
            return;
        }
        OnNextStep();
        /*kmj['p;;;;;;;;;/]
        currentStepIndex++;
        if (currentStepIndex > currentPlotArgs.listActionArgs.Count)
        {
            OnDonePlot();
            return;
        }

        currentPlotActionArgs = currentPlotArgs.listActionArgs[currentStepIndex - 1];
        if (currentPlotActionArgs.dialogType == PlotDialogType.Reward)
        {
            Debug.Log("=== PlotSystem:  通过剧情获得道具 " + currentPlotActionArgs.rewardArgs.reward + " " + currentPlotActionArgs.rewardArgs.num + " ===");
            ItemSystem.Instance.GainItem(currentPlotActionArgs.rewardArgs.reward, currentPlotActionArgs.rewardArgs.num);
        }*/
    }

    void DonePlot()
    {
        currentStepIndex = 0;
        Debug.Log("=== PlotSystem: Done Plot " + currentPlotData.plotName + " ===");

        EventManager.TriggerEvent<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_END_UI, new UIPlotArgs()
        {
            sceneType = currentPlotData.sceneType,
        });

        //触发完成回调
        onComplete?.Invoke();

        currentPlotData = null;
    }
}