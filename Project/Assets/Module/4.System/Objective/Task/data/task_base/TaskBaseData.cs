using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TaskBaseData", menuName = "OniData/System/Objective/Task/TaskBaseData", order = 1)]
public class TaskBaseData : ScriptableObject
{
    [ReadOnly]
    public string taskBaseName;

    public TaskActionType taskActionType;
    public ActionType actionType;

    [ValueDropdown("GetLocalizationKeyList")]
    public string displayName;
#if UNITY_EDITOR

    public List<string> GetLocalizationKeyList()
    {
        List<string> listKey = new List<string>();
        LocalizationData asset = FileFinder.FindAssetByName<LocalizationData>(EditorPathUtility.localizationPath + "ui/", "loc_page_task");
        foreach (LocalizationSerializedItem item in asset.list)
        {
            listKey.Add(item.key);
        }
        return listKey;
    }

    [Button("Init Data")]
    public void InitData()
    {
        taskBaseName = this.name;
    }
#endif
}