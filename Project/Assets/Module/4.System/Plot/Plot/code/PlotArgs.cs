public class UIPlotArgs : UIBaseArgs
{
    public PlotItem currentPlotItem;
    public PlotSceneType sceneType;
}

public class UIPlotLobbyRewardArgs : UIBaseArgs
{
    public string rewardName;
    public int rewardNum;
}

public class PlotEventName
{
    //教程流程相关
    public const string EVENT_ON_PLOT_UI = "EVENT_ON_PLOT_UI";
    public const string EVENT_ON_PLOT_NEXT_STEP_UI = "EVENT_ON_PLOT_NEXT_STEP_UI";
    public const string EVENT_ON_PLOT_LOBBY_REWARD_UI = "EVENT_ON_PLOT_LOBBY_REWARD_UI";
    public const string EVENT_ON_PLOT_END_UI = "EVENT_ON_PLOT_END_UI";
    //public const string EVENT_ON_PLOT_STOP_CURSOR_UI = "EVENT_ON_PLOT_STOP_CURSOR_UI";
}

public enum PlotDialogPosition
{
    Top,
    Mid,
    Bottom,
}

//Lobby对话类型里的类型，分为普通对话，玩家选择和图片
public enum PlotLobbyDialogType
{
    Dialog,   //对话
    Option,   //选择
    Image,    //图片
}

public enum PlotSceneType
{
    Lobby,
    Battle,
    BattleBubble,
}