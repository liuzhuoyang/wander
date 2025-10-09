using UnityEngine;
using System.Collections.Generic;
using System;
/*
public class TutArgs
{
    public string tutName;
    public int totalStep;
    public List<TutActionArgs> listActionArgs;
    public string joinedActionString;
    public string chainEventName;

    public void OnInitData()
    {
        listActionArgs = new List<TutActionArgs>();
        string[] parsedString = joinedActionString.Split('&');
        foreach (string actionString in parsedString)
        {
            TutActionArgs actionArgs = new TutActionArgs();
            actionArgs.OnInitData(actionString);
            listActionArgs.Add(actionArgs);
        }
    }
}

public class TutActionArgs
{
    public string dialogKey;
    public TutActionType tutActionType;
    public string btnEventKey;
    public TutCustomAction customAction;
    public TutDialogPosition dialogPosition;

    public void OnInitData(string actionString)
    {
        string[] parsedString = actionString.Split('^');
        dialogKey = parsedString[0];
        tutActionType = (TutActionType)Enum.Parse(typeof(TutActionType), parsedString[1]);
        btnEventKey = parsedString[2];
        customAction = (TutCustomAction)Enum.Parse(typeof(TutCustomAction), parsedString[3]);
        dialogPosition = (TutDialogPosition)Enum.Parse(typeof(TutDialogPosition), parsedString[4]);
    }
}*/

public class UITutArgs : UIBaseArgs
{
    public TutDataAction currentTutActionArgs;
}

public class UITutFingerArgs : UIBaseArgs
{
    public Vector2 startPos;
    public Vector2 endPos;
}

public class UITutMaskArgs : UIBaseArgs
{
    public Transform highlightObj;
}

public class TutEventName
{
    //教程流程相关
    public const string EVENT_ON_TUT_UI = "EVENT_ON_TUT_UI";
    public const string EVENT_ON_TUT_NEXT_STEP_UI = "EVENT_ON_TUT_NEXT_STEP_UI";
    public const string EVENT_ON_TUT_END_UI = "EVENT_ON_TUT_END_UI";
    //教程按钮相关
    public const string EVENT_ON_TUT_BTN_TRIGGER_UI = "EVENT_ON_TUT_BTN_TRIGGER_UI";
    public const string EVENT_ON_TUT_BTN_TRIGGERED_UI = "EVENT_ON_TUT_BTN_TRIGGERED_UI";
    //教程自定义事件相关
    public const string EVENT_ON_TUT_CUSTOM_ACTION = "EVENT_ON_TUT_CUSTOM_ACTION";
    //教程手指动画
    public const string EVENT_TUT_FINGER_DRAG_UI = "EVENT_TUT_FINGER_DRAG_UI";
    public const string EVENT_TUT_FINGER_STOP_UI = "EVENT_TUT_FINGER_STOP_UI";
}

//教程中指向的类型
public enum TutActionType
{
    DIALOG_ACTION,  //点击屏幕
    BUTTON_ACTION,  //点击按钮
    CUSTOM_ACTION,  //自定义场景动作
}

public enum TutCustomAction
{
    None = 0,
    TUT_BATTLE_GEAR_MOVE = 101,
    TUT_BATTLE_TILE_EXPAND = 102,
    TUT_BATTLE_SUPPORT_SKILL_SHOW_UP = 103,
    TUT_TAVERN_SUMMON_GEAR = 201,
}

public enum TutDialogPosition
{
    Hide,
    Top,
    Mid,
    Bottom,
}

public class TutBtnHandlerArgs : EventArgs
{
    public string btnEventKey;
    public Action onClickAction;
    public Transform uiRootTransform;
    public RectTransform btnTransform;
    public bool isCreatePointer;
}

public class TutCustomActionArgs : EventArgs
{
    public TutCustomAction tutCustomAction;
    public bool isSuccess;
}