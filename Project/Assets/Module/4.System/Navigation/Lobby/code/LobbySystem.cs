using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class LobbySystem : Singleton<LobbySystem>
{

    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("lobby", "home");

        TempData.selectedLevel = 1;//这个值需要根据用户数据来设置，这里是测试用
    }

    public void Open()
    {
        ModeHomeControl.OnOpen("lobby");

        List<string> listShowHub = new List<string>() { "gem", "coin", "profile"};

        //获得屏幕比例，如果屏幕比例超过2:1，则不显示coin
        if (UIUtility.IsScreenOverRatio())
        {
            listShowHub.Remove("coin");
        }

        HeaderControl.OnShowMainHideHub(listShowHub);
        FooterControl.OnSelect("lobby");

        Refresh();

        EventManager.TriggerEvent<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_INIT, new UILobbyArgs()
        {
            selectedLevel = TempData.selectedLevel,
            totalLevel = AllLevel.dictMainLevelData.Count,
            themeName = AllLevel.dictMainLevelData[TempData.selectedLevel].themeName,
            themeVarient = AllLevel.dictMainLevelData[TempData.selectedLevel].themeVarient,
        });

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.OnBackToLobby,
        });
    }

    //刷新lobby界面
    //只会刷新当前选中的按个level_slot
    public void Refresh()
    {
        EventManager.TriggerEvent<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_REFRESH_UI, new UILobbyArgs()
        {
            levelID = TempData.selectedLevel,
            displayName = AllLevel.dictMainLevelData[TempData.selectedLevel].displayName,
        });
        return ;
    }

    public void OnChangeLevel(bool isNextLevel)
    {
        int totalLevel = AllLevel.dictMainLevelData.Count;
        
        int selectedLevel = TempData.selectedLevel;
        if(isNextLevel)
        {
            selectedLevel++;
        }
        else
        {
            selectedLevel--;
        }
        if(selectedLevel > totalLevel)
        {
            selectedLevel = 1;
        }
        if(selectedLevel < 1)
        {
            selectedLevel = totalLevel;
        }
        TempData.selectedLevel = selectedLevel;

        LevelData levelData = AllLevel.dictMainLevelData[selectedLevel];
        EventManager.TriggerEvent<UILobbyChangeLevelArgs>(LobbyEventName.EVENT_LOBBY_CHANGE_LEVEL, new UILobbyChangeLevelArgs()
        {
            isNextLevel = isNextLevel,
            selectedLevel = selectedLevel,
            totalLevel = totalLevel,
            commingThemeName = levelData.themeName,
            commingThemeVarient = levelData.themeVarient,
        });

        Refresh();
    }


    #region 展示下方槽位
    public void OnShowBottom()
    {
        EventManager.TriggerEvent<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_SHOW_BOTTOM, null);
    }
    #endregion
}
