using GameBasic.Event;

namespace PlayerInteraction
{
    using PlayerInteraction.Event;
    public class PlayerInputManager : Singleton<PlayerInputManager>
    {
        public PlayerInputControl m_currentPlayerInput { get; private set; }

        private bool isInTransition;

        public bool m_canControl => !isInTransition;
        protected override void Awake()
        {
            base.Awake();
            EventHandler.E_AfterLoadScene += FindPlayerInput;
            PlayerInputEvent.E_OnFlashPlayerInput += FlashInput;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventHandler.E_AfterLoadScene -= FindPlayerInput;
            PlayerInputEvent.E_OnFlashPlayerInput -= FlashInput;
        }
        void Start()
        {
            FindPlayerInput();
        }
        void FindPlayerInput()
        {
            m_currentPlayerInput = FindAnyObjectByType<PlayerInputControl>();
        }
        void FlashInput()
        {
            m_currentPlayerInput?.ReleaseCurrentHolding();
        }
    }
}
