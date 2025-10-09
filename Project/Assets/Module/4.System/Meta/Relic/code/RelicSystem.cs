using System;
using System.Collections.Generic;
using System.Linq;

/// 遗物系统
public class RelicSystem : Singleton<RelicSystem>
{
    UserRelic userRelic;
    UIRelicArgs uiRelicArgs;
    List<string> newRelicName;
    bool needSort;

    #region 初始化
    public void Init()
    {
        uiRelicArgs = new UIRelicArgs();
        uiRelicArgs.dictRelicSlotViewArgs = new Dictionary<Rarity, List<RelicSlotViewArgs>>();
        userRelic = GameData.userData.userRelic;
        newRelicName = new List<string>();
        gameObject.AddComponent<RelicPinHandler>().Init();

        var relicListArgs = AllRelic.dictData.Values.ToList();
        foreach (RelicData data in relicListArgs)
        {
            RelicSlotViewArgs args = new RelicSlotViewArgs()
            {
                relicData = data,
                count = 0,
                star = -1,
                isNew = false,
                needCount = 0,
            };
            //判断是否没有
            if (!userRelic.dictRelic.ContainsKey(data.name))
            {
                userRelic.dictRelic.Add(data.name, -1);
            }
            args.star = userRelic.dictRelic[data.name];

            if (args.star == -1)
            {
                //没有解锁
                args.needCount = RelicFomular.GetRelicUnlockNeedCount(data.rarity);
            }
            else
            {
                //有解锁
                args.needCount = RelicFomular.GetRelicUpgradeNeedCount(args.star + 1, data.rarity);
            }

            //更新ui层数据
            if (!uiRelicArgs.dictRelicSlotViewArgs.ContainsKey(data.rarity))
            {
                uiRelicArgs.dictRelicSlotViewArgs[data.rarity] = new List<RelicSlotViewArgs>();
            }
            uiRelicArgs.dictRelicSlotViewArgs[data.rarity].Add(args);
        }
        //品质排序
        uiRelicArgs.dictRelicSlotViewArgs = uiRelicArgs.dictRelicSlotViewArgs.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

    }
    #endregion

    #region 打开界面
    public async void Open()
    {
        await UIMain.Instance.OpenUI("relic", UIPageType.Normal);
        RefreshData();
        uiRelicArgs.needRefresh = true;
        EventManager.TriggerEvent<UIRelicArgs>(EventNameRelic.EVENT_RELIC_REFRESH_UI, uiRelicArgs);
        uiRelicArgs.needRefresh = false;
    }
    void RefreshData()
    {
        //判断碎片数量和是否新获得
        foreach (var item in uiRelicArgs.dictRelicSlotViewArgs)
        {
            foreach (RelicSlotViewArgs args in item.Value)
            {
                //判断碎片
                args.count = ItemSystem.Instance.GetItemNum("item_shard_" + args.relicData.name);
                //判断是否新获得
                args.isNew = newRelicName.Contains(args.relicData.name);
            }
            SortRelic(item.Value);
        }
    }

    void SortRelic(List<RelicSlotViewArgs> list)
    {
        list.Sort((a, b) =>
        {
            //解锁在前 未解锁在后
            if (a.star == -1 || b.star == -1)
            {
                //都未解锁根据碎片数量排序
                if (a.star == -1 && b.star == -1)
                {
                    return b.count.CompareTo(a.count);
                }
                int unlockComparison = b.star.CompareTo(a.star);
                if (unlockComparison != 0)
                {
                    return unlockComparison;
                }
            }
            //已解锁根据星级排序
            return b.star.CompareTo(a.star);
        });
    }
    #endregion


    #region 详细信息
    public void OnClickSlot(RelicSlotViewArgs args, Action onSuccess)
    {
        //判断是否可解锁
        if (args.star == -1 && args.count >= args.needCount)
        {
            //消耗材料
            ItemSystem.Instance.UseItem("item_shard_" + args.relicData.name, args.needCount, () =>
            {
                //更新玩家数据
                userRelic.dictRelic[args.relicData.name] = 1;
                //更新ui数据
                args.star = 1;
                args.count = ItemSystem.Instance.GetItemNum("item_shard_" + args.relicData.name);
                args.needCount = RelicFomular.GetRelicUpgradeNeedCount(args.star + 1, args.relicData.rarity);
                args.isNew = true;
                newRelicName.Add(args.relicData.name);
                RewardSystem.Instance.OnRewardDisplay(new List<RewardShowArgs>(){
                    new RewardShowArgs(){
                        name = "item_shard_" + args.relicData.name,
                        num = 0,
                    }
                });
                SortRelic(uiRelicArgs.dictRelicSlotViewArgs[args.relicData.rarity]);
                CheckGlobalBonusUpdate(args);
                EventManager.TriggerEvent(EventNameRelic.EVENT_RELIC_REFRESH_UI, uiRelicArgs);
                EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
                {
                    action = ActionType.RelicUnlock,
                });
            });
            return;
        }
        if (args.star != -1 && args.isNew)
        {
            args.isNew = false;
            newRelicName.Remove(args.relicData.name);
            onSuccess?.Invoke();
        }
        EventManager.TriggerEvent(EventNameRelic.EVENT_RELIC_REFRESH_INFO, args);
    }

    public void OnCloseInfo()
    {
        if (!needSort)
        {
            return;
        }
        foreach (var item in uiRelicArgs.dictRelicSlotViewArgs)
        {
            SortRelic(item.Value);
        }
        needSort = false;
        EventManager.TriggerEvent(EventNameRelic.EVENT_RELIC_REFRESH_UI, uiRelicArgs);
    }
    #endregion

    #region 升星
    public void OnClickStarUp(RelicSlotViewArgs slotArgs)
    {
        //解锁和最大星级保护
        if (slotArgs.star == -1 || slotArgs.star >= EventNameRelic.RELIC_STAR_MAX)
        {
            return;
        }
        ItemSystem.Instance.UseItem("item_shard_" + slotArgs.relicData.name, slotArgs.needCount, () =>
        {
            needSort = true;
            //更新用户数据
            userRelic.dictRelic[slotArgs.relicData.name] += 1;
            //更新ui数据
            slotArgs.star += 1;
            slotArgs.count = ItemSystem.Instance.GetItemNum("item_shard_" + slotArgs.relicData.name);
            slotArgs.needCount = RelicFomular.GetRelicUpgradeNeedCount(slotArgs.star + 1, slotArgs.relicData.rarity);
            CheckGlobalBonusUpdate(slotArgs);
            EventManager.TriggerEvent(EventNameRelic.EVENT_RELIC_REFRESH_STARUP, slotArgs);
            EventManager.TriggerEvent(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
            {
                action = ActionType.RelicUpgrade,
            });
        });
    }
    #endregion

    #region 奖励
    public RewardArgs OnRewardRelic(Rarity rarity, bool isShard)
    {
        if (uiRelicArgs.dictRelicSlotViewArgs.Count == 0)
        {
            return null;
        }
        return AddMineItem(rarity, isShard);
    }

    RewardArgs AddMineItem(Rarity rarity, bool isShard)
    {
        //判断该品质遗物
        if (!uiRelicArgs.dictRelicSlotViewArgs.TryGetValue(rarity, out var relicList) || relicList.Count == 0)
        {
            return null;
        }
        //判断是否有满足条件的遗物
        var availableRelics = new List<RelicSlotViewArgs>();
        foreach (var data in relicList)
        {
            if (data.star < EventNameRelic.RELIC_STAR_MAX)
            {
                availableRelics.Add(data);
            }
        }
        if (availableRelics.Count == 0)
        {
            return null;
        }
        //随机一个遗物
        var randomIndex = UnityEngine.Random.Range(0, availableRelics.Count);
        var relicData = availableRelics[randomIndex];
        RewardArgs rewardArgs = new RewardArgs();
        rewardArgs.reward = "item_shard_" + relicData.relicData.name;
        // rewardArgs.showName = relicData.relicData.name;
        rewardArgs.isShard = true;
        //如果是碎片+5 如果是遗物可解锁或转换为碎片
        if (isShard)
        {
            rewardArgs.num = 5;
        }
        else
        {
            rewardArgs.num = rarity switch
            {
                Rarity.Rare => 20,
                Rarity.Epic => 30,
                Rarity.Legendary => 40,
                Rarity.Mythic => 50,
                Rarity.Arcane => 60,
                _ => 0
            };
            // }
        }

        return rewardArgs;
    }
    #endregion

    #region 获取遗物加成
    public void GetRelicBonus()
    {
        foreach (var item in uiRelicArgs.dictRelicSlotViewArgs)
        {
            foreach (var data in item.Value)
            {
                if (data.star == -1) continue;

                float value = RelicFomular.GetRelicAttributeAddition(data.relicData.relicName, data.star);

                //根据实际游戏情况做拓展
            }
        }
    }

    #endregion

    #region 判断全局加成更新数据
    void CheckGlobalBonusUpdate(RelicSlotViewArgs args)
    {

    }
    public float GetRelicGoldBonus()
    {
        float result = 0f;
        foreach (var item in uiRelicArgs.dictRelicSlotViewArgs)
        {
            foreach (var data in item.Value)
            {
                if (data.star == -1) continue;
                if (data.relicData.passiveType != RelicPassiveType.BattleToken) continue;

                float value = RelicFomular.GetRelicAttributeAddition(data.relicData.relicName, data.star);
                result += value;
            }
        }
        return result;
    }
    #endregion

    #region 调试
    public void OnDebugGetRelic()
    {
        List<RewardArgs> listReward = new List<RewardArgs>();
        foreach (var item in AllRelic.dictData)
        {
            listReward.Add(new RewardArgs()
            {
                reward = $"item_shard_{item.Value.relicName}",
                num = 100,
            });
        }
        RewardSystem.Instance.OnReward(listReward);
    }
    #endregion
}