using UnityEngine;

public static class GearFormula
{
    public static int GetGearAttack(int baseAttack, int level)
    {
        return baseAttack + (int)(baseAttack * 0.1f * level);
    }

    public static int GetGearNeedCardCount(int level, Rarity rarity)
    {
        int baseValue = 20;
        baseValue = rarity switch
        {
            Rarity.Common => 20,
            Rarity.Rare => 20,
            Rarity.Epic => 20,
            Rarity.Legendary => 20,
            Rarity.Mythic => 20,
            Rarity.Arcane => 20,
            _ => 20
        };
        return baseValue * (int)Mathf.Pow(2, level - 1);
    }

    public static int GetGearCoin(int level)
    {
        return 100 * level;
    }

    public static string GetGearCard(string gearName)
    {
        return "item_shard_gear_" + gearName;
    }

    public static string GetGearIconName(string gearName)
    {
        return "gear_" + gearName;
    }
}
