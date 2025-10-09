using UnityEngine;
using System.Collections.Generic;

public class EndArgs
{   
    public bool isWin;
    public bool isManualQuit;
}

public class EndEventName
{
    public const string EVENT_END_INIT_UI = "EVENT_END_INIT_UI";
    // public const string EVENT_PAWN_END_INIT_UI = "EVENT_PAWN_END_INIT_UI";
}

public class UIEndArgs : UIBaseArgs
{
    public bool isWin;
    public List<RewardArgs> listRewardArgs;
    public bool canTakeAD;
}

public class UIPawnEndArgs : UIBaseArgs
{
    public bool isWin;
    public List<RewardArgs> listRewardArgs;
}