using System.Collections.Generic;

/// <summary>
/// 遗物数据
/// </summary>
public class EventNameRelic
{
    public const string EVENT_RELIC_REFRESH_UI = "EVENT_RELIC_REFRESH_UI";
    public const string EVENT_RELIC_REFRESH_INFO = "EVENT_RELIC_REFRESH_INFO";
    public const string EVENT_RELIC_REFRESH_STARUP = "EVENT_RELIC_REFRESH_STARUP";
    public const int RELIC_STAR_MAX = 10;
}

public class UIRelicArgs : UIBaseArgs
{
    public Dictionary<Rarity, List<RelicSlotViewArgs>> dictRelicSlotViewArgs;
    public bool needRefresh;
}

public class RelicSlotViewArgs : UIBaseArgs
{
    public RelicData relicData;
    public int count;
    public int star;
    public bool isNew;
    public int needCount;
}