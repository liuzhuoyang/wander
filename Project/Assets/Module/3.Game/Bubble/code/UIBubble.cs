using System.Collections.Generic;
using UnityEngine;

public class UIBubble : Singleton<UIBubble>
{
    [SerializeField] GameObject bubbleSlot;

    private List<BubbleSlot> activeIndicators;
    public void Init()
    {
        activeIndicators = new List<BubbleSlot>();
    }
    public void CreateBubble(Vector2 position)
    {
        GameObject bubble = Instantiate(bubbleSlot, transform);
        bubble.transform.position = position;
    }

    public void CreateIndicatorBubble(Vector2 targetPos, Vector2 currentPos)
    {
        GameObject bubble = Instantiate(bubbleSlot, transform);
        bubble.transform.position = targetPos;
        bubble.GetComponent<BubbleSlot>().Initialize(targetPos, currentPos, 3f);
        activeIndicators.Add(bubble.GetComponent<BubbleSlot>());
    }

    // public void CreateIndicatorBubble(GameObject targetObj, Vector2 currentPos)
    // {
    //     GameObject bubble = Instantiate(bubbleSlot, transform);
    //     bubble.transform.position = targetObj.transform.position;
    //     bubble.GetComponent<BubbleSlot>().Initialize(targetObj, currentPos);
    //     activeIndicators.Add(bubble.GetComponent<BubbleSlot>());
    // }

    public void RemoveBubble(BubbleSlot bubbleSlot)
    {
        if (activeIndicators.Contains(bubbleSlot))
        {
            activeIndicators.Remove(bubbleSlot);
            Destroy(bubbleSlot.gameObject);
        }
    }
}