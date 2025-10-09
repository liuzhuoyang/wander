using UnityEngine;


/// <summary>
/// 气泡系统
/// </summary>
public class BubbleSystem : Singleton<BubbleSystem>
{
    UIBubble uIBubble;
    public void Init()
    {
        uIBubble = UIBubble.Instance;
        uIBubble.Init();
    }

    //在制定位置创建气泡
    public void CreateBubble(Vector2 position)
    {
        uIBubble.CreateBubble(position);
    }

    //在指定目标位置和当前位置创建指示气泡
    public void CreateIndicatorBubble(Vector2 targetPos, Vector2 currentPos)
    {
        uIBubble.CreateIndicatorBubble(targetPos, currentPos);
    }

    //在指定目标位置和当前位置创建指示气泡
    // public void CreateIndicatorBubble(GameObject targetObj, Vector2 currentPos)
    // {
    //     // 检查目标是否在屏幕内
    //     if (CameraManager.Instance.CheckIsOnScreen(targetObj.transform.position))
    //     {
    //         return;
    //     }
    //     uIBubble.CreateIndicatorBubble(targetObj, currentPos);
    // }

    //移除指定的气泡
    public void RemoveBubble(BubbleSlot bubbleSlot)
    {
        uIBubble.RemoveBubble(bubbleSlot);
    }
}