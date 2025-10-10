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
            PlayerInputEvent.E_OnFlashPlayerInput += FlashInput;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerInputEvent.E_OnFlashPlayerInput -= FlashInput;
        }
        internal void RegisterInput(PlayerInputControl input)
        {
            m_currentPlayerInput = input;
        }
        internal void UnregisterInput(PlayerInputControl input)
        {
            if(m_currentPlayerInput == input)
                m_currentPlayerInput = null;
        }
        void FlashInput()
        {
            m_currentPlayerInput?.ReleaseCurrentHolding();
        }
    }
}
