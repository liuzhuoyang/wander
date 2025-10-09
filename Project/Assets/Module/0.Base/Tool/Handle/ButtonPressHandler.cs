using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetPanel; // 需要隐藏和显示的页面

    // 按下按钮时隐藏页面
    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(false);
            // if (targetPanel.GetComponent<CanvasGroup>() == null)
            // {
            //     targetPanel.AddComponent<CanvasGroup>();
            // }
            // targetPanel.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    // 松开按钮时显示页面
    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetPanel != null)
        {
            targetPanel.SetActive(true);
            // targetPanel.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
