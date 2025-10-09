using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Button))]
public class ButtonSingleTouchBlocker : MonoBehaviour, IPointerDownHandler
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //引导模式下不判断
        if (TutSystem.Instance.isOnTutorial) return;
        int activeFingerCount = Touch.activeTouches.Count;

        if (activeFingerCount > 1)
        {
            // 暂时禁用按钮
            button.interactable = false;
        }
    }

    void Update()
    {
        //引导模式下不判断
        if (TutSystem.Instance == null || TutSystem.Instance.isOnTutorial) return;
        if (Touch.activeTouches.Count <= 1 && !button.interactable)
        {
            button.interactable = true;
        }
    }
}
