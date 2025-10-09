using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MsgBase : MonoBehaviour
{
    Action onConfirm;

    public virtual void Init(MsgArgs args)
    {
        this.onConfirm = args.onConfrim;
    }

    public virtual void OnConfirm()
    {
        onConfirm?.Invoke();
        OnClose();
    }

    public void OnClose()
    {
        //callbackClose?.Invoke();
        if (gameObject != null)
            Destroy(gameObject);
    }
}