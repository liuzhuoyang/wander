using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayerInteraction
{
    public interface IInteractable
    {
        GameObject gameObject { get; }
        bool m_interactable { get; }
        void OnInteract(PlayerInputControl inputControl);
        void OnRelease(PlayerInputControl inputControl);
        void OnFailInteract(PlayerInputControl inputControl);
        void HoldingUpdate(PlayerInputControl inputControl);
    }
    public interface IInteractable_UI : IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {}
}