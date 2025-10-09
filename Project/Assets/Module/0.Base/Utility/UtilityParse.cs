public static class UtilityParse
{
    public static string GetLevelName(int chapterID, int levelID, LevelType levelType)
    {
        string chapterIdStream = chapterID.ToString("D3");
        string levelIdStream = levelID.ToString("D2");
        string levelName = "level_" + levelType.ToString().ToLower() + "_" + chapterIdStream + "_" + levelIdStream;
        return levelName;
    }
}
