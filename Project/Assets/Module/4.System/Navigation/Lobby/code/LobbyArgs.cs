using System.Collections.Generic;

public class UILobbyArgs : UIBaseArgs
{
    // public bool isLockChapter;
    public string chapterDisplayName;
    public bool isReadyToUnlockSector;
    public int chapterID;
    public int levelID;
    public string picName;
    public int currentWave;
    public Difficulty difficulty;
    public string themeName;
}

public class UILobbyScrollArgs : UIBaseArgs
{
    public bool isUnlockAnimation; //触发解锁动画
    public int chapterID;
}

public class LobbyEventName
{
    public const string EVENT_LOBBY_REFRESH_UI = "EVENT_LOBBY_REFRESH_UI";
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

