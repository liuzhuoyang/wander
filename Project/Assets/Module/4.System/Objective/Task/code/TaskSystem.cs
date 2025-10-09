using System;
using System.Collections.Generic;
using System.Linq;
using SimpleVFXSystem;
using UnityEngine;

public class TaskSystem : Singleton<TaskSystem>
{
    Dictionary<string, TaskBaseData> dictTaskBase;
    List<TaskData> listTask;
    UserTask userTask;
    UITaskArgs uiTaskArgs;
    #region 初始化
    public void Init()
    {
        TaskUtil.InitData();
        gameObject.AddComponent<TaskPinHandler>().Init();

        dictTaskBase = AllTaskBase.dictData;
        listTask = AllTask.dictData.Values.ToList();
        userTask = GameData.userData.userTask;

        uiTaskArgs = new UITaskArgs();
        uiTaskArgs.listTaskView = new List<TaskViewArgs>();

        //保护，用户任务数据和任务数量一样
        if (userTask.dictUserTask == null || userTask.dictUserTask.Count != listTask.Count)
        {
            //清空用户任务列表
            // userTask.ResetDaily();
            // userTask.ResetWeekly();
            userTask.dictUserTask.Clear();
            //根据表数据，写入新的用户任务列表
            foreach (TaskData taskData in listTask)
            {
                userTask.dictUserTask.Add(taskData.taskName, new UserTaskArgs() { doneNum = 0, isClaim = false });
            }
        }

        InitData();

        // 如果任务功能解锁，则开始监听任务完成情况
        // if (GlobalFeatureData.CheckIsUnlock(FeatureType.Task))
        // {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
        // }
        // else
        // {
        // 如果任务功能未解锁，则监听功能解锁
        // EventManager.StartListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_TRIGGER_UI, OnTaskUnlock);
        // }
    }

    void InitData()
    {
        uiTaskArgs.listDailyReward = TaskUtil.GetDailyRewardList();
        uiTaskArgs.listWeeklyReward = TaskUtil.GetWeeklyRewardList();
        uiTaskArgs.maxDailyPoint = TaskUtil.GetDailyRewardList().Last().pointNeeded;
        uiTaskArgs.maxWeeklyPoint = TaskUtil.GetWeeklyRewardList().Last().pointNeeded;

        foreach (TaskData taskData in listTask)
        {
            TaskViewArgs taskViewArgs = new TaskViewArgs();
            TaskBaseData taskBaseData = AllTaskBase.dictData[taskData.taskBaseName];
            UserTaskArgs userTaskArgs;
            GameData.userData.userTask.dictUserTask.TryGetValue(taskData.taskName, out userTaskArgs);
            if (userTaskArgs == null) continue;
            taskViewArgs.taskName = taskData.taskName;
            taskViewArgs.targetNum = taskData.targetNum;
            taskViewArgs.rewardPoint = taskData.rewardPoint;
            taskViewArgs.displayName = taskBaseData.displayName;
            uiTaskArgs.listTaskView.Add(taskViewArgs);
        }
    }
    #endregion

    #region 监听任务完成情况
    // void OnTaskUnlock(UIFeatureArgs args)
    // {
    //     if (args.featureType == FeatureType.Task)
    //     {
    //         taskRegistered = true;
    //         EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    //         EventManager.StopListening<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_TRIGGER_UI, OnTaskUnlock);
    //     }
    // }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // if (taskRegistered)
        // {
        // taskRegistered = false;
        EventManager.StopListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
        // }
    }

    void OnAction(ActionArgs args)
    {
        foreach (TaskData taskData in listTask)
        {
            TaskBaseData taskBaseData = dictTaskBase[taskData.taskBaseName];
            //先检查动作
            if (taskBaseData.actionType == args.action)
            {
                UserTaskArgs userTaskArgs = null;
                userTask.dictUserTask.TryGetValue(taskData.taskName, out userTaskArgs);
                if (userTaskArgs == null) continue;
                if (userTaskArgs.isClaim) continue;

                //再检查任务类型
                OnCheckTask(taskBaseData, userTaskArgs, args);
            }
        }
    }

    //动作匹配的情况下，检查任务类型是否匹配
    void OnCheckTask(TaskBaseData taskBaseData, UserTaskArgs userTaskArgs, ActionArgs args)
    {
        switch (taskBaseData.actionType)
        {
            //关卡结束时候一次性处理战斗内相关任务
            case ActionType.LevelEnd:
                if (taskBaseData.taskActionType == TaskActionType.Kill) userTaskArgs.doneNum += BattleData.enemyKilled;        //击杀敌人
                if (taskBaseData.taskActionType == TaskActionType.Merge) userTaskArgs.doneNum += BattleData.mergeCount;      //合成
                if (taskBaseData.taskActionType == TaskActionType.EarnBattleToken) userTaskArgs.doneNum += BattleData.battleTokenEarned; //赚取的战斗币数量
                if (taskBaseData.taskActionType == TaskActionType.PassLevel)
                {
                    if (BattleData.isPassLevel) userTaskArgs.doneNum += 1;
                }
                break;
            case ActionType.UseEnergy:
                if (taskBaseData.taskActionType == TaskActionType.UseEnergy) userTaskArgs.doneNum += args.value;
                break;
            default:
                //其他事件默认每个事件+1完成度
                userTaskArgs.doneNum += 1;
                break;
        }
        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.TaskClaimed });
    }

    #endregion

    #region 打开任务界面
    public async void Open()
    {
        await UIMain.Instance.OpenUI("task", UIPageType.Normal);
        OnRefresh();
    }
    #endregion

    #region 刷新任务
    public void OnRefresh()
    {
        //刷新任务进度
        foreach (TaskViewArgs taskViewArgs in uiTaskArgs.listTaskView)
        {
            UserTaskArgs userTaskArgs;
            GameData.userData.userTask.dictUserTask.TryGetValue(taskViewArgs.taskName, out userTaskArgs);
            taskViewArgs.doneNum = userTaskArgs.doneNum;
            taskViewArgs.isClaimed = userTaskArgs.isClaim;
            taskViewArgs.isClaimable = CheckIsTaskCompletable(taskViewArgs.taskName);
        }
        //排序
        SortList();

        uiTaskArgs.dailyPoint = userTask.dailyPoint;
        uiTaskArgs.dailyClaimPoint = userTask.dailyClaimPoint;
        uiTaskArgs.weeklyPoint = userTask.weeklyPoint;
        uiTaskArgs.weeklyClaimPoint = userTask.weeklyClaimPoint;

        //刷新奖励
        OnRefreshReward();
        EventManager.TriggerEvent<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_UI, uiTaskArgs);
    }
    void OnRefreshReward()
    {
        for (int i = 0; i < uiTaskArgs.listDailyReward.Count; i++)
        {
            uiTaskArgs.listDailyReward[i].isClaimable = CheckIsRewardClaimable(i, TaskType.DailyTask);
            uiTaskArgs.listDailyReward[i].isClaimed = CheckIsRewardClaimed(i, TaskType.DailyTask);
        }
        for (int i = 0; i < uiTaskArgs.listWeeklyReward.Count; i++)
        {
            uiTaskArgs.listWeeklyReward[i].isClaimable = CheckIsRewardClaimable(i, TaskType.WeeklyTask);
            uiTaskArgs.listWeeklyReward[i].isClaimed = CheckIsRewardClaimed(i, TaskType.WeeklyTask);
        }
    }
    #endregion

    #region 排序
    void SortList()
    {
        uiTaskArgs.listTaskView.Sort((x, y) =>
        {
            if (x.isClaimed != y.isClaimed)
            {
                return x.isClaimed == true ? 1 : -1;
            }
            if (x.isClaimable != y.isClaimable)
            {
                return x.isClaimable == true ? -1 : 1;
            }
            return y.doneNum.CompareTo(x.doneNum);
        });
    }
    #endregion

    #region 检查任务是否可以完成
    public bool CheckIsTaskCompletable(string taskName)
    {
        if (!userTask.dictUserTask.ContainsKey(taskName))
        {
            return false;
        }
        UserTaskArgs userTaskArgs = userTask.dictUserTask[taskName];
        TaskData taskData = AllTask.dictData[taskName];
        return userTaskArgs.doneNum >= taskData.targetNum;
    }
    #endregion

    #region 检查上方奖励是否可以领取
    bool CheckIsRewardClaimable(int index, TaskType taskType)
    {
        int currentPoint = taskType == TaskType.DailyTask ? uiTaskArgs.dailyPoint : uiTaskArgs.weeklyPoint;
        int pointNeeded = taskType == TaskType.DailyTask ? uiTaskArgs.listDailyReward[index].pointNeeded : uiTaskArgs.listWeeklyReward[index].pointNeeded;
        return currentPoint >= pointNeeded;
    }
    #endregion

    #region 检查上方奖励是否已经被领取
    bool CheckIsRewardClaimed(int index, TaskType taskType)
    {
        bool isClaim = false;
        int reachPoint = taskType == TaskType.DailyTask ? uiTaskArgs.dailyClaimPoint : uiTaskArgs.weeklyClaimPoint;
        int pointNeeded = taskType == TaskType.DailyTask ? uiTaskArgs.listDailyReward[index].pointNeeded : uiTaskArgs.listWeeklyReward[index].pointNeeded;

        if (reachPoint >= pointNeeded)
            return true;

        return isClaim;
    }
    #endregion

    #region 领取任务奖励
    public void OnCompleteTask(string taskName, int rewardNum, Transform transSlot)
    {
        if (!CheckIsTaskCompletable(taskName))
        {
            // TipManager.Instance.OnTip(GameData.AllLocalization["tip/task_not_completed"]);
            return;
        }
        //更新点数
        userTask.dailyPoint += rewardNum;
        userTask.weeklyPoint += rewardNum;

        //播放飞行特效
        // VFXControl.Instance.OnUIFlyerVFX(ConstantItem.TOKEN_TASK, transSlot.transform.position);
        VFXManager.Instance.OnVFXFlayerBatchUI(new List<RewardArgs>() { new RewardArgs() { reward = ConstantItem.POINT_TASK, num = rewardNum } });

        GameData.userData.userTask.dictUserTask[taskName].isClaim = true;
        OnRefresh();
        // CheckPin();

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.TaskClaimed });
    }

    public bool CheckIsTaskCompletable(TaskType taskType, string taskName)
    {
        if (!userTask.dictUserTask.ContainsKey(taskName))
            return false;
        UserTaskArgs userTaskArgs = userTask.dictUserTask[taskName];
        TaskData taskData = AllTask.dictData[taskName];
        return userTaskArgs.doneNum >= taskData.targetNum;
    }
    #endregion

    #region 领取进度奖励
    public void OnCompleteProgressReward(TaskType taskType)
    {
        List<TaskRewardItem> listRewardItem = taskType == TaskType.DailyTask ? uiTaskArgs.listDailyReward : uiTaskArgs.listWeeklyReward;
        //判断进度类型
        int point = 0;
        List<RewardArgs> listRewardArgs = new List<RewardArgs>();
        foreach (TaskRewardItem rewardItem in listRewardItem)
        {
            //已经领取过跳过
            if (rewardItem.isClaimed)
                continue;
            //不能领取跳过
            if (!rewardItem.isClaimable)
                break;
            rewardItem.isClaimed = true;
            point = rewardItem.pointNeeded;
            listRewardArgs.Add(new RewardArgs() { reward = rewardItem.reward, num = rewardItem.rewardNum });
        }
        if (listRewardArgs.Count <= 0)
        {
            return;
        }
        if (taskType == TaskType.DailyTask)
        {
            userTask.dailyClaimPoint = point;
            uiTaskArgs.dailyClaimPoint = point;
        }
        else if (taskType == TaskType.WeeklyTask)
        {
            userTask.weeklyClaimPoint = point;
            uiTaskArgs.weeklyClaimPoint = point;
        }
        RewardSystem.Instance.OnReward(listRewardArgs);
        OnRefreshReward();
        EventManager.TriggerEvent<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_REWARD, uiTaskArgs);

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.TaskClaimed });
    }
    #endregion
}