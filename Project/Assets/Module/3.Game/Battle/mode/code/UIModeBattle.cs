using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModeBattle : UIMode
{
    public Transform groupInactive;
    public Transform groupActive;

    Transform selectedPage;

    private void OnDestroy()
    {
        Deactivate();
    }

    public override void Deactivate()
    {
        EventManager.StopListening<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_SELECT_UI, OnSelect);
        EventManager.StopListening<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_CLOSE_UI, OnClose);
    }

    public override void Activate()
    {
        gameObject.SetActive(true);
        EventManager.StartListening<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_SELECT_UI, OnSelect);
        EventManager.StartListening<UIModeBattleArgs>(EventNameModeBattle.EVENT_BATTLE_ON_CLOSE_UI, OnClose);
        groupInactive.gameObject.SetActive(false);
    }

    public void OnSelect(UIModeBattleArgs args)
    {
        string pageName = "ui_mode_battle_" + args.targetPage;

        gameObject.SetActive(true);

        if (selectedPage != null)
        {
            if (selectedPage.name == pageName) return;
            selectedPage.SetParent(groupInactive);
        }

        selectedPage = groupInactive.Find(pageName);
        if (selectedPage == null)
        {
            Debug.LogError("=== UIModeBattle: selected page is null, page name = " + pageName);
        }

        selectedPage.SetParent(groupActive);
    }

    public void OnClose(UIModeBattleArgs args)
    {
        if (selectedPage != null)
        {
            selectedPage.SetParent(groupInactive);
            selectedPage = null;
        }
    }
}
