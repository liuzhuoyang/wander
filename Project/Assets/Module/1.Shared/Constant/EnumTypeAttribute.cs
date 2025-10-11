
//值类型
public enum AttributeValueType
{
    Unknown,//未知
    VeryLow,//极低
    Low,//低
    Normal,//正常
    High,//高
    VeryHigh,//极高
}

//属性
//大量编辑器资源配置了此枚举，不能随便修改对应的数值
public enum AttributeType
{
    Damage = 0,//伤害
    CritRate = 1,//暴击率
    CritDamage = 2,//暴击伤害
    Health = 3,//血量
    Armor = 4,//护甲
    Speed = 5,//速度
    AttackRange = 6,//攻击距离
    AttackSpeed = 7,//攻击速度
    EffectRange = 8,//效果范围
    Shield = 9,//护盾值

    PhysicalResistance = 12,//物理抗性
    CryoResistance = 13,//冰冻抗性
    ElectroResistance = 14,//电磁抗性
    ThermalResistance = 15,//热能抗性
    BiochemicalResistance = 25,//生化抗性

    ArmorBreak = 24, //护甲破坏
    ArmorPenetration = 16, //护甲穿透

    MaxAmmo = 17, //弹药的最大值
    SummonLimit = 18,//召唤上限(只用于显示和建筑，如果launch要使用请使用GearAttributeType中的SummonLimit)
    Cure = 19,//治疗
    ShieldRecovery = 20,//护盾恢复
    ManaRecovery = 26,//法力恢复
    FinalDamage = 27,//最终伤害
    Invincible = 28,//无敌
    CritRateResist = 39, //抗暴击率
    CritDamageResist = 40, //抗暴击增伤

    #region 最大值修改，最大值修改需要另外计算
    MaxHealth = 21,
    MaxArmor = 22,
    MaxShield = 23,
    #endregion
}