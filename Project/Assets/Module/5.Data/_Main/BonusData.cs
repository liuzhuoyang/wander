using System.Collections.Generic;

/// <summary>
/// 全局加成数据
/// </summary>
public static class BonusData
{
    //战斗开始token
    public static float battleStartTokenBonus;
    //武器攻击力加成
    public static float gearDamageBonus;
    //基地血量加成
    public static float baseHealthBonus;
    //基地护盾加成
    public static float baseShieldBonus;
    //技能强度加成
    public static float skillBonus;
    //对boss伤害加成
    public static float bossDamageBonus;
    //高级技能出现概率
    public static float advancedSkillRate;
    //高级装备出现概率
    public static float advancedGearRate;
    //伤害类型加成
    public static Dictionary<DamageType, BattleDamageTypeBonus> battleDamageTypeBonus;

    public static void InitGlobalBattleBonus()
    {
        battleStartTokenBonus = 0f;
        gearDamageBonus = 0f;
        baseHealthBonus = 0f;
        baseShieldBonus = 0f;
        skillBonus = 0f;
        bossDamageBonus = 0f;
        advancedSkillRate = 0f;
        advancedGearRate = 0f;
        battleDamageTypeBonus = new Dictionary<DamageType, BattleDamageTypeBonus>
        {
            { DamageType.Physical, new BattleDamageTypeBonus() },
            { DamageType.Cryo, new BattleDamageTypeBonus() },
            { DamageType.Electro, new BattleDamageTypeBonus() },
            { DamageType.Thermal, new BattleDamageTypeBonus() },
            { DamageType.Biochemical, new BattleDamageTypeBonus() }
        };
    }


    public static void InitGlobalBonus()
    {
        GetEnergyLimitBonus();

        GetTavernCountBonus();

        GetGoldBonus();
    }
    //体力上限加成
    public static float energyLimitBonus;
    public static void GetEnergyLimitBonus()
    {
        energyLimitBonus = 0f;
        //energyLimitBonus += TalentSystem.Instance.GetEnergyLimitBonus();
    }
    //酒馆碎片数量加成
    public static float tavernCountBonus;
    public static void GetTavernCountBonus()
    {
        tavernCountBonus = 0f;
        //tavernCountBonus += TalentSystem.Instance.GetTavernCountBonus();
    }
    //全局金币获取额
    public static float goldBonus;
    public static void GetGoldBonus()
    {
        goldBonus = 0f;
       // goldBonus += TalentSystem.Instance.GetCoinTokenBonus();
        //goldBonus += RelicSystem.Instance.GetRelicGoldBonus();
    }
}

//战斗加成数据
public class BattleDamageTypeBonus
{
    public float damageBonus;//伤害
    public float critRateBonus;//暴击率
    public float critDamageBonus;//暴击伤害
    public float effectRangeBonus;//效果范围
    public float attackSpeedBonus;//攻击速度
    public float attackRangeBonus;//攻击距离
}