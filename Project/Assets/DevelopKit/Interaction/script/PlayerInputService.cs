using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PlayerInteraction
{
    public static class PlayerInputService
    {
        public static int InteractableLayer = LayerMask.NameToLayer("Interactable");
        
        //判断当前位置是否处于UI下
        public static bool IsPointerOverUI(Vector2 screenPosition)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = screenPosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
    }
}