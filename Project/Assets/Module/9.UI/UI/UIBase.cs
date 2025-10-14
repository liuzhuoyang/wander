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
        
        UIMain.Instance.CloseUI(this.transform);
        HeaderControl.OnCloseUIHideHub(this.name);
    }
}
