using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonSystem : Singleton<DungeonSystem>
{
    DungeonData currentDungeon;
    UserDungeon userDungeon;

    public void Init()
    {
        userDungeon = GameData.userData.userDungeon;
    }

    public void OnPlayDungeon(DungeonData dungeonData,Action callback)
    {
        if (ItemSystem.Instance.GetItemNum(dungeonData.costItemName) <= 0)
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/tip_lack_item"));
            return;
        }

        currentDungeon = dungeonData;
        callback?.Invoke();
    }

    public void OnPassDungeon()
    {
        userDungeon.dictDungeonLevel[currentDungeon.dungeonName]++;
    }

    public List<RewardArgs> GetDungeonReward(string dungeonName, int dungeonLevel)
    {
        switch (dungeonName)
        {
            case "dungeon_01":
                return new List<RewardArgs>()
                {
                    new RewardArgs() { reward = ConstantItem.ENERGY, num = 10 + Mathf.CeilToInt((dungeonLevel - 1) / 2) },
                    new RewardArgs() { reward = ConstantItem.GEM, num = 10 + Mathf.CeilToInt(dungeonLevel  / 2 - 1) },
                };
            case "dungeon_02":
                return new List<RewardArgs>()
                {
                    new RewardArgs() { reward = ConstantItem.GEM, num = dungeonLevel },
                };
            default:
                return new List<RewardArgs>();
        }
    }

    #region 扫荡
    public void OnSweepDungeon(DungeonData dungeonData,Action callback)
    {
        if (dungeonData.isLocked)
        {
            return;
        }
        if (GameData.userData.userDungeon.dictDungeonLevel[dungeonData.dungeonName] <= 1)
        {
            return;
        }
        ItemSystem.Instance.UseItem(dungeonData.costItemName, 1, () =>
        {
            RewardSystem.Instance.OnReward(GetDungeonReward(dungeonData.dungeonName, userDungeon.dictDungeonLevel[dungeonData.dungeonName] - 1));
            callback?.Invoke();
        });
    }
    #endregion
}
