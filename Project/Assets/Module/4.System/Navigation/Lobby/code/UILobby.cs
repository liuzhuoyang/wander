using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobby : Singleton<UILobby>
{
    [SerializeField] LobbyViewLevel viewLevel;

    [SerializeField] Animator animator;
    [SerializeField] GameObject objBtnPlay;
    [SerializeField] GameObject objBottom;

    void Start()
    {
        EventManager.StartListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_INIT, OnInit);
        EventManager.StartListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_REFRESH_UI, OnRefresh);
        EventManager.StartListening<UILobbyChangeLevelArgs>(LobbyEventName.EVENT_LOBBY_CHANGE_LEVEL, OnChangeLevel);
        EventManager.StartListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_SHOW_BOTTOM, OnShowBottom);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_INIT, OnInit);
        EventManager.StopListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_REFRESH_UI, OnRefresh);
        EventManager.StopListening<UILobbyChangeLevelArgs>(LobbyEventName.EVENT_LOBBY_CHANGE_LEVEL, OnChangeLevel);
        EventManager.StopListening<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_SHOW_BOTTOM, OnShowBottom);
    }

    /// <summary>
    /// 滚动到指定章节
    /// </summary>
    /// <param name="args"></param>
    void OnScroll(UILobbyScrollArgs args)
    {
        /*
        int index = args.chapterID - 1;
        scrollSanp.GoToPanel(index);
        progressView.OnActivePanel(index);
        progressView.Refresh();
        if (args.isUnlockAnimation)
        {
            scrollSanp.Panels[index].GetComponent<LobbyViewProgressSlot>().OnUnlock();
        }*/
    }
    /// <summary>
    /// 展示下方槽位
    /// </summary>
    /// <param name="args"></param>
    void OnShowBottom(UILobbyArgs args)
    {
        objBottom.SetActive(true);
    }

    void OnInit(UILobbyArgs args)
    {
        viewLevel.Init(args.themeName, args.themeVarient, args.selectedLevel, args.totalLevel);
    }

    /// <summary>
    /// 刷新指定的章节
    /// </summary>
    /// <param name="args"></param>
    void OnRefresh(UILobbyArgs args)
    {
        viewLevel.OnRefresh(args.displayName, args.levelID);
    }

    void OnChangeLevel(UILobbyChangeLevelArgs args)
    {
        viewLevel.OnChangeLevel(
            args.isNextLevel, 
            args.selectedLevel, 
            args.totalLevel,
            args.commingThemeName,
            args.commingThemeVarient
            );
    }

    public void OnChangeMode()
    {
        // LobbySystem.Instance.OnChangeMode();
    }
}