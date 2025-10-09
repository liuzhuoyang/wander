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

    // 显示文字提示，传入文本列表（每行一个文本）,入口按钮的RectTransform, 提示显示的方向
    // centerPoint需要独立传入，因为有时候图片的pivoit point在边缘，传入这个centerPoint用于箭头位置
    public void ShowTooltipText(List<string> contentList, RectTransform entryTransform, Vector2 centerPoint, Direction direction)
    {
        TooltipContentArgs contentArgs = new TooltipContentArgs();
        contentArgs.contentList = contentList;

        TooltipPosArgs posArgs = new TooltipPosArgs();
        posArgs.entryTransform = entryTransform;
        posArgs.centerPoint = centerPoint;
        posArgs.direction = direction;

        ShowTooltip(new TooltipArgs
        {
            tooltipType = TooltipType.TEXT,
            contentArgs = contentArgs,
            posArgs = posArgs
        });
    }

    // 显示图片提示
    public void ShowTooltipImage(List<string> imageNameList, RectTransform entryTransform, Vector2 centerPoint, Direction direction)
    {
        TooltipContentArgs contentArgs = new TooltipContentArgs();
        contentArgs.imageNameList = imageNameList;

        TooltipPosArgs posArgs = new TooltipPosArgs();
        posArgs.entryTransform = entryTransform;
        posArgs.centerPoint = centerPoint;
        posArgs.direction = direction;

        ShowTooltip(new TooltipArgs
        {
            tooltipType = TooltipType.IMAGE,
            contentArgs = contentArgs,
            posArgs = posArgs
        });
    }

    // 显示物品奖励提示
    public void ShowTooltipItemReward(List<RewardArgs> itemRewardList, RectTransform entryTransform, Vector2 centerPoint, Direction direction)
    {
        TooltipContentArgs contentArgs = new TooltipContentArgs();
        contentArgs.itemRewardList = itemRewardList;

        TooltipPosArgs posArgs = new TooltipPosArgs();
        posArgs.entryTransform = entryTransform;
        posArgs.centerPoint = centerPoint;
        posArgs.direction = direction;

        ShowTooltip(new TooltipArgs
        {
            tooltipType = TooltipType.ITEMREWARD,
            contentArgs = contentArgs,
            posArgs = posArgs
        });
    }

    //显示道具详情
    public void ShowTooltipItemInfo(string itemName, RectTransform entryTransform, Vector2 centerPoint, Direction direction)
    {
        TooltipContentArgs contentArgs = new TooltipContentArgs();
        contentArgs.itemName = itemName;

        TooltipPosArgs posArgs = new TooltipPosArgs();
        posArgs.entryTransform = entryTransform;
        posArgs.centerPoint = centerPoint;
        posArgs.direction = direction;

        ShowTooltip(new TooltipArgs
        {
            tooltipType = TooltipType.ITEMINFO,
            contentArgs = contentArgs,
            posArgs = posArgs
        });
    }

    void ShowTooltip(TooltipArgs args)
    {
        EventManager.TriggerEvent<UITooltipArgs>(EventNameTooltip.EVENT_OPEN_UI, new UITooltipArgs { tooltipArgs = args });
    }

    public void CloseTooltip()
    {
        EventManager.TriggerEvent<UITooltipArgs>(EventNameTooltip.EVENT_CLOSE_UI, null);
    }

    #region 工具方法
    //根据按钮的大小和位置计算小提示窗口的位置
    public static Vector3 CalculateTooltipPos(TooltipPosArgs posArgs, int width, int height)
    {
        //框与对象之间的间距，如果为0，框的下边缘会贴着目标对象的上边缘
        int offsetY = 30;
        int offsetX = 30;
        Vector3 targetPosition = posArgs.entryTransform.position;
        switch (posArgs.direction)
        {
            case Direction.Top:
                targetPosition += new Vector3(0, posArgs.entryTransform.sizeDelta.y / 2f + height / 2 + offsetY, 0);
                break;
            case Direction.Bottom:
                targetPosition += new Vector3(0, -posArgs.entryTransform.sizeDelta.y / 2f - height / 2 - offsetY, 0);
                break;
            case Direction.Left:
                targetPosition += new Vector3(-posArgs.entryTransform.sizeDelta.x / 2f - width / 2 - offsetX, 0, 0);
                break;
            case Direction.Right:
                targetPosition += new Vector3(posArgs.entryTransform.sizeDelta.x / 2f + width / 2 + offsetX, 0, 0);
                break;
        }
        return targetPosition;
    }

    public static void CalculateArrowPos(TooltipPosArgs posArgs, int width, int height, GameObject objArrow, Vector3 pos)
    {
        int arrowOffsetY = 8;
        switch (posArgs.direction)
        {
            case Direction.Top:
                objArrow.transform.position = new Vector3(posArgs.centerPoint.x, pos.y - height / 2 + arrowOffsetY, 0);
                break;
            case Direction.Bottom:
                // 计算箭头的位置，确保紧贴下边缘
                objArrow.transform.position = new Vector3(posArgs.centerPoint.x, pos.y + height / 2 - arrowOffsetY, 0);
                objArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.Left:
                objArrow.transform.position = new Vector3(pos.x - width / 2, pos.y, 0);
                objArrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Right:
                objArrow.transform.position = new Vector3(pos.x + width / 2, pos.y, 0);
                objArrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    //限制位置在屏幕内
    // public static void ClampToScreen(RectTransform rectTransform)
    // {
    //     Vector2 pos = rectTransform.position;
    //     Vector2 size = rectTransform.sizeDelta;

    //     // 计算边界
    //     float minX = size.x / 2 + 20;
    //     float maxX = Screen.width - size.x / 2 - 20;
    //     float minY = size.y / 2 + 20;
    //     float maxY = Screen.height - size.y / 2 - 20;

    //     // 限制位置
    //     pos.x = Mathf.Clamp(pos.x, minX, maxX);
    //     pos.y = Mathf.Clamp(pos.y, minY, maxY);

    //     rectTransform.transform.position = pos;
    // }
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
