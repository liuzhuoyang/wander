using UnityEngine;

public class LevelSystem : Singleton<LevelSystem>
{
    [SerializeField] private LevelDataCollection levelDataCollection;
    public LevelData GetLevelData(LevelType levelType, int chapterID, int levelID)
    {
        string typeKey = string.Empty;
        switch(levelType)
        {
            case LevelType.Main: typeKey = "main"; break;
            case LevelType.Event: typeKey = "event"; break;
            case LevelType.Dungeon: typeKey = "dungeon"; break;
        }
        var levelKey = $"level_{typeKey}_{chapterID:D3}_{levelID:D3}";
        return levelDataCollection.listLevelData.Find(x=>x.name == levelKey);
    }
}
