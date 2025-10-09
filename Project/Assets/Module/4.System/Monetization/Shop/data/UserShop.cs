using System.Collections.Generic;
using System.Linq;

public class UserShop
{
    public Dictionary<string, UserShopSlotData> dictShopSlot;
    public void InitData()
    {
        dictShopSlot = new Dictionary<string, UserShopSlotData>();
    }

    public void OnResetDaily()
    {
        //删除数据中重置类型为每日的数据
        var keysToRemove = dictShopSlot.Where(x => x.Value.resetType == (int)TimeResetType.Daily)
                                     .Select(x => x.Key)
                                     .ToList();
        foreach (var key in keysToRemove)
        {
            dictShopSlot.Remove(key);
        }
    }

    public void OnResetWeekly()
    {
        //删除数据中重置类型为每周的数据
        var keysToRemove = dictShopSlot.Where(x => x.Value.resetType == (int)TimeResetType.Weekly)
                                     .Select(x => x.Key)
                                     .ToList();
        foreach (var key in keysToRemove)
        {
            dictShopSlot.Remove(key);
        }
    }

    public void OnResetMonthly()
    {
        //删除数据中重置类型为每月的数据
        var keysToRemove = dictShopSlot.Where(x => x.Value.resetType == (int)TimeResetType.Monthly)
                                     .Select(x => x.Key)
                                     .ToList();
        foreach (var key in keysToRemove)
        {
            dictShopSlot.Remove(key);
        }
    }
}

public class UserShopSlotData
{
    public int buyCount;
    public int resetType;
}
