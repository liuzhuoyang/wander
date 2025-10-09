using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TaskDebug : DebuggerSharedMenu
{
    public void OnCompleteFirstTask()
    {
        GameData.userData.userTask.dictUserTask.Where(x => x.Value.isClaim == false).First().Value.doneNum += 99999;
        TaskSystem.Instance.OnRefresh();
    }

    public void OnRefreshDaily()
    {
        GameData.userData.userTask.dictUserTask = new Dictionary<string, UserTaskArgs>();
        TaskSystem.Instance.Init();
        TaskSystem.Instance.OnRefresh();
    }
}
