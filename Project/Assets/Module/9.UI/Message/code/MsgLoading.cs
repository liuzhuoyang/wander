using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgLoading : MsgBase
{
    private void OnDestroy()
    {
        EventManager.StopListening<MsgArgs>(EventNameMsg.EVENT_MESSAGE_CLOSE_LOADING_UI, OnEventClose);
    }
    /*
    public void Init()
    {
        EventManager.StartListening(EventName.EVENT_CLOSE_MESSAGE_LOADING_UI, OnEventClose);
    }
    */
    public override void Init(MsgArgs args)
    {
        base.Init(args);
        EventManager.StartListening<MsgArgs>(EventNameMsg.EVENT_MESSAGE_CLOSE_LOADING_UI, OnEventClose);
    }

    void OnEventClose(MsgArgs e)
    {
        Debug.Log("=== MsgLoading: kill loading page ===");
        base.OnClose();
    }
}
