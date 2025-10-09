
public enum AccountType
{
    Facebook,
    Apple,
    Google
}

public enum UserAcquisitionType
{
    None,
    Ogranic,
    AD
}

public enum SpeedType
{
    x1,
    x2,
    x3
}


public enum TextColorType
{
    Title,
    TextLight,
}

public enum Rarity
{
    Common = 1,
    Rare = 2,
    Epic = 3,
    Legendary = 4,
    Mythic = 5,
    Arcane = 6,
    None = 0,
}

public enum UnlockConditionType
{
    Chapter,
    Event
}

public enum CostType
{
    Item,
    IAP,
    Free,
    AD
}

//值类型
public enum ValueType
{
    Fixed,//固定值
    Rate,//百分比
}

//不同的类型拥有不同的刷新逻辑
public enum GearType
{
    Tile,   //格子拓展都叫Tile
    Gear,   //格子上面放置的Gear
    GearBranch, //分支,常规刷新不能被刷出来
    BattleOnly, //只在战斗里面会刷新出来 其他地方看不到
}

public enum Comparison
{
    Less = 0,
    Equal = 1,
    Great = 2,
}

public enum Difficulty
{
    VeryEasy = 0,
    Easy = 1,
    Normal = 2,
    Hard = 3,
    VeryHard = 4,
    Extreme = 5,
}

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right,
}

#region 地图编辑出生点出怪频率
public enum SpawnFrequencyType
{
    //立刻
    Immediate,
    //短间隔
    ShortInterval,
    //长间隔
    LongInterval,
}
#endregion

