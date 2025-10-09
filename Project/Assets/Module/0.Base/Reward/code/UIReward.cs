using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class UIReward : UIBase
{
    public GameObject prefabSlot;
    public Transform container;

    public GameObject objBlock;
    public GameObject objBtnConfirm;
    //public GameObject objTextContinue;

    void Awake()
    {
        EventManager.StartListening<UIRewardArgs>(EventNameReward.EVENT_REWARD_OPEN_UI, OnOpen);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIRewardArgs>(EventNameReward.EVENT_REWARD_OPEN_UI, OnOpen);
    }

    async void OnOpen(UIRewardArgs args)
    {
        this.callbackClose = args.callbackClose;

        objBlock.SetActive(true);
        objBtnConfirm.SetActive(false);
        //objTextContinue.SetActive(false);

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        float delay = 0;
        if (args.listRewardArgs != null && args.listRewardArgs.Count > 0)
        {
            for (int i = 0; i < args.listRewardArgs.Count; i++)
            {
                GameObject go = Instantiate(prefabSlot, container);
                go.GetComponent<RewardViewSlot>().Init(args.listRewardArgs[i]);

                delay += 0.1f;
                if (AllItem.dictData[args.listRewardArgs[i].reward].rarity >= Rarity.Legendary)
                {
                    delay += 0.1f;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
                delay = 0f;
            }
        }
        else if (args.listRewardShowArgs != null && args.listRewardShowArgs.Count > 0)
        {
            for (int i = 0; i < args.listRewardShowArgs.Count; i++)
            {
                GameObject go = Instantiate(prefabSlot, container);
                go.GetComponent<RewardViewSlot>().Init(args.listRewardShowArgs[i]);

                delay += 0.1f;
                //高稀有度等待时间长
                if (AllItem.dictData[args.listRewardShowArgs[i].name].rarity >= Rarity.Legendary)
                {
                    delay += 0.1f;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
                delay = 0f;
            }
        }

        if (objBlock != null) objBlock.SetActive(false);
        if (objBtnConfirm != null) objBtnConfirm.SetActive(true);
    }

    public void OnClose()
    {
        base.CloseUI();
        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.OnRewardClosed });
    }
}
