using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class BattleFormula
{
    public static int GetInGameLevelExpNeeded(int level)
    {
        return (int)(600 * Mathf.Log10(level) + 1500);
        // 改为合成给经验后 固定为10
        // return 10;
    }

    public static int GetMaxInGameLevel()
    {
        return 999;
    }

    //根据等级，获取局内Actor的数值成长
    public static float GetUnitAttributeByLevel(Vector2 minmaxValue, int level, int maxLevel) => minmaxValue.x + (minmaxValue.y - minmaxValue.x) * Mathf.Pow((level - 1f) / (maxLevel - 1f), 1.6f);

    //获取基地血量
    public static float GetBaseMaxHealth()
    {
        //TODO
        return 1000;
    }

    //获取基地护盾
    public static float GetBaseMaxShield()
    {
        //TODO
        return 500;
    }

    #region 特殊刷新

    #endregion
}
