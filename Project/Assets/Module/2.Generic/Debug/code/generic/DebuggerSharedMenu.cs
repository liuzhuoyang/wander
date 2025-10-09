using UnityEngine;

public class DebuggerEventName
{
    public const string EVENT_DEBUGGER_MENU_REFRESH = "EVENT_DEBUGGER_MENU_REFRESH";
}

public class DebuggerSharedMenu : MonoBehaviour
{
    public virtual void Start()
    {
        gameObject.SetActive(GameConfig.debugToolRunTime == DebugTool.On);
        EventManager.StartListening<UIBaseArgs>(DebuggerEventName.EVENT_DEBUGGER_MENU_REFRESH, OnRefresh);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIBaseArgs>(DebuggerEventName.EVENT_DEBUGGER_MENU_REFRESH, OnRefresh);
    }

    void OnEnable()
    {
        gameObject.SetActive(GameConfig.debugToolRunTime == DebugTool.On);
    }

    void OnRefresh(UIBaseArgs args)
    {
        gameObject.SetActive(GameConfig.debugToolRunTime == DebugTool.On);
    }
}
