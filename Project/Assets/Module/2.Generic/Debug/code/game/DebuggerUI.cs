using UnityEngine;
using System.Collections.Generic;

public class DebuggerUI : MonoBehaviour
{
    public void OnDebugLogUIDynamicTarget()
    {
        foreach (var item in UIDynamicControl.Instance.uiTargetDict)
        {
            Debug.Log(item.Key + " " + item.Value.position);
        }
    }


    public void OnDebugTipPower()
    {
        Debugger.Instance.OnCloseDebug();
        PowerSystem.Instance.OnDebugTipPower();
    }

    public void OnDebugTooltipPointerDown()
    {
        Debugger.Instance.OnCloseDebug();
        Vector2 posScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        TooltipManager.Instance.OnTooltipItemHub(new List<RewardArgs>(){
            new RewardArgs()
            {
                reward = "item_currency_coin",
                num = 100
            },
            new RewardArgs()
            {
                reward = "item_currency_gem",
                num = 50
            }
        }, posScreenCenter, Direction.Down);
    }

    public void OnDebugTooltipPointerUp()
    {
        Debugger.Instance.OnCloseDebug();
        Vector2 posScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        TooltipManager.Instance.OnTooltipItemHub(new List<RewardArgs>(){
            new RewardArgs()
            {
                reward = "item_currency_coin",
                num = 100
            },
            new RewardArgs()
            {
                reward = "item_currency_gem",
                num = 50
            }
        }, posScreenCenter, Direction.Up);
    }
}
