using System.Collections.Generic;
using System.Linq;

#region User Promo
public class UserPromo
{
    public Dictionary<string, UserPromoData> dictPromoData; // 常规礼包 礼包名 - 礼包数据
    public Dictionary<string, UserPromoEndlessData> dictPromoEndlessData; // 无尽礼包 礼包名 - 礼包数据
    public Dictionary<string, UserPromoGearData> dictPromoGearData; // 装备礼包 礼包名 - 礼包数据
    public Dictionary<string, UserPromoStarterData> dictPromoStarterData; // 新手礼包 礼包名 - 礼包数据
    public Dictionary<string, string> dictPromoCustom; // 自定义道具名 - 礼包名
    public List<string> listPromoRemove; // 已经结束的礼包

    public void InitData()
    {
        dictPromoData = new Dictionary<string, UserPromoData>();
        dictPromoEndlessData = new Dictionary<string, UserPromoEndlessData>();
        dictPromoGearData = new Dictionary<string, UserPromoGearData>();
        dictPromoStarterData = new Dictionary<string, UserPromoStarterData>();
        dictPromoCustom = new Dictionary<string, string>();
        listPromoRemove = new List<string>();
    }

    public T GetPromoData<T>(PromoType promoType, string promoName) where T : UserPromoData
    {
        return promoType switch
        {
            PromoType.Endless => dictPromoEndlessData.TryGetValue(promoName, out var endlessData) ? endlessData as T : null,
            PromoType.Gear => dictPromoGearData.TryGetValue(promoName, out var gearData) ? gearData as T : null,
            PromoType.Starter => dictPromoStarterData.TryGetValue(promoName, out var starterData) ? starterData as T : null,
            _ => dictPromoData.TryGetValue(promoName, out var promoData) ? promoData as T : null,
        };
    }

    public void ResetDaily()
    {
        var listRemove = dictPromoEndlessData
            .Where(promo => promo.Value.resetType == (int)TimeResetType.Daily)
            .Select(promo => promo.Key)
            .ToList();

        foreach (var promo in listRemove)
        {
            dictPromoEndlessData.Remove(promo);
        }

        foreach (var promo in dictPromoStarterData.Values)
        {
            ++promo.canTakeDay;
        }
    }

    public void OnAddNeedRemovePromo(string promoName)
    {
        if (listPromoRemove.Contains(promoName))
        {
            return;
        }
        listPromoRemove.Add(promoName);
    }

    public void OnRemovePromo()
    {
        foreach (var promo in listPromoRemove)
        {
            if (dictPromoData.ContainsKey(promo))
            {
                dictPromoData.Remove(promo);
            }
            if (dictPromoEndlessData.ContainsKey(promo))
            {
                dictPromoEndlessData.Remove(promo);
            }
            if (dictPromoGearData.ContainsKey(promo))
            {
                dictPromoGearData.Remove(promo);
            }
            if (dictPromoStarterData.ContainsKey(promo))
            {
                dictPromoStarterData.Remove(promo);
            }
        }
    }
}

public class UserPromoData
{
    public int popupCount;//弹窗次数
    public int buyCount;//购买次数
    public int endTime;//结束时间
    public bool isReset;//是否重置
    public int resetType;//重置类型
}

public class UserPromoStarterData : UserPromoData
{
    public int canTakeDay;//可领取天数
    public int takeDay;//已领取天数
}

public class UserPromoEndlessData : UserPromoData
{
    public int buyIndex;//购买索引
}

public class UserPromoGearData : UserPromoData
{
    public string rewardItemName;//自定义奖励名
}
#endregion

