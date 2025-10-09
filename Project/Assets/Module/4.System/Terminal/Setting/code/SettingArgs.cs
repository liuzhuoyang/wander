public class EventNameSetting
{
    public const string EVENT_SETTING_REFRESH_UI = "OnRefreshSettingUI";
    public const string EVENT_SETTING_CLOSE_UI = "OnCloseSettingUI";
}
public class UISettingArgs : UIBaseArgs
{
    public string uid;
    public bool isSoundOn;
    public bool isMusicOn;
    public bool isNotiOn;
    public bool isHapticOn;
}
