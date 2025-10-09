using System.Collections.Generic;
public class UserLiveEvent
{
    public Dictionary<int, UserLiveEventData> dictLiveEvent;
    public void InitData()
    {
        dictLiveEvent = new Dictionary<int, UserLiveEventData>();
    }

    public void OnResetDaily()
    {
        foreach (var item in dictLiveEvent)
        {
            foreach (var task in item.Value.dictTask)
            {
                task.Value.isClaim = false;
                task.Value.doneNum = 0;
            }
        }
    }
}

public class UserLiveEventData
{
    public string liveEventName;//活动名
    public int endTime;//结束时间
    public string mythicGearName;//当前神话装备
    public string arcaneGearName;//当前秘术装备
    public string exchangeGearName;//当前兑换装备
    public Dictionary<int, int> marketData;//活动兑换详情
    public Dictionary<int, UserTaskArgs> dictTask; //活动任务
    public string liveEventInfo;//活动信息
    public UserLiveEventFundData fundData;
}

public class UserLiveEventFundData
{
    public int rewardProgress;
    public int rewardProgressVIP;
    public int point;
}