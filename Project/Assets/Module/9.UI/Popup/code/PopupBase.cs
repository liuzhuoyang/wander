using UnityEngine;
using System;

public class PopupBase : MonoBehaviour
{
    Action closeCallBack;

    public virtual void OnOpen<T>(T args)
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClose()
    {
        gameObject.SetActive(false);

        EventManager.TriggerEvent<GPTriggerArgs>(GPTriggerEventName.EVENT_UI_CLOSED, null);

        closeCallBack?.Invoke();

        if (Game.Instance.fsm.State == GameStates.Home)
        {
            EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
            {
                action = ActionType.OnBackToLobby,
            });
        }

        Destroy(gameObject);
    }

    public virtual void SetCloseCallBack(Action action)
    {
        closeCallBack = action;
    }
}
