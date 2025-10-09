using UnityEngine;

//Loot Pin的检测条件
public class TaskPinHandler : MonoBehaviour
{
    private const string TASK_PIN_ID = "pin_task";
    public void Init()
    {
        CheckTaskPin();
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void OnDestroy()
    {
        EventManager.StartListening<ActionArgs>(EventNameAction.EVENT_ON_ACTION, OnAction);
    }
    void CheckTaskPin()
    {
        /*
        if (!GlobalFeatureData.CheckIsUnlock(FeatureType.Task))
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(TASK_PIN_ID, false));
            return;
        }*/
        
        //判断积分
        UserTask userTask = GameData.userData.userTask;
        if (userTask.dailyPoint >= TaskUtil.GetNextDailyPoint(userTask.dailyClaimPoint))
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(TASK_PIN_ID, true));
            return;
        }
        if (userTask.weeklyPoint >= TaskUtil.GetNextWeeklyPoint(userTask.weeklyClaimPoint))
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(TASK_PIN_ID, true));
            return;
        }
        //判断任务
        foreach (var task in userTask.dictUserTask)
        {
            if (task.Value.isClaim)
            {
                continue;
            }
            if (task.Value.doneNum >= AllTask.dictData[task.Key].targetNum)
            {
                EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(TASK_PIN_ID, true));
                return;
            }
        }
        EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(TASK_PIN_ID, false));
    }
    void OnAction(ActionArgs args)
    {
        if (args.action == ActionType.OnBackToLobby)
        {
            CheckTaskPin();
        }
    }
}
