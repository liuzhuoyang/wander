using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using SimpleVFXSystem;

public class RewardSystem : Singleton<RewardSystem>
{
    public void Init()
    {

    }

    public void Open()
    {

    }

    //获取物品
    public async void OnReward(List<RewardArgs> listRewardArgs, RewardViewType rewardViewType = RewardViewType.Overlay, Action callback = null)
    {
        foreach (RewardArgs args in listRewardArgs)
        {
            ItemSystem.Instance.GainItem(args.reward, args.num);
        }

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.ItemReward });

        if (rewardViewType == RewardViewType.Overlay)
        {
            await UIMain.Instance.OpenUI("reward", UIPageType.Overlay);

            EventManager.TriggerEvent<UIRewardArgs>(EventNameReward.EVENT_REWARD_OPEN_UI, new UIRewardArgs
            {
                listRewardArgs = listRewardArgs,
                callbackClose = ()=>
                {
                    // VFXControl.Instance.OnVFXFlayerBatch(listRewardArgs);
                    callback?.Invoke();
                }
            });
        }

        if (rewardViewType == RewardViewType.Flyer)
        {
            VFXManager.Instance.OnVFXFlayerBatchUI(listRewardArgs);
        }
    }

    //只显示 不获取实际物品
    public async void OnRewardDisplay(List<RewardShowArgs> listRewardShowArgs, RewardViewType rewardViewType = RewardViewType.Overlay, Action callback = null)
    {
        await UIMain.Instance.OpenUI("reward", UIPageType.Overlay);

        EventManager.TriggerEvent<UIRewardArgs>(EventNameReward.EVENT_REWARD_OPEN_UI, new UIRewardArgs
        {
            listRewardShowArgs = listRewardShowArgs,
            callbackClose = callback
        });
    }

/*
    //解析字符串
    public List<RewardArgs> ConverRewardStreamToRewardArgsList(List<string> listRewardArgs)
    {
        List<RewardArgs> listReward = new List<RewardArgs>();
        for (int i = 0; i < listRewardArgs.Count; i++)
        {
            RewardArgs rewardArgs = ConvertRewardStreamToArgs(listRewardArgs[i]);
            listReward.Add(rewardArgs);
        }
        return listReward;
    }

    /// <summary>
    /// rewardName ^ num 的形式
    /// </summary>
    /// <param name="reward"></param>
    /// <returns></returns>
    public RewardArgs ConvertRewardStreamToArgs(string args)
    {
        string[] nameNumArgs = args.Split("^");
        RewardArgs rewardArgs = new RewardArgs()
        {
            reward = nameNumArgs[0],
            num = int.Parse(nameNumArgs[1])
        };
        return rewardArgs;
    }
    */
}
