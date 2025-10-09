using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransition : UIBase
{
    public Animator animator;
    void Awake()
    {
        EventManager.StartListening<UITransitArgs>(EventNameTransit.EVENT_TRANSITION_OPEN_UI, OnOpen);
        EventManager.StartListening<UITransitArgs>(EventNameTransit.EVENT_TRANSITION_CLOSE_UI, OnClose);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UITransitArgs>(EventNameTransit.EVENT_TRANSITION_OPEN_UI, OnOpen);
        EventManager.StopListening<UITransitArgs>(EventNameTransit.EVENT_TRANSITION_CLOSE_UI, OnClose);
    }

    void OnOpen(UITransitArgs args)
    {
        
    }

    void OnClose(UITransitArgs args)
    {
        animator.SetTrigger("OnHide");
        //base.CloseUI();
    }

    //动画事件调用
    public void OnCloseComplete()
    {
        base.CloseUI();
    }

    public void OnSFXIn()
    {
        AudioControl.Instance.PlaySFX("sfx_ui_transit_in");
    }

    public void OnSFXOut()
    {
        AudioControl.Instance.PlaySFX("sfx_ui_transit_out");
    }   
}
