
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_task_base", menuName = "OniData/System/Objective/Task/TaskBaseDataCollection", order = 1)]
public class TaskBaseDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<TaskBaseData> listTaskBaseData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        path = $"{path}task_base/";
        listTaskBaseData = FileFinder.FindAllAssets<TaskBaseData>(path);
    }
#endif
}

public static class AllTaskBase
{
    //数据游戏中使用
    public static Dictionary<string, TaskBaseData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, TaskBaseData>();
        TaskBaseDataCollection dataCollection = GameDataControl.Instance.Get("all_task_base") as TaskBaseDataCollection;
        foreach (TaskBaseData taskBaseData in dataCollection.listTaskBaseData)
        {
            dictData.Add(taskBaseData.taskBaseName, taskBaseData);
        }
    }
}