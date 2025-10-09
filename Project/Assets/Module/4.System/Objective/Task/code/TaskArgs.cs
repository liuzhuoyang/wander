using System.Collections.Generic;

public class EventNameTask
{
    public const string EVENT_TASK_REFRESH_UI = "EVENT_TASK_REFRESH_UI";
    public const string EVENT_TASK_REFRESH_REWARD = "EVENT_TASK_REFRESH_REWARD";
}

public class TaskRewardItem
{
    public int pointNeeded;
    public string reward;
    public int rewardNum;
    public bool isClaimed;
    public bool isClaimable;
}

public class UITaskArgs : UIBaseArgs
{
    public int dailyPoint;
    public int dailyClaimPoint;
    public int weeklyPoint;
    public int weeklyClaimPoint;
    public List<TaskViewArgs> listTaskView;
    public List<TaskRewardItem> listDailyReward;
    public int maxDailyPoint;
    public List<TaskRewardItem> listWeeklyReward;
    public int maxWeeklyPoint;
}

public class TaskViewArgs
{
    public string taskName;
    public string displayName;
    public int doneNum;
    public int targetNum;
    public int rewardPoint;
    public bool isClaimable;
    public bool isClaimed;
}