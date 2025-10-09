using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipType
{
    Text,
    Unlock,
    Reward,
    HubText,
    PowerChange,    //玩家战力变化
    TokenInfo,
}

public class TipManager : Singleton<TipManager>
{
    public void Init()
    {

    }

    public void OnTip(string content)
    {
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs() { tipType = TipType.Text, textTip = content });
    }

    // public void OnTipUnlock(Vector2 pos, string textTitle, string textContent)
    // {
    //     EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
    //     {
    //         tipType = TipType.Unlock,
    //         posX = pos.x,
    //         posY = pos.y,
    //         textTipUnlockTitle = textTitle,
    //         textTipUnlockContent = textContent,
    //     });
    // }

    public void OnTipUnlock(string textContent)
    {
        EventManager.TriggerEvent(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
        {
            tipType = TipType.Unlock,
            textTip = textContent,
        });
    }

    public void OnTipReward(Vector2 pos, List<RewardArgs> listRewardArgs)
    {
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
        {
            tipType = TipType.Reward,
            posX = pos.x,
            posY = pos.y,
            rewardArgs = listRewardArgs
        });
    }

    public void OnTipHubText(Vector2 pos, string textContent)
    {
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
        {
            tipType = TipType.HubText,
            posX = pos.x,
            posY = pos.y,
            textTip = textContent
        });
    }

    public void OnTipPowerChange(int numberOrigin, int numberResult)
    {
        if (numberOrigin == numberResult) return;
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
        {
            tipType = TipType.PowerChange,
            numberOrigin = numberOrigin,
            numberResult = numberResult,
        });
    }

    public void OnTipHubTokenInfo(Vector2 pos)
    {
        EventManager.TriggerEvent<UITipArgs>(EventNameTip.EVENT_TIP_ON_UI, new UITipArgs()
        {
            tipType = TipType.TokenInfo,
            posX = pos.x,
            posY = pos.y,
        });
    }
}
