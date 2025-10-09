using System.Collections.Generic;

public class EventNameItem
{
    public const string EVENT_ON_ITEM_CHANGED = "OnEventItemChanged";
}

public enum ItemType
{
    Item,
    Currency,
    Token,
    Pass,
    Key,
    Battle
}

// public class ItemArgs
// {
//     public string itemName;
//     public string displayName;
//     public Rarity rarity;
//     public bool isHide;
//     public bool isReset;
//     public TimeResetType timeResetType;
//     public string textFlavor;
//     public string textSource;
//     public ItemType itemType;
//     public string sfxDrop;
//     public string sfxCollect;
// }

/*
public class UserItemArgs
{
    public string itemName;
    public int num;
}*/

public class ItemUsageArgs
{
    public string itemName;
    public int costNum;
}

public class ItemChangeArgs : EventArgs
{
    public List<string> listItem;
    public List<int> listItemChangeValue;
}

public class ItemViewSlotArgs
{
    public string itemName;
    public int num;
    public Rarity rarity;
    public bool isInteractable;
}
