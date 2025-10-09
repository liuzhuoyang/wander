using System.Collections.Generic;
using UnityEngine;

public enum TooltipType
{
    TEXT,
    IMAGE,
    ITEMREWARD,
    ITEMINFO,
}

public class TooltipArgs
{
    public TooltipType tooltipType;
    public TooltipContentArgs contentArgs;
    public TooltipPosArgs posArgs;
}

public class TooltipContentArgs
{
    public string itemName;
    public List<string> contentList;
    public List<string> imageNameList;
    public List<RewardArgs> itemRewardList;
}

public class TooltipPosArgs
{    
    public RectTransform entryTransform;
    public Vector2 centerPoint;
    public Direction direction;
}

public class UITooltipArgs : UIBaseArgs
{
    public TooltipArgs tooltipArgs;
}

public class EventNameTooltip
{
    public const string EVENT_OPEN_UI = "EVENT_OPEN_UI";
    public const string EVENT_CLOSE_UI = "EVENT_CLOSE_UI";
}