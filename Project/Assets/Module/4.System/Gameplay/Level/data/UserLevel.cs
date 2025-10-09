
public class UserLevel
{
    public UserLevelProgress levelProgressMain; //主线关卡进度数据

    //游戏关卡动态难度，是一个隐藏的数据，用户满足某些条件，比如失败太多，提升幸运值，暗中调整让玩家获得更强局内增益或道具
    public UserLevelLuck levelLuck; 

    public UserLevel()
    {
        levelProgressMain = new UserLevelProgress()
        {
            levelIndex = 1,
            bestWave = 0,
        };
    }
}

public class UserLevelProgress
{
    public int levelIndex;  //当前关卡ID
    public int bestWave;    //最佳波次
}

//游戏关卡动态难度，是一个隐藏的数据，用户满足某些条件，比如失败太多，提升幸运值，暗中调整让玩家获得更强局内增益或道具

public class UserLevelLuck
{
    //幸运值,是一个0-1的float值
    //0是默认，1是最幸运（也是玩家失败最多的时候）
    public float luck;
    public int winStreak; //连胜给上难度
    public int levelRetryCount;
    public int loseStreak;

    public void OnLevelWin()
    {
        //重试次数清零
        levelRetryCount = 0;
        //增加连胜次数
        winStreak++;
        //幸运值清零
        luck = 0;
    }

    public void OnLevelFail()
    {
        //重试次数增加一次
        levelRetryCount++;
        //连胜次数清零
        winStreak = 0;
        //增加一点幸运值
        luck += 0.1f;
        if(luck > 1) luck = 1;//幸运值不能超过1
    }
}

