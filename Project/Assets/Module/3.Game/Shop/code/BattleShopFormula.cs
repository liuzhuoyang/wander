using UnityEngine;

public static class BattleShopFormula
{
     public static int GetGearCoin(int baseCoin,int level)
     {
        return baseCoin * level;
     }
}

