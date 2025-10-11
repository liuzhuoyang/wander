using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInteraction
{
    public class ClickableUI : MonoBehaviour, IPointerClickHandler
    {
        private Action<Vector2> onClickAction;
        public void Init(Action<Vector2> onClickAction)
        {
            this.onClickAction = onClickAction;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onClickAction?.Invoke(eventData.position);
        }
    }
}
