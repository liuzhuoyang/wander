
public static class FooterControl
{
    public static void OnShow()
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_SHOW_FOOTER_UI, null);
    }

    public static void OnHide()
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_HIDE_FOOTER_UI, null);
    }

    public static void OnSelect(string tabName)
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_ON_SELECT_FOOTER_UI, new UIFooterArgs { tabName = tabName });
    }

    public static void OnLockPlay()
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_ON_LOCK_PLAY_FOOTER_UI, null);
    }

    public static void OnUnlockPlay()
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_ON_UNLOCK_PLAY_FOOTER_UI, null);
    }

    public static void OnTick()
    {
        EventManager.TriggerEvent<UIFooterArgs>(EventNameFooter.EVENT_ON_TICK_FOOTER_UI, null);
    }
}

