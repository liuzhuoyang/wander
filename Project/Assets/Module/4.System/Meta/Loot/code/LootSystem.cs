using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LootSystem : Singleton<LootSystem>
{
    UILootArgs uiLootArgs;
    bool isJump = false;
    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("loot", "home");
        LootUtility.InitData();

        uiLootArgs = new UILootArgs();
        //宝箱锁
        uiLootArgs.chestLockArgs = LootUtility.GetChestLockArgs(GameData.userData.userLoot.lockIndex);
        uiLootArgs.listCount = new List<int>();
    }

    public void Open(bool isJump = false)
    {
        this.isJump = isJump;

        ModeHomeControl.OnOpen("loot");
        HeaderControl.OnShowMainHideHub(new List<string>() { "gem" });
        FooterControl.OnSelect("loot");
        uiLootArgs.listCount.Clear();
        for (int i = 0; i < 5; i++)
        {
            uiLootArgs.listCount.Add(ItemSystem.Instance.GetItemNum("item_loot_box_" + (i + 1)));
        }
        uiLootArgs.chooseIndex = 0;
        RefreshUI(false);
        EventManager.TriggerEvent<UILootArgs>(EventNameLoot.EVENT_LOOT_INIT, uiLootArgs);
    }

    void RefreshUI(bool needEvent = true)
    {
        uiLootArgs.chooseRarity = AllItem.dictData["item_loot_box_" + (uiLootArgs.chooseIndex + 1)].rarity;
        //可开宝箱数量
        CheckCount();
        if (needEvent)
        {
            EventManager.TriggerEvent<UILootArgs>(EventNameLoot.EVENT_LOOT_REFRESH_UI, uiLootArgs);
        }
    }

    void CheckCount()
    {
        int count = uiLootArgs.listCount[uiLootArgs.chooseIndex];
        if (count <= 10)
        {
            uiLootArgs.canOpenCount = count;
        }
        else
        {
            uiLootArgs.canOpenCount = 10;
        }
    }

    public void OnClickSlot(int index)
    {
        if (index == uiLootArgs.chooseIndex) return;
        uiLootArgs.chooseIndex = index;
        RefreshUI();
    }

    public void OnClickBtnLeft()
    {
        if (uiLootArgs.chooseIndex <= 0) return;
        uiLootArgs.chooseIndex--;
        RefreshUI();
    }

    public void OnClickBtnRight()
    {
        if (uiLootArgs.chooseIndex >= uiLootArgs.listCount.Count - 1) return;
        uiLootArgs.chooseIndex++;
        RefreshUI();
    }

    public void OnClickBtnClaim()
    {
        if (uiLootArgs.canOpenCount <= 0 || uiLootArgs.listCount[uiLootArgs.chooseIndex] < uiLootArgs.canOpenCount) return;
        //消耗数量
        string itemName = "item_loot_box_" + (uiLootArgs.chooseIndex + 1);
        int useNum = uiLootArgs.canOpenCount;
        ItemSystem.Instance.UseItem(itemName, useNum, () =>
        {
            //奖励
            OnReward();
            uiLootArgs.listCount[uiLootArgs.chooseIndex] -= useNum;
            //增加点数
            GameData.userData.userLoot.point += LootUtility.GetChestPoint(uiLootArgs.chooseIndex) * useNum;
            //刷新UI
            CheckCount();
            EventManager.TriggerEvent<UILootArgs>(EventNameLoot.EVENT_LOOT_CLAIM_REFRESH, uiLootArgs);
            EventManager.TriggerEvent(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
            {
                action = ActionType.ItemReward,
            });
        });
    }

    public void OnClickChestLock()
    {
        if (uiLootArgs.chestLockArgs.needPoint > GameData.userData.userLoot.point) return;
        List<int> listRewardCount = new List<int>() { 0, 0, 0, 0, 0 };
        while (GameData.userData.userLoot.point >= uiLootArgs.chestLockArgs.needPoint)
        {
            //消耗点数
            GameData.userData.userLoot.point -= uiLootArgs.chestLockArgs.needPoint;
            //增加宝箱数量
            listRewardCount[uiLootArgs.chestLockArgs.chestIndex]++;
            //下一个宝箱锁  
            var data = LootUtility.GetNextChestLock(GameData.userData.userLoot.lockIndex);
            uiLootArgs.chestLockArgs = data.Item1;
            GameData.userData.userLoot.lockIndex = data.Item2;
        }
        //增加宝箱数量
        List<RewardArgs> listReward = new List<RewardArgs>();
        for (int i = 0; i < listRewardCount.Count; i++)
        {
            string itemName = "item_loot_box_" + (i + 1);
            if (listRewardCount[i] <= 0) continue;
            listReward.Add(new RewardArgs() { reward = itemName, num = listRewardCount[i] });
            uiLootArgs.listCount[i] += listRewardCount[i];
        }
        RewardSystem.Instance.OnReward(listReward);
        CheckCount();
        EventManager.TriggerEvent<UILootArgs>(EventNameLoot.EVENT_LOOT_CLAIM_REFRESH, uiLootArgs);
        EventManager.TriggerEvent(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
        {
            action = ActionType.ItemReward,
        });
    }

    public void OnClose()
    {
        if (!isJump)
        {
            return;
        }
        isJump = false;
        //RelicSystem.Instance.Open();
    }

    void OnReward()
    {
        List<RewardArgs> listReward = new List<RewardArgs>();

        for (int i = 0; i < uiLootArgs.canOpenCount; ++i)
        {
            // 概率奖励 随机遗物
            float allPro = 0;
            float randomValue = Random.Range(0f, 1f);
            foreach (var item in LootUtility.GetChestProbabilityArgs(uiLootArgs.chooseIndex + 1))
            {
                allPro += item.probability;
                if (randomValue <= allPro)
                {
                    /* 改用Item
                    var reward = RelicSystem.Instance.OnRewardRelic(item.rarity, item.isShard);
                    if (reward != null)
                    {
                        listReward.Add(reward);
                    }
                    */
                    break;
                }
            }
        }
        RewardSystem.Instance.OnReward(listReward);
    }

    // void ProcessReward(ChestItemArgs itemArgs, List<RewardArgs> listReward)
    // {
    //     float allPro = 0;
    //     float randomPro = Random.Range(0f, 1f);
    //     foreach (var item in itemArgs.listItemProArgs)
    //     {
    //         allPro += item.probability;
    //         if (randomPro <= allPro)
    //         {
    //             RewardArgs rewardArgs = new RewardArgs()
    //             {
    //                 reward = itemArgs.itemName,
    //                 num = item.listItemNum[Random.Range(0, item.listItemNum.Count)],
    //                 isShard = false
    //             };
    //             listReward.Add(rewardArgs);
    //             return;
    //         }
    //     }
    // }

    #region debug
    public void OnDebugGetLoot(int index)
    {
        RewardSystem.Instance.OnReward(new List<RewardArgs>()
        {
            new RewardArgs(){
                reward = "item_loot_box_" + index,
                num = 100
            }
        });
        uiLootArgs.listCount.Clear();
        for (int i = 0; i < 5; i++)
        {
            uiLootArgs.listCount.Add(ItemSystem.Instance.GetItemNum("item_loot_box_" + (i + 1)));
        }
        uiLootArgs.chooseIndex = 0;
        RefreshUI(false);
        EventManager.TriggerEvent<UILootArgs>(EventNameLoot.EVENT_LOOT_INIT, uiLootArgs);
    }
    #endregion
}
