using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgTimeout : MsgBase
{
    void Start()
    {

    }

    public override void Init(MsgArgs args)
    {
        base.Init(args);
    }

    public void OnReconnect()
    {
        Game.Instance.Restart();
        InitManager.Instance.isTimeOut = false;
    }

    public void OnContinue()
    {
        base.OnConfirm();
    }
}
