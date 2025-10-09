public class EventNameFooter
{
    public const string EVENT_SHOW_FOOTER_UI = "EVENT_SHOW_FOOTER_UI";
    public const string EVENT_HIDE_FOOTER_UI = "EVENT_HIDE_FOOTER_UI";

    public const string EVENT_ON_SELECT_FOOTER_UI = "EVENT_ON_SELECT_FOOTER_UI";

    public const string EVENT_ON_LOCK_PLAY_FOOTER_UI = "EVENT_ON_LOCK_PLAY_FOOTER_UI";
    public const string EVENT_ON_UNLOCK_PLAY_FOOTER_UI = "EVENT_ON_UNLOCK_PLAY_FOOTER_UI";
    public const string EVENT_ON_TICK_FOOTER_UI = "EVENT_ON_TICK_FOOTER_UI";
}

public class UIFooterArgs : EventArgs
{
    public string tabName;
}