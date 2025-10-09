using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActing : MonoBehaviour
{
    public GameObject objMask;
    public GameObject objDebugHint;

    void Start()
    {
        EventManager.StartListening<UIActingArgs>(EventNameActing.EVENT_ON_ACTING, OnActing);
        EventManager.StartListening<UIActingArgs>(EventNameActing.EVENT_STOP_ACTING, StopActing);
        objMask.SetActive(false);
        objDebugHint.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIActingArgs>(EventNameActing.EVENT_ON_ACTING, OnActing);
        EventManager.StopListening<UIActingArgs>(EventNameActing.EVENT_STOP_ACTING, StopActing);
    }

    void OnActing(UIActingArgs args)
    {
        Debug.Log("OnActing");
        objMask.SetActive(true);

        objDebugHint.SetActive(GameConfig.debugToolRunTime == DebugTool.On);

    }

    void StopActing(UIActingArgs args)
    {
        objMask.SetActive(false);
        objDebugHint.SetActive(false);
    }
}
