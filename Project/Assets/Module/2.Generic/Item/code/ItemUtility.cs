using UnityEngine;
using System.Collections.Generic;

public static class ItemUtility
{
    public static string GetRarityFrameName(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return "ui_item_dynamic_slot_01_common";
            case Rarity.Rare:
                return "ui_item_dynamic_slot_02_rare";
            case Rarity.Epic:
                return "ui_item_dynamic_slot_03_epic";
            case Rarity.Legendary:
                return "ui_item_dynamic_slot_04_legendary";
            case Rarity.Mythic:
                return "ui_item_dynamic_slot_05_mythic";
            case Rarity.Arcane:
                return "ui_item_dynamic_slot_06_arcane";
            default:
                return "ui_shared_dynamic_slot_item_01_common";
        }
    }

    //更新自定义过期时间的物品
    public static void OnUpdateItemCustomExpiry()
    {
        Dictionary<string, int> dictItemCustomExpiry = GameData.userData.userItem.dictItemCustomExpiry;
        //获取是否过了一天
        bool isNewDay = GameData.userData.userMisc.isNewDayLogin;
        if (!isNewDay)
        {
            return;
        }
        if (dictItemCustomExpiry.Count == 0)
        {
            return;
        }
        //获取距离上次保存时间的天数
        int lastTime = TimeManager.Instance.GetZeroTimeSpanOfDay(GameData.userData.userAccount.saveTime) + 24 * 60 * 60;
        //判断中间天数
        int days = ((int)TimeManager.Instance.GetCurrentTimeSpan() - lastTime) / (24 * 60 * 60) + 1;
        List<string> removeList = new List<string>();
        List<string> updateKeys = new List<string>();

        foreach (var item in dictItemCustomExpiry)
        {
            int newValue = item.Value - days;
            if (newValue <= 0)
            {
                removeList.Add(item.Key); // 过期
            }
            else
            {
                updateKeys.Add(item.Key); // 需要更新的
            }
        }

        // 统一更新
        foreach (var key in updateKeys)
        {
            dictItemCustomExpiry[key] -= days;
        }

        // 统一删除
        foreach (var key in removeList)
        {
            dictItemCustomExpiry.Remove(key);
            //判断道具
            ItemSystem.Instance.EmptyItem(key);
        }
    }

    //添加物品到过期列表
    public static void OnAddItemCustomExpiry(string itemName)
    {
        if (!AllItem.dictData.ContainsKey(itemName))
        {
            return;
        }
        var itemData = AllItem.dictData[itemName];
        if (!itemData.isCustomResetTime)
        {
            return;
        }
        GameData.userData.userItem.dictItemCustomExpiry.TryAdd(itemName, itemData.customResetTime);
        ItemSystem.Instance.GainItem(itemName, 1);
    }

    //判断是否过期
    public static bool CheckIsItemExpiration(string itemName)
    {
        if (GameData.userData.userItem.dictItemCustomExpiry.ContainsKey(itemName))
        {
            return GameData.userData.userItem.dictItemCustomExpiry[itemName] <= 0;
        }
        return true;
    }

    //获取道具剩余天数
    public static int GetItemExpiration(string itemName)
    {
        if (GameData.userData.userItem.dictItemCustomExpiry.ContainsKey(itemName))
        {
            return GameData.userData.userItem.dictItemCustomExpiry[itemName];
        }
        return 0;
    }
}