using System.Collections.Generic;

public class UserTask
{
    public int dailyPoint;
    public int dailyClaimPoint;
    public int weeklyPoint;
    public int weeklyClaimPoint;
    public Dictionary<string, UserTaskArgs> dictUserTask;
    public void InitData()
    {
        dictUserTask = new Dictionary<string, UserTaskArgs>();
        dailyPoint = 0;
        dailyClaimPoint = 0;
        weeklyPoint = 0;
        weeklyClaimPoint = 0;
    }

    public void ResetDaily()
    {
        dictUserTask = new Dictionary<string, UserTaskArgs>();

        dailyPoint = 0;
        dailyClaimPoint = 0;
    }

    public void ResetWeekly()
    {
        weeklyPoint = 0;
        weeklyClaimPoint = 0;
    }
}

public class UserTaskArgs
{
    public int doneNum;
    public bool isClaim;
}
