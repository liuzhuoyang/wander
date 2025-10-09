using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInteraction
{
    public class PlayerInputControl : MonoBehaviour
    {
        private InputSystem_Actions.BattleActions battleActions;
        private Camera mainCam;
        private Vector2 pointerScrPos;
        private IInteractable holdingInteractable;

        public Vector2 m_pointerWorldPos => mainCam.ScreenToWorldPoint(pointerScrPos);

        #region 生命周期
        void Start()
        {
            mainCam = Camera.main;
        }
        private void OnEnable()
        {
            battleActions = new InputSystem_Actions().Battle;
            battleActions.TouchPress.performed += OnFingerDown;
            battleActions.TouchPress.canceled += OnFingerUp;
            battleActions.TouchPosition.performed += OnFingerMove;
            battleActions.Enable();
        }
        private void OnDisable()
        {
            battleActions.TouchPress.performed -= OnFingerDown;
            battleActions.TouchPress.canceled -= OnFingerUp;
            battleActions.TouchPosition.performed -= OnFingerMove;
            battleActions.Disable();
        }
        void Update()
        {
            if (holdingInteractable != null)
            {
                holdingInteractable.HoldingUpdate(this);
            }
        }
        #endregion

        public void ReleaseCurrentHolding()=> ClearHoldingInteractable();
        public void HoldingInteractable(IInteractable interactable)
        {
            if (holdingInteractable != null)
            {
                Debug.LogError($"last holding {holdingInteractable.gameObject.name} is not corretly released!!!");
                return;
            }
            holdingInteractable = interactable;
        }
        protected void ClearHoldingInteractable()
        {
            if (holdingInteractable != null)
            {
                var holding = holdingInteractable;
                holdingInteractable = null;
                holding.OnRelease(this);
            }
        }

        #region Input Control
        void EnableInput()
        {
            battleActions.Enable();
        }
        void DisableInput()
        {
            battleActions.Disable();
        }
        void OnFingerDown(InputAction.CallbackContext context)
        {
            if (PlayerInputService.IsPointerOverUI(pointerScrPos))
                return;

            RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(pointerScrPos), Vector2.zero, 100, 1 << PlayerInputService.InteractableLayer);
            if (hit.collider != null)
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (interactable.m_interactable)
                        interactable.OnInteract(this);
                    else
                        interactable.OnFailInteract(this);
                }
            }
        }
        void OnFingerUp(InputAction.CallbackContext context)
        {
            ClearHoldingInteractable();
        }
        void OnFingerMove(InputAction.CallbackContext context)
        {
            pointerScrPos = context.ReadValue<Vector2>();
        }
        #endregion
    }
}