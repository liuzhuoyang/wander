using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TaskData", menuName = "OniData/System/Objective/Task/TaskData", order = 1)]
public class TaskData : ScriptableObject
{
    [ReadOnly]
    public string taskName;
    public int targetNum;
#if UNITY_EDITOR
    public TaskBaseData taskBase;
#endif

    [ReadOnly]
    public string taskBaseName;
    
    public int rewardPoint;

#if UNITY_EDITOR
    [Button("Init Data")]
    public void InitData()
    {
        taskName = this.name;
        taskBaseName = taskBase.taskBaseName;
    }
#endif
}