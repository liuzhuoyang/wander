using System.Collections.Generic;
using System;

public class UserItem
{
    public Dictionary<string, int> userItemDict = new Dictionary<string, int>();

    public Dictionary<string, int> dictItemCustomExpiry; //自定义过期时间的物品，会被加入到这里，每天减少1

    public UserItem()
    {
        //检查Item是否存在，有可能后面删除的Item没有清空
        CheckItemValidation();
    }

    public void OnResetDaily()
    {
        ResetItemsByTimeType(TimeResetType.Daily, "Daily");
    }

    public void OnResetWeekly()
    {
        ResetItemsByTimeType(TimeResetType.Weekly, "Weekly");
    }

    public void OnResetMonthly()
    {
        ResetItemsByTimeType(TimeResetType.Monthly, "Monthly");
    }

    //删除物品用自然日，周，月来定义删除的物品，比如自然月的月卡，赛季卡等，过了自然月必定要删除
    private void ResetItemsByTimeType(TimeResetType resetType, string logPrefix)
    {
        Dictionary<string, int> userItemDict = GameData.userData.userItem.userItemDict;
        List<string> removeList = new List<string>();
        
        foreach (string key in userItemDict.Keys)
        {
            if (AllItem.dictData[key].timeResetType == resetType)
            {
                removeList.Add(key);
                UnityEngine.Debug.Log($" === ItemSystem: Reset {logPrefix}: Item expired: {key} ===");
            }
        }

        foreach (string key in removeList)
        {
            userItemDict.Remove(key);
        }
    }

    //查询时发现用户数据里存在的数据，但游戏中已经没有，删除用户的数据，加入到列表
    List<string> invalidItemList = new List<string>();
    void CheckItemValidation()
    {
        foreach (string key in userItemDict.Keys)
        {
            if (!AllItem.dictData.ContainsKey(key))
            {
                invalidItemList.Add(key);
            }
        }

        foreach (string key in invalidItemList)
        {
            userItemDict.Remove(key);
        }
    }
}

