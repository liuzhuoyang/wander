public class ActionArgs : UIBaseArgs
{
    public ActionType action;
    public string target;
    public int value;
}

public class EventNameAction
{
    public const string EVENT_ON_ACTION = "OnAction";
    public const string EVENT_ON_BATTLE_ACTION = "OnBattleAction";
    public const string EVENT_ON_MERGEVIEW_REFRESH = "OnMergeViewRefresh";
}

public class BattleActionArgs : UIBaseArgs
{
    public BattleActionType battleAction;
    public string target;
}