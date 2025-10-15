using System.Collections.Generic;

public class UILobbyArgs : UIBaseArgs
{
    public string displayName;
    //public int chapterID;
    public int levelID;
    //public string pic;
    public int selectedLevel;
    public int totalLevel;
    public int currentWave;
    public Difficulty difficulty;
    public string themeName;
    public int themeVarient;
}

public class UILobbyChangeLevelArgs : UILobbyArgs
{
    public bool isNextLevel;
    public string commingThemeName;
    public int commingThemeVarient;
}

public class UILobbyScrollArgs : UIBaseArgs
{
    public bool isUnlockAnimation; //触发解锁动画
    public int chapterID;
}

public class LobbyEventName
{
    public const string EVENT_LOBBY_INIT = "EVENT_LOBBY_INIT";
    public const string EVENT_LOBBY_REFRESH_UI = "EVENT_LOBBY_REFRESH_UI";
    public const string EVENT_LOBBY_CHANGE_LEVEL = "EVENT_LOBBY_CHANGE_LEVEL";
    public const string EVENT_LOBBY_SHOW_BOTTOM = "EVENT_LOBBY_SHOW_BOTTOM";
}

/*
public class UILobbyTimeEventArgs : UIBaseArgs
{
    public Dictionary<string, UserTimeEventArgs> listUserTimeEventData;
}

public class UILobbyPackArgs : UIBaseArgs
{
    public Dictionary<string,UserPackArgs> dictPack;
}

public class UILobbyPromoArgs : UIBaseArgs
{
    public Dictionary<string, UserPromoArgs> dictPromo;
    public Dictionary<string, int> buyDict;
}*/

