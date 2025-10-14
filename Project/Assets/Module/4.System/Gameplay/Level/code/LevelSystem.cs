using UnityEngine;

public class LevelSystem : Singleton<LevelSystem>
{
    [SerializeField] private LevelDataCollection levelDataCollection;
    public LevelData GetLevelData(int chapterID, int levelID)
    {
        var levelKey = $"level_{chapterID:D3}_{levelID:D3}";
        return levelDataCollection.listLevelData.Find(x=>x.name == levelKey);
    }
}
