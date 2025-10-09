using UnityEngine;

public static class UIBackgroundControl
{
    static string currentThemeName;

    public static void OnShowUIBackground()
    {
        EventManager.TriggerEvent<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_SHOW, new UIBackgroundArgs() {});
    }

    public static void OnHideUIBackground()
    {
        EventManager.TriggerEvent<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_HIDE, new UIBackgroundArgs() {});
    }

    public static void OnRefreshUIBackground(string themeName)
    {
        // 如果当前主题与新主题相同，则不刷新
        if (currentThemeName == themeName) return;

        currentThemeName = themeName;
        EventManager.TriggerEvent<UIBackgroundArgs>(UIBackgroundEventName.UI_BG_REFRESH, new UIBackgroundArgs() { themeName = themeName });
    }
}
