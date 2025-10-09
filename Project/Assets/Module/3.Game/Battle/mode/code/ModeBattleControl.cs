
public static class ModeBattleControl
{
    public static void OnOpen(string targetPage)
    {
        EventManager.TriggerEvent<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_SELECT_UI, new UIModeBattleArgs { targetPage = targetPage });
    }

    public static void OnCloseActive()
    {
        EventManager.TriggerEvent<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_CLOSE_UI, null);
    }
}

