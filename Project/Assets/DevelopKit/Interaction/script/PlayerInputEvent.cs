using System;

namespace PlayerInteraction.Event
{
    public static class PlayerInputEvent
    {
        public static Action E_OnFlashPlayerInput;
        public static void Call_OnFlashPlayerInput()=>E_OnFlashPlayerInput?.Invoke();
    }
}