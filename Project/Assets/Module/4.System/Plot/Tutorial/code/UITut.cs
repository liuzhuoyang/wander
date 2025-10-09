using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITut : MonoBehaviour
{
    public GameObject objDialog;
    public TextMeshProUGUI textContent;

    public UITutHoleMask holeMask;
    public GameObject mask;

    public GameObject prefabFinger;
    GameObject objFinger;

    bool isBtnTutTriggered; //教程按钮事件是否成功触发
    TutDataAction currentTutActionArgs;

    float clickCD;
    // List<string> listBtnEventKey;

    private void Start()
    {
        EventManager.StartListening<UITutArgs>(TutEventName.EVENT_ON_TUT_UI, OnTutStart);
        EventManager.StartListening<UITutArgs>(TutEventName.EVENT_ON_TUT_NEXT_STEP_UI, OnNextStep);
        EventManager.StartListening<UITutArgs>(TutEventName.EVENT_ON_TUT_END_UI, OnTutEnd);
        EventManager.StartListening<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGERED_UI, OnTutBtnTriggered);

        EventManager.StartListening<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_DRAG_UI, OnFingerAnime);
        EventManager.StartListening<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_STOP_UI, OnStopFingerAnime);

        mask.SetActive(false);
        holeMask.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UITutArgs>(TutEventName.EVENT_ON_TUT_UI, OnTutStart);
        EventManager.StopListening<UITutArgs>(TutEventName.EVENT_ON_TUT_NEXT_STEP_UI, OnNextStep);
        EventManager.StopListening<UITutArgs>(TutEventName.EVENT_ON_TUT_END_UI, OnTutEnd);
        EventManager.StopListening<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGERED_UI, OnTutBtnTriggered);

        EventManager.StopListening<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_DRAG_UI, OnFingerAnime);
        EventManager.StopListening<UITutFingerArgs>(TutEventName.EVENT_TUT_FINGER_STOP_UI, OnStopFingerAnime);
    }

    void OnTutStart(UITutArgs args)
    {
        gameObject.SetActive(true);
        objDialog.SetActive(true);
        // listBtnEventKey = new List<string>();
    }

    void OnNextStep(UITutArgs args)
    {
        currentTutActionArgs = args.currentTutActionArgs;
        //设置对话框位置
        SetDialogPosition(args.currentTutActionArgs.dialogPosition);
        textContent.text = UtilityLocalization.GetPlotLocalization(args.currentTutActionArgs.dialogKey);
        float preferredHeight = textContent.preferredHeight;
        //对话框的高度自适应
        textContent.GetComponent<RectTransform>().sizeDelta = new Vector2(textContent.GetComponent<RectTransform>().sizeDelta.x, preferredHeight);
        //外框的高度加40
        objDialog.GetComponent<RectTransform>().sizeDelta = new Vector2(objDialog.GetComponent<RectTransform>().sizeDelta.x, preferredHeight + 80);
        ResetCD();

        //处理按钮类教程
        if (args.currentTutActionArgs.tutActionType == TutActionType.BUTTON_ACTION)
        {
            // if (!listBtnEventKey.Contains(args.currentTutActionArgs.btnEventKey))
            // {
                //如果按钮不在预制体里配置，则手动添加按钮Handler
                if (TutUtility.IsBtnEventKeyNeedSearch(args.currentTutActionArgs.btnEventKey))
                {
                    StartCoroutine(AddTutButtonHandler(args.currentTutActionArgs.btnEventKey));
                }
                //重复发送事件直到按钮接受到事件为止
                StartCoroutine(SendTutButtonEvent());
                // listBtnEventKey.Add(args.currentTutActionArgs.btnEventKey);
            // }
        }

        mask.SetActive(args.currentTutActionArgs.tutActionType == TutActionType.DIALOG_ACTION);
        mask.GetComponent<CanvasGroup>().alpha = args.currentTutActionArgs.dialogPosition == TutDialogPosition.Hide ? 0.1f : 1f;
        holeMask.gameObject.SetActive(args.currentTutActionArgs.tutActionType == TutActionType.BUTTON_ACTION);
    }

    IEnumerator AddTutButtonHandler(string btnEventKey)
    {
        int trialCount = 0;
        yield return null;
        GameObject objBtn = null;
        while (objBtn == null || objBtn.GetComponent<TutBtnHandler>() == null && trialCount < 15)
        {
            //搜索按钮对象添加handler
            trialCount++;
            objBtn = TutUtility.SearchBtnObjInScene(btnEventKey);
            if (objBtn != null)
            {
                objBtn.AddComponent<TutBtnHandler>();
                objBtn.GetComponent<TutBtnHandler>().btnEventKey = btnEventKey;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    IEnumerator SendTutButtonEvent()
    {
        isBtnTutTriggered = false;
        int trialCount = 0;
        ActingSystem.Instance.OnActing(this.name);
        yield return null;
        while (!isBtnTutTriggered && trialCount < 20)
        {
            trialCount++;
            EventManager.TriggerEvent<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGER_UI, new TutBtnHandlerArgs()
            {
                btnEventKey = currentTutActionArgs.btnEventKey,
                onClickAction = OnTutBtnClick,
                uiRootTransform = transform,
                isCreatePointer = TutUtility.IsCreatePointer(currentTutActionArgs.btnEventKey),
            });
            yield return new WaitForSecondsRealtime(0.1f);
        }
        isBtnTutTriggered = false;

        if (trialCount >= 20)
        {
            Debug.LogError("=== TutSystem: 找不到指定的教程按钮 强制结束教程: " + currentTutActionArgs.btnEventKey + " ===");
            TutSystem.Instance.OnDoneTut();
            ActingSystem.Instance.StopActing(this.name);
        }
    }

    void OnTutEnd(UITutArgs args)
    {
        mask.SetActive(false);
        holeMask.StopMask();
        holeMask.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void SetDialogPosition(TutDialogPosition pos)
    {
        objDialog.SetActive(true);
        switch (pos)
        {
            case TutDialogPosition.Hide:
                objDialog.SetActive(false);
                break;
            case TutDialogPosition.Top:
                objDialog.transform.localPosition = new Vector3(0, 700, 0);
                break;
            case TutDialogPosition.Mid:
                objDialog.transform.localPosition = new Vector3(0, 0, 0);
                break;
            case TutDialogPosition.Bottom:
                objDialog.transform.localPosition = new Vector3(0, -700, 0);
                break;
        }
    }

    void OnTutBtnTriggered(TutBtnHandlerArgs args)
    {
        Debug.Log("OnTutBtnTriggered");

        isBtnTutTriggered = true;
        holeMask.SetTarget(args.btnTransform);

        //0.6秒后停止Acting
        StartCoroutine(TimerTick.StartRealtime(0.6f, () =>
        {
            ActingSystem.Instance.StopActing(this.name);
        }));
    }

    void OnTutBtnClick()
    {
        TutSystem.Instance.OnNextStep();
        holeMask.StopMask();
    }

    void OnFingerAnime(UITutFingerArgs args)
    {
        if (objFinger != null)
        {
            Destroy(objFinger);
            objFinger = null;
        }

        objFinger = Instantiate(prefabFinger, transform);
        objFinger.GetComponent<TutFingerHelper>().Init(args.startPos, args.endPos);
    }

    void OnStopFingerAnime(UITutFingerArgs args)
    {
        if (objFinger != null)
        {
            Destroy(objFinger);
            objFinger = null;
        }
    }

    //玩家点击屏幕, 只有DialogAction类型才会触发下一步
    public void OnMaskClick()
    {
        if (currentTutActionArgs.tutActionType == TutActionType.DIALOG_ACTION && clickCD <= 0)
        {
            TutSystem.Instance.OnNextStep();
            ResetCD();
        }
    }

    void ResetCD()
    {
        clickCD = 1f;
    }

    void Update()
    {
        clickCD -= Time.deltaTime;
    }

    #region Helper
    // void OnHighLightObj(GameObject gameObject)
    // {
    //     highlightGO = gameObject;
    //     Canvas canvas = highlightGO.AddComponent<Canvas>();
    //     canvas.overrideSorting = true;
    //     canvas.sortingOrder = 950;
    // }

    // void OnUnHighLightObj()
    // {
    //     if (highlightGO != null)
    //     {
    //         Destroy(highlightGO.GetComponent<Canvas>());
    //     }
    //     highlightGO = null;
    // }
    #endregion
}
