public class EventNameModeHome
{
    public const string EVENT_HOME_ONSELECT_UI = "EVENT_HOME_ONSELECT_UI";
    public const string EVENT_HOME_HIDE_CURRENT_UI = "EVENT_HOME_HIDE_CURRENT_UI";
    //public const string EVENT_HOME_SHOW_CURRENT_UI = "EVENT_HOME_SHOW_CURRENT_UI";
}

public class UIModeHomeArgs : UIBaseArgs
{
    public bool isMovingLeft;
    public string targetPage;
    public string displayArgs;
    public string leftPage;
    public string rightPage;
}
