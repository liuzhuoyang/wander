using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用小提示管理器，提供文字、图片、物品奖励提示，用于UI使用
/// </summary>
public class TooltipManager : Singleton<TooltipManager>
{
    public void Init()
    {

    }

    public void ShowTooltipText(List<string> contentList, RectTransform entryTransform, Vector2 centerPoint, Direction direction)
    {
        
    }

    // 显示物品奖励提示
    public void OnTooltipItemHub(List<RewardArgs> listRewardArgs, Vector2 pos,  Direction direction)
    {
        ShowTooltip(new TooltipItemHubArgs
        {
            tooltipType = TooltipType.ItemHub,
            posX = pos.x,
            posY = pos.y,
            direction = direction,
            listRewardArgs = listRewardArgs
        });
    }

    void ShowTooltip(UITooltipArgs args)
    {
        EventManager.TriggerEvent<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_OPEN_UI, args);
    }

    public void CloseTooltip()
    {
        EventManager.TriggerEvent<UITooltipArgs>(EventNameTooltip.EVENT_TOOLTIP_CLOSE_UI, null);
    }

    #region 工具方法
    public static void ClampToScreen(RectTransform rectTransform)
    {
        Vector2 pos = rectTransform.position;
        Vector2 size = Vector2.Scale(rectTransform.sizeDelta, rectTransform.lossyScale); // 考虑缩放
        Vector2 pivot = rectTransform.pivot;

        float minX = size.x * pivot.x + 20;
        float maxX = Screen.width - size.x * (1 - pivot.x) - 20;
        float minY = size.y * pivot.y + 20;
        float maxY = Screen.height - size.y * (1 - pivot.y) - 20;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rectTransform.position = pos;
    }
    #endregion
}
