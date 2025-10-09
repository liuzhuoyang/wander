using System.Collections.Generic;

public class EventNameLoot
{
    public const string EVENT_LOOT_INIT = "EVENT_LOOT_INIT";
    public const string EVENT_LOOT_REFRESH_UI = "EVENT_LOOT_REFRESH_UI";
    public const string EVENT_LOOT_CLAIM_REFRESH = "EVENT_LOOT_CLAIM_REFRESH";
}

public class UILootArgs : UIBaseArgs
{
    public UIChestLockArgs chestLockArgs;//进度宝箱
    public int chooseIndex;//选择宝箱
    public Rarity chooseRarity;//选择宝箱品质
    public int canOpenCount;//可开宝箱数量
    public List<int> listCount;//宝箱数量
}

public class UIChestLockArgs
{
    public int chestIndex;
    public int needPoint;
}

public class UIChestProbabilityArgs
{
    // public ChestItemArgs itemOneArgs;
    // public ChestItemArgs itemTwoArgs;
    public List<ChestProArgs> listChestProArgs;//宝箱概率
}

// public class ChestItemArgs
// {
//     public string itemName;
//     public float probability;
//     public List<ChestItemProArgs> listItemProArgs;
// }

// public class ChestItemProArgs
// {
//     public float probability;
//     public List<int> listItemNum;
// }

public class ChestProArgs
{
    public Rarity rarity;
    public bool isShard;
    public float probability;
}