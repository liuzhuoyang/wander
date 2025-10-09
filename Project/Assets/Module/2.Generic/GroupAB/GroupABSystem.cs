using System;
using System.Collections.Generic;
using System.Linq;

public class GroupABSystem : Singleton<GroupABSystem>
{
    public GroupABArgs groupABArgs;

    public void Init()
    {
        groupABArgs = new GroupABArgs();
        groupABArgs.isOpen = false;
        groupABArgs.listBattleLevel = new List<GroupABBattleLevelArgs>();
        groupABArgs.groupABType = GameData.userData.userAnalytics.groupABType;
        //老用户数据
        // if (groupABArgs.groupABType == GroupABType.None)
        // {
        //     //A-50% B-50%
        //     groupABArgs.groupABType = Random.Range(0, 100) < 50 ? GroupABType.A : GroupABType.B;
        //     GameData.userData.userAnalytics.groupABType = groupABArgs.groupABType;
        //     EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
        //     {
        //         action = ActionType.UpdateGroupAB
        //     });
        // }
        //B组用户需要请求是否启动AB测试
        if (groupABArgs.groupABType != GroupABType.B)
        {
            return;
        }
        CloudGroupAB.Instance.GetGroupABOpen();
    }

    public void SetGroupABOpen(bool isOpen)
    {
        groupABArgs.isOpen = isOpen;
        if (isOpen)
        {
            CloudGroupAB.Instance.GetGroupABLevel(null);
        }
    }

    public void SetGroupABLevel(List<GroupABBattleLevelArgs> listBattleLevel)
    {
        groupABArgs.listBattleLevel = listBattleLevel;
    }

    public GroupABBattleLevelArgs GetGroupABLevel(int chapter, int level)
    {
        var data = groupABArgs.listBattleLevel.Where(x => x.chapter == chapter && x.level == level).ToList();
        if (data.Count == 0)
        {
            return null;
        }
        return data[0];
    }

    #region debug
    public void SetGroupABType(Action callBack)
    {
        if (groupABArgs.groupABType == GroupABType.A)
        {
            groupABArgs.listBattleLevel.Clear();
            callBack?.Invoke();
        }
        else
        {
            MessageManager.Instance.OnLoading();
            CloudGroupAB.Instance.GetGroupABLevel(() =>
            {
                MessageManager.Instance.CloseLoading();
                callBack?.Invoke();
            });
        }

    }
    #endregion
}