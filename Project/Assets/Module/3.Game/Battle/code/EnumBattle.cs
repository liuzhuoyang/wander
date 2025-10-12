//目标类型
public enum TargetType
{
    Single,//单个
    Multi,//多目标
    Area,//区域
}

//伤害类型
public enum DamageType
{
    Physical,//物理
    Cryo,    //冰冻
    Electro, //电磁  
    Thermal, //热能
    Biochemical, //生化
    All = 999,//战斗技能不要配置，所有,Relic会对所有伤害类型加成
}

//索敌方式
public enum TargetingType
{
    Nearest,//最近
    Random,//随机
    Self,//自身
    Fixed,//固定
    MaxHealth,//最高生命值
}

//发射孢子类型
public enum LaunchSporeType
{
    Time,//时间
    LaunchCount,//发射次数
}

//影响效果类型
public enum StatusEffectType
{
    Buff,//增益
    Debuff,//减益
}

//持续类型
public enum BuffDurationType
{
    Permanent,//永久
    OverTime,//时间
}


public enum TrajectoryType
{
    Projectile,//直线
    Missile,//导弹路线
    Immediate,//立即
    Summon,//召唤
    Parabola,//抛射
    Laser,//激光
    Landmine,//地雷
    CircularSpread,//圆形扩散
    Castle,//自身
    MissilePos,//对地点导弹
    LaserSabre,//激光剑
    LaserFocus,//激光聚焦
    MovingLauncher,//移动发射器
    Sector,//扇形AOE
    WaveImpulse,//逐渐变大的冲击波
    Delay,//延时    
}

//装备发射属性
public enum GearAttributeType
{
    Trajectory,//轨迹
    AttackSpeed,//攻击速度
    Range,//范围
    LaunchCount,//发射数量
    LaunchPathNum,//发射路径数量
    Interval,//间隔
    SummonLimit,//召唤上限
    HealthBonus,//生命值加成
    Penetration,//穿透
    SectorMultiple,//扇形倍数
    ShieldBonus,//护盾加成
}
