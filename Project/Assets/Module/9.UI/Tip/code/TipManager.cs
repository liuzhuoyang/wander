using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipType
{
    Generic, //普通提示，出现在屏幕中央，只有一行Text
    Frame,  //气泡提示，出现在指定位置，有一个框和箭头指向
    Custom, //自定义类型提示
}

public class TipManager : Singleton<TipManager>
{
    public void Init()
    {

    }

    public void OnTip(string content)
    {
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs() { 
            tipType = TipType.Generic, 
            textTip = content 
        });
    }

    public void OnFrameTip(Vector2 pos, string textContent)
    {
        EventManager.TriggerEvent<UITipFrameArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipFrameArgs()
        {
            tipType = TipType.Frame,
            posX = pos.x,
            posY = pos.y,
            textTip = textContent
        });
    }

    public void OnCustomTip(UITipCustomArgs args)
    {
        args.tipType = TipType.Custom;
        EventManager.TriggerEvent<UITipCustomArgs>(EventNameTip.EVENT_TIP_ON_UI,args);
    }
}
