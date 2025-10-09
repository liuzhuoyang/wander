
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_task", menuName = "OniData/System/Objective/Task/TaskDataCollection", order = 1)]
public class TaskDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<TaskData> listTaskData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        path = $"{path}task/";
        listTaskData = AssetsFinder.FindAllAssets<TaskData>(path);
    }
#endif
}

public static class AllTask
{
    //数据游戏中使用
    public static Dictionary<string, TaskData> dictData;

    //初始化数据，从资源中加载
    public static void Init()
    {
        dictData = new Dictionary<string, TaskData>();
        TaskDataCollection dataCollection = GameDataControl.Instance.Get("all_task") as TaskDataCollection;
        foreach (TaskData taskData in dataCollection.listTaskData)
        {
            dictData.Add(taskData.taskName, taskData);
        }
    }
}