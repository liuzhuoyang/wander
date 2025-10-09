using System.Collections.Generic;

public class EventNameDungeon
{
    public const string EVENT_ON_DUNGEON_REFRESH_UI = "EVENT_ON_DUNGEON_REFRESH_UI";
    public const string EVENT_ON_DUNGEON_CLOSE_UI = "EVENT_ON_DUNGEON_CLOSE_UI";
}

public class UserDungeon
{
    public Dictionary<string, int> dictDungeonLevel;

    public UserDungeon()
    {
        dictDungeonLevel = new Dictionary<string, int>();
    }
}