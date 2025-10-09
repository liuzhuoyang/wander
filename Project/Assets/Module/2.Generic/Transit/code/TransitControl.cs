using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class TransitControl
{
    public static   void Init()
    {

    }

    public static async UniTask OnTransit(Action callback = null)
    {
        await UIMain.Instance.OpenUI("transit", UIPageType.Overlay);
        await UniTask.Delay(500);
        callback?.Invoke();
    }

    public static void CloseTransit()
    {
        EventManager.TriggerEvent<UITransitArgs>(EventNameTransit.EVENT_TRANSITION_CLOSE_UI, null);
    }
}
