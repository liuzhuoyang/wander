using UnityEngine;

public class UITask : UIBase
{
    [SerializeField] RectTransform container;
    [SerializeField] GameObject prefabTaskSlot;
    [SerializeField] TaskViewReward taskDailyReward, taskWeeklyReward;
    [SerializeField] TimeViewBase timeViewBase;

    void Awake()
    {
        EventManager.StartListening<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_UI, OnRefresh);
        EventManager.StartListening<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_REWARD, OnRefreshReward);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_UI, OnRefresh);
        EventManager.StopListening<UITaskArgs>(EventNameTask.EVENT_TASK_REFRESH_REWARD, OnRefreshReward);
    }

    void OnRefresh(UITaskArgs uiTaskArgs)
    {
        timeViewBase.Refresh(TimeManager.Instance.GetSecondUntilNextDay());
        //刷新进度
        RefreshProgress(uiTaskArgs);
        //刷新任务列表
        RefreshTaskList(uiTaskArgs);
    }

    void OnRefreshReward(UITaskArgs uiTaskArgs)
    {
        RefreshProgress(uiTaskArgs);
    }

    void RefreshProgress(UITaskArgs uiTaskArgs)
    {
        taskDailyReward.Init(uiTaskArgs.dailyPoint, uiTaskArgs.maxDailyPoint, uiTaskArgs.listDailyReward, TaskType.DailyTask);
        taskWeeklyReward.Init(uiTaskArgs.weeklyPoint, uiTaskArgs.maxWeeklyPoint, uiTaskArgs.listWeeklyReward, TaskType.WeeklyTask);
    }

    void RefreshTaskList(UITaskArgs uiTaskArgs)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (TaskViewArgs viewArgs in uiTaskArgs.listTaskView)
        {
            GameObject go = Instantiate(prefabTaskSlot, container);
            go.GetComponent<TaskViewSlot>().Init(viewArgs);
        }
    }

    public void OnClose()
    {
        base.CloseUI();
    }
}