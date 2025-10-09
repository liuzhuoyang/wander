using System.Collections.Generic;

public class EventNameHeader
{
    public const string EVENT_HEADER_UPDATE_ITEM_NUM_UI = "EVENT_UPDATE_ITEM_NUM_UI";
    public const string EVENT_HEADER_SHOW_UI = "EVENT_HEADER_SHOW_UI";
    public const string EVENT_HEADER_HIDE_UI = "EVENT_HEADER_HIDE_UI";
    public const string EVENT_HEADER_INIT_UI = "EVENT_HEADER_INIT_UI";

    public const string EVENT_HEADER_REFRESH_PROFILE = "EVENT_HEADER_REFRESH_PROFILE";

    //隐藏显示部分模块（profile，金币等）
    public const string EVENT_HEADER_SHOW_HIDE_HUB_UI = "EVENT_HEADER_SHOW_HIDE_HUB_UI";

    public const string EVENT_HEADER_REFRESH_POWER = "EVENT_HEADER_REFRESH_POWER";
}

public class UIHeaderArgs : EventArgs
{
    public int power;
}

public class UIHeaderItemNumArgs : EventArgs
{
    public bool isPlayAnimation;
    public string itemName;
    public int startValue;
    public int endValue;
}

public class UIHeaderShowHeaderArgs : EventArgs
{
    public List<string> listShowHub;
}
