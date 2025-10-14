using System.Collections.Generic;

public enum TooltipType
{
    Text,
    ItemHub,
    Custom
}

public class TooltipItemHubArgs : UITooltipArgs
{
    public List<RewardArgs> listRewardArgs;
}


public class UITooltipArgs : UIBaseArgs
{

    public TooltipType tooltipType;
    public float posX;
    public float posY;
    public Direction direction;
}

public class EventNameTooltip
{
    public const string EVENT_TOOLTIP_OPEN_UI = "EVENT_TOOLTIP_OPEN_UI";
    public const string EVENT_TOOLTIP_CLOSE_UI = "EVENT_TOOLTIP_CLOSE_UI";
}