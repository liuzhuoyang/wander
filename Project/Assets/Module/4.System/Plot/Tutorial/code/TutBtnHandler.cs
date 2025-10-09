using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TutBtnHandler : MonoBehaviour
{
    public string btnEventKey;
    GameObject objPointer;
    //按钮上已经有重写层级了
    // bool hasCanvas = false;
    // bool originIsOverrideSorting;
    // int originSortingOrder;

    Button.ButtonClickedEvent originEvent;
    Action onClickAction;
    Transform uiRootTransform;
    private void Start()
    {
        EventManager.StartListening<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGER_UI, OnBtnTrigger);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGER_UI, OnBtnTrigger);
    }

    void OnBtnTrigger(TutBtnHandlerArgs args)
    {
        if (args.btnEventKey != this.btnEventKey) return;

        onClickAction = args.onClickAction;
        uiRootTransform = args.uiRootTransform;

        // Canvas canvas;
        // if (gameObject.GetComponent<Canvas>() != null)
        // {
        //     canvas = gameObject.GetComponent<Canvas>();
        //     hasCanvas = true;
        //     originIsOverrideSorting = canvas.overrideSorting;
        //     originSortingOrder = canvas.sortingOrder;
        // }
        // else
        // {
        //     canvas = gameObject.AddComponent<Canvas>();
        //     gameObject.AddComponent<GraphicRaycaster>();
        // }

        // canvas.overrideSorting = true;
        // canvas.sortingOrder = 1001;

        originEvent = gameObject.GetComponent<Button>().onClick;
        gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        gameObject.GetComponent<Button>().onClick.AddListener(OnContinue);
        //刷新
        // gameObject.SetActive(false);
        // gameObject.SetActive(true);
        if (args.isCreatePointer)
        {
            OnCreatePointer();
        }

        //发送事件表示已经成功触发按钮教程
        EventManager.TriggerEvent<TutBtnHandlerArgs>(TutEventName.EVENT_ON_TUT_BTN_TRIGGERED_UI, new TutBtnHandlerArgs()
        {
            btnEventKey = btnEventKey,
            btnTransform = gameObject.transform as RectTransform
        });
    }

    //创建指引icon
    async void OnCreatePointer()
    {
        objPointer = Instantiate(await GameAsset.GetPrefabAsync("tut_finger"), uiRootTransform);
        objPointer.transform.position = new Vector2(transform.position.x + 50f, transform.position.y + 150f);

        objPointer.GetComponent<TutFingerHelper>().Init(false, 0.5f);
    }

    void OnContinue()
    {
        // 这个按钮有可能会出现两个按钮同时按的情况,如果同时按则返回
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1)
        {
            Debug.Log("=== OnContinue Return === ");
            return;
        }

        // originEvent?.Invoke();
        onClickAction?.Invoke();

        // if (hasCanvas)
        // {
        //     gameObject.GetComponent<Canvas>().overrideSorting = originIsOverrideSorting;
        //     gameObject.GetComponent<Canvas>().sortingOrder = originSortingOrder;
        // }
        // else
        // {
        //     Destroy(gameObject.GetComponent<GraphicRaycaster>());
        //     Destroy(gameObject.GetComponent<Canvas>());
        // }
        Destroy(objPointer);

        gameObject.GetComponent<Button>().onClick.RemoveListener(OnContinue);
        gameObject.GetComponent<Button>().onClick = originEvent;
        originEvent?.Invoke();
        originEvent = null;
        

        Debug.Log("=== TutHandlerBase: on click continue ===");
    }
}
