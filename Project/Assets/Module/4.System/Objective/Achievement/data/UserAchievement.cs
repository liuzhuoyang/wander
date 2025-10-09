using System.Collections.Generic;

public class UserAchievement
{
    public Dictionary<string, UserAchievementArgs> dictUserAchievement;
    public void InitData()
    {
        dictUserAchievement = new Dictionary<string, UserAchievementArgs>();
    }
}

public class UserAchievementArgs
{
    //总共完成的数量
    public int doneNum;
    //当前完成的阶段
    public int doneStage;
}
