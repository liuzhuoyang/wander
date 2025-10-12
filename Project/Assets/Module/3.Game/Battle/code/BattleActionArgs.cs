public class BattleActionArgs : UIBaseArgs
{
    public BattleActionType battleAction;
    public string target;
}

public class EventNameBattleAction
{
    public const string EVENT_ON_BATTLE_ACTION = "OnBattleAction";
}