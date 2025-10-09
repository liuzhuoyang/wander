using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//表演管理器，用于过场，剧情等待等地方，提示玩家现在进行表演，阻止玩家操作
public class ActingSystem : Singleton<ActingSystem>
{
    // public CanvasGroup uiCanvasGroup;

    public async void Init()
    {
        await UIMain.Instance.CreateStaticPage("acting");
    }

    //进入表演模式
    //传入类名，用于调试
    public void OnActing(string className)
    {
        EventManager.TriggerEvent<UIActingArgs>(EventNameActing.EVENT_ON_ACTING, null);
        Debug.Log($" === ActingManager: On Enter Acting {className} ===");
    }

    //退出表演模式
    public void StopActing(string className)
    {
        EventManager.TriggerEvent<UIActingArgs>(EventNameActing.EVENT_STOP_ACTING, null);
        Debug.Log($" === ActingManager: On Exit Acting {className} ===");
    }
}
