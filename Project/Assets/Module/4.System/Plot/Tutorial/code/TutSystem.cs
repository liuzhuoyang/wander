using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;

//教程系统
public class TutSystem : Singleton<TutSystem>
{
    TutCustomActionManager tutCustomActionManager;
    TutUtilityHelper tutUtilityHelper;

    TutData currentTutData;
    TutDataAction currentTutAction;
    int currentStepIndex;
    public bool isOnTutorial;

    //被教程管理中的按钮列表
    List<Button> managedButtonList = new List<Button>();
    Action onComplete;

    public void Init()
    {
        tutCustomActionManager = gameObject.AddComponent<TutCustomActionManager>();
        tutCustomActionManager.Init();
        tutUtilityHelper = gameObject.AddComponent<TutUtilityHelper>();
        tutUtilityHelper.Init();
        isOnTutorial = false;
    }

    public void OnTriggerTut(string tutName, Action tutPreFunction = null, bool isForce = false, Action onComplete = null)
    {
        currentTutData = AllTutorial.dictData[tutName];
        this.onComplete = onComplete;

        Debug.Log("=== TutSystem: Trigger Tutorial " + currentTutData.tutName + " ===");
        //教程触发后立刻写入用户数据并立即保存
        //GameData.userData.userTut.SetTutComplete(currentTutData.tutName);
        //BattleData.isOnTut = true;

        currentStepIndex = 0;
        tutPreFunction?.Invoke();

        EventManager.TriggerEvent<UITutArgs>(TutEventName.EVENT_ON_TUT_UI, new UITutArgs()
        {

        });

        OnNextStep();
    }

    public async void OnNextStep()
    {
        //处理教程某些步骤的特殊后处理
        TutUtility.FinalizeTutCustomStep(currentTutData, currentStepIndex);

        currentStepIndex++;
        if (currentStepIndex > currentTutData.listAction.Count)
        {
            OnDoneTut();
            return;
        }

        currentTutAction = currentTutData.listAction[currentStepIndex - 1];

        //处理教程某些步骤的特殊预处理
        await TutUtility.InitTutCustomStep(currentTutData, currentStepIndex);

        EventManager.TriggerEvent<UITutArgs>(TutEventName.EVENT_ON_TUT_NEXT_STEP_UI, new UITutArgs()
        {
            currentTutActionArgs = currentTutAction,
        });
        //开启自定义教程任务
        if (currentTutAction.tutActionType == TutActionType.CUSTOM_ACTION)
        {
            StartCustomTutTask(currentTutAction.customAction);
        }

    }

    public void OnDoneTut()
    {
        Debug.Log("=== TutSystem: Done Tutorial " + currentTutData.tutName + " ===");
        EventManager.TriggerEvent<UITutArgs>(TutEventName.EVENT_ON_TUT_END_UI, new UITutArgs()
        {

        });
        onComplete?.Invoke();

        TutUtility.FinalizeTutCustom(currentTutData);
    }

    void StartCustomTutTask(TutCustomAction tutCustomAction)
    {
        EventManager.StartListening<TutCustomActionArgs>(TutEventName.EVENT_ON_TUT_CUSTOM_ACTION, DoneCustomTutAction);

        DisableButton();
        //处理自定义事件
        tutCustomActionManager.InitCustomAction(tutCustomAction);
    }

    void DoneCustomTutAction(TutCustomActionArgs args)
    {
        if (currentTutAction.customAction != args.tutCustomAction) return;
        if (!args.isSuccess)
        {
            tutCustomActionManager.FinalizeCustomAction(args.tutCustomAction);
            tutCustomActionManager.InitCustomAction(args.tutCustomAction);
            return;
        }

        EventManager.StopListening<TutCustomActionArgs>(TutEventName.EVENT_ON_TUT_CUSTOM_ACTION, DoneCustomTutAction);
        tutCustomActionManager.FinalizeCustomAction(args.tutCustomAction);

        EnableButton();
        OnNextStep();
    }

    public void DisableButton()
    {
        isOnTutorial = true;
        // 禁用所有按钮
        managedButtonList = new List<Button>();
        Button[] allButton = FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (var item in allButton)
        {
            if (item.GetComponent<ButtonFeedbackHandler>() == null) continue;
            item.GetComponent<ButtonFeedbackHandler>()?.SetOnTutorial(true);
            //禁用了的按钮加入列表 后续恢复
            managedButtonList.Add(item);
        }
    }

    public void EnableButton()
    {
        isOnTutorial = false;
        foreach (var button in managedButtonList)
        {
            // if (button == null) continue;
            button.GetComponent<ButtonFeedbackHandler>()?.SetOnTutorial(false);
        }
    }
}
