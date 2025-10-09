using UnityEngine;

public static class MessageControl
{
    public static void OnShowMessage<T>(T args) where T : MsgArgs
    {
        // 触发事件，弹出相应的UI
        EventManager.TriggerEvent<T>(EventNameMsg.EVENT_MESSAGE_UI, args);
    }
}
