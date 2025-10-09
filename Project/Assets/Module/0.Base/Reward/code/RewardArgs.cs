using System.Collections.Generic;
using System;

public class EventNameReward
{
    public const string EVENT_REWARD_OPEN_UI = "EVENT_REWARD_OPEN_UI";
}

public class RewardArgs
{
    public string reward;
    // public string showName;
    public int num;
    public bool isShard = false;

    /// <summary>
    /// 如果物品是特定名字，比如item_token_shard, item_token_blueprint，进行则进行随机抽取
    /// </summary>
    public void OnConvertDrawItem()
    {

    }
}

public class RewardShowArgs
{
    public string name;
    public int num;
}

public class RewardRandomArgs
{
    public string reward;
    public int min;
    public int max;
}

public class UIRewardArgs : UIBaseArgs
{
    public List<RewardArgs> listRewardArgs;
    public List<RewardShowArgs> listRewardShowArgs;
}
