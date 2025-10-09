using UnityEngine;

 public static class RelicFomular
 {
    #region 遗物解锁所需碎片
    public static int GetRelicUnlockNeedCount(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Rare => 20,
            Rarity.Epic => 30,
            Rarity.Legendary => 40,
            Rarity.Mythic => 50,
            Rarity.Arcane => 60,
            _ => 0
        };
    }
    #endregion

    #region 遗物升级所需碎片
    //返回(所需熵块数量,所需碎片数量)
    public static int GetRelicUpgradeNeedCount(int star, Rarity rarity)
    {
        int baseValue = 0;
        switch (rarity)
        {
            case Rarity.Rare: baseValue = 20; break;
            case Rarity.Epic: baseValue = 40; break;
            case Rarity.Legendary: baseValue = 60; break;
            case Rarity.Mythic: baseValue = 80; break;
            case Rarity.Arcane: baseValue = 160; break;
            default: return 0;
        }

        if (star >= 1 && star <= 4)
        {
            return baseValue * (int)Mathf.Pow(2, star - 1);
        }
        else if (star > 4)
        {
            return baseValue * 8 + (star - 4) * baseValue * 4;
        }
        else
        {
            return 0;
        }
    }
    #endregion

    #region 遗物属性加成
    public static float GetRelicAttributeAddition(string relicName, int star)
    {
        RelicData relicData = AllRelic.dictData[relicName];
        float ratio = relicData.rarity switch
        {
            Rarity.Rare => 1f,
            Rarity.Epic => 1.2f,
            Rarity.Legendary => 1.6f,
            Rarity.Mythic => 2f,
            Rarity.Arcane => 3f,
            _ => 1f
        };
        return relicData.value * star * ratio + star / 5 * relicData.value;
    }
    #endregion
 }
 