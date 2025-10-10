using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "TutData", menuName = "OniData/System/Plot/Tutorial/TutDataCollection", order = 1)]
public class TutData : ScriptableObject
{
    [ReadOnly]
    [BoxGroup("Info")]
    public string tutName;

    [ReadOnly]
    [BoxGroup("Info")]
    public int totalStep;

    [BoxGroup("Info")]
    [TableList]
    public List<TutDataAction> listAction;

#if UNITY_EDITOR
    [Button("Init Data", ButtonSizes.Gigantic)]
    void OnInitData()
    {
        tutName = this.name;
        LocalizationData locAsset = FileFinder.FindAssetByName<LocalizationData>(EditorPathUtility.localizationPath + "/plot/", "loc_" + tutName);

        totalStep = listAction.Count;
        for (int i = 0; i < totalStep; i++)
        {
            listAction[i].dialogKey = locAsset.list[i].key;
            listAction[i].OnInitData();
        }
    }
#endif
}

[Serializable]
public class TutDataAction
{
    [ReadOnly]
    public string dialogKey;
    public TutActionType tutActionType;
    [ShowIf("tutActionType", Value = TutActionType.BUTTON_ACTION)]
    public string btnEventKey;
    [ShowIf("tutActionType", Value = TutActionType.CUSTOM_ACTION)]
    public TutCustomAction customAction;
    public TutDialogPosition dialogPosition;

    [HideInInspector]
    public string joinedActionString;
    public void OnInitData()
    {
        
    }
}

