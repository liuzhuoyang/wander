using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInteraction
{
    public class DraggableUI : MonoBehaviour, IInteractableUI
    {
        [SerializeField] private RectTransform rectTransform;

        private bool isDragging = false;
        private Vector2 clickPos;

        public event Action<Vector2> onEndDrag;
        public event Action onClick;
        public event Action onBeginDrag;

        public bool m_isDragging => isDragging;

        private const float MIN_DRAG_OFFSET = 10f;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos = eventData.position;

            if (isDragging)
            {
                Vector2 mousePos = eventData.position;
                rectTransform.position = new Vector3(mousePos.x, mousePos.y, rectTransform.position.z);
            }
            else
            {
                if (Vector2.Distance(pos, clickPos) > MIN_DRAG_OFFSET)
                {
                    Debug.Log($"开始拖拽 {gameObject.name}");
                    isDragging = true;
                    onBeginDrag?.Invoke();
                }
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log($"开始抓取 {gameObject.name}");
            clickPos = eventData.position;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                Debug.Log($"放置 Gears {gameObject.name}");
                isDragging = false;
                onEndDrag?.Invoke(eventData.position);
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isDragging)
            {
                Debug.Log($"点击 {gameObject.name}");
                onClick?.Invoke();
            }
        }
      
    }
}