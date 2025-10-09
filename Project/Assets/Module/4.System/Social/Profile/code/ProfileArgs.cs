using UnityEngine;

public class ProfileArgs
{

}

public class UIProfileArgs : UIBaseArgs
{
    public string userName;
    public string userAvatar;
}

public class EventNameProfile
{
    public const string EVENT_PROFILE_REFRESH_UI = "EVENT_PROFILE_REFRESH_UI";
    
    public const string EVENT_PROFILE_STARTUP_INIT_UI = "EVENT_PROFILE_STARTUP_INIT_UI";
    public const string EVENT_PROFILE_STARTUP_REFRESH_UI = "EVENT_PROFILE_STARTUP_REFRESH_UI";
}