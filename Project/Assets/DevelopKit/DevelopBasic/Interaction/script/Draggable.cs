using UnityEngine;
using System;

namespace PlayerInteraction
{
    public class Draggable : MonoBehaviour, IInteractable
    {
        [SerializeField] protected Transform dragRoot;
        [SerializeField] protected Collider2D hitbox;

        public event Action<Vector2> onEndDrag;
        public event Action onClick;
        public event Action onBeginDrag;

        protected bool isDragging = false;
        protected Vector2 dragOffset = Vector2.zero;
        protected Vector2 clickPos;

        public bool m_interactable => true;

        protected const float MIN_DRAG_OFFSET = 0.1f;

        void Reset()
        {
            hitbox = GetComponent<Collider2D>();
        }
        void Start()
        {
            if (dragRoot == null)
                dragRoot = transform;
        }
        public virtual void OnInteract(PlayerInputControl playerInput)
        {
            Debug.Log($"开始交互 {gameObject.name}");
            clickPos = playerInput.m_pointerWorldPos;
            playerInput.HoldingInteractable(this);
        }

        public virtual void OnFailInteract(PlayerInputControl playerInput)
        {
            Debug.Log($"当前物体 {gameObject.name} 无法交互");
        }

        public void OnRelease(PlayerInputControl playerInput)
        {
            if (isDragging)
            {
                Debug.Log($"放置物体 {gameObject.name}");
                onEndDrag?.Invoke(playerInput.m_pointerWorldPos);
            }
            else
            {
                Debug.Log($"点击物体 {gameObject.name}");
                onClick?.Invoke();
            }
            isDragging = false;
        }
        public virtual void HoldingUpdate(PlayerInputControl playerInput)
        {
            Vector2 mousePos = playerInput.m_pointerWorldPos;
            if (!isDragging)
            {
                if ((mousePos - clickPos).sqrMagnitude >= MIN_DRAG_OFFSET * MIN_DRAG_OFFSET)
                {
                    Debug.Log($"开始拖拽 {gameObject.name}");
                    isDragging = true;
                    onBeginDrag?.Invoke();
                    return;
                }
            }
            else
            {
                dragRoot.position = new Vector3(mousePos.x, mousePos.y, dragRoot.position.z) + (Vector3)dragOffset;
            }
        }
    }
}