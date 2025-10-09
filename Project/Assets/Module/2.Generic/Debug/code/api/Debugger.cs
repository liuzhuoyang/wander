using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : Singleton<Debugger>
{
    public GameObject objBtnDebug;
    bool isHideButton;

    public GameObject objDebugPanel;
    public GameObject objClose;

    public Transform transTab;
    public Transform transGroup;
    public GameObject objItemList;

    public Dictionary<string, GameObject> dictTab;
    public Dictionary<string, GameObject> dictGroup;

    public DebuggerPlot debuggerGPEvent;
    public DebuggerChapter debuggerChapter;
    public DebuggerSystem debuggerSystem;

    public void OnMaxDebug()
    {
        //MaxSdk.ShowMediationDebugger();
    }

    public void OnIAPDebug()
    {
        //ClassicIAPControl.Instance.Init();
    }
    public void Init()
    {
        dictTab = new Dictionary<string, GameObject>();
        dictGroup = new Dictionary<string, GameObject>();

        dictTab.Add("api", transTab.Find("tab_api").Find("on").gameObject);
        dictTab.Add("server", transTab.Find("tab_server").Find("on").gameObject);
        dictTab.Add("analytics", transTab.Find("tab_analytics").Find("on").gameObject);
        dictTab.Add("game", transTab.Find("tab_game").Find("on").gameObject);

        dictGroup.Add("api", transGroup.Find("api").gameObject);
        dictGroup.Add("server", transGroup.Find("server").gameObject);
        dictGroup.Add("analytics", transGroup.Find("analytics").gameObject);
        dictGroup.Add("game", transGroup.Find("game").gameObject);

        objItemList.SetActive(false);

        objDebugPanel.SetActive(false);
        objClose.SetActive(false);
    }

    public void OnHideDebugger()
    {
        gameObject.SetActive(false);
    }

    public void OnShowDebugger()
    {
        gameObject.SetActive(true);
    }

    public void OnDebug()
    {
        if (isHideButton)
        {
            OnShowButton();
            return;
        }

        //进入暂停状态机
        Game.Instance.OnChangeState(GameStates.Pause);

        objDebugPanel.SetActive(true);
        objClose.SetActive(true);
        debuggerGPEvent.Init();
        debuggerSystem.Init();
        debuggerChapter.Init();
        OnTabGame();
    }

    public void OnCloseDebug()
    {
        objDebugPanel.SetActive(false);
        objClose.SetActive(false);

        //进入Home状态
        Game.Instance.OnChangeState(GameStates.Home);
    }

    public void OnTabAPI()
    {
        Reset();
        dictTab["api"].gameObject.SetActive(true);
        dictGroup["api"].gameObject.SetActive(true);
    }

    public void OnTabServer()
    {
        Reset();
        dictTab["server"].gameObject.SetActive(true);
        dictGroup["server"].gameObject.SetActive(true);
    }

    public void OnTabGame()
    {
        Reset();
        dictTab["game"].gameObject.SetActive(true);
        dictGroup["game"].gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();
    }
    public void OnTabAnalytics()
    {
        Reset();
        dictTab["analytics"].gameObject.SetActive(true);
        dictGroup["analytics"].gameObject.SetActive(true);
    }

    private void Reset()
    {
        foreach (string key in dictTab.Keys)
        {
            dictTab[key].gameObject.SetActive(false);
        }

        foreach (string key in dictGroup.Keys)
        {
            dictGroup[key].gameObject.SetActive(false);
        }
    }

    public void OnHideButton()
    {
        objBtnDebug.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        isHideButton = true;
        OnCloseDebug();
    }

    void OnShowButton()
    {
        objBtnDebug.GetComponent<Image>().color = new Color(1, 0.3f, 0.3f, 1f);
        isHideButton = false;
    }
}
