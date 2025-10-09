using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UIBase : MonoBehaviour
{
    public Action callbackClose;

    public void CloseUI()
    {
        callbackClose?.Invoke();
        callbackClose = null;
        /*
        switch (type)
        {
            case UIPageType.Normal:
                //CloseBlur();
                break;
            case UIPageType.Overlay:
                //CloseOverlay();
                break;
        }*/
        UIMain.Instance.CloseUI(this.transform);
        HeaderControl.OnCloseUIHideHub(this.name);

        if (Game.Instance.fsm.State == GameStates.Home)
        {
            EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
            {
                action = ActionType.OnBackToLobby,
            });
        }
    }
}
