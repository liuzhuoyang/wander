using System;
using Cysharp.Threading.Tasks;

public class LevelControl
{
    public static void Init()
    {

    }

    public static void OnRefreshLevelLobbyUI(LevelData args)
    {
        EventManager.TriggerEvent<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_REFRESH_UI, new UILobbyArgs()
        {
            chapterID = args.chapterID,
            levelID = args.levelID,
            //chapterDisplayName = args.chapterDisplayName,
            //picName = args.picName,
        });
    }

    /// <summary>
    /// 开始游戏，打开对应levelType，chapterID, levelID的关卡文件。如: normal_001_01
    /// </summary>
    public static async UniTask OnLoadLevel(LevelType levelType, int chapterID, int levelID)
    {
        string levelName = UtilityParse.GetLevelName(chapterID, levelID, levelType);
        LevelData args = AllLevel.dictData[levelName];
 
        await TransitControl.OnTransit();

        await GameAssetManagerBattle.Instance.OnLoadBattleAsset();

        UIMain.Instance.OnModeUI("battle");
        //await MapControl.Instance.OpenLevel(args);
        //关闭过场
        TransitControl.CloseTransit();
    }

    /// <summary>
    /// 退出游戏唯一出口
    /// </summary>
    public static async void OnQuitLevel()
    {
        await TransitControl.OnTransit();
        //清除用户战斗数据
        Game.Instance.OnChangeState(GameStates.Home);
        
        TransitControl.CloseTransit();
    }

    #region 关卡详情
    public static void OnOpenLevelInfo(int chapterID, int levelID)
    {
        /*
        PopupLevelInfoArgs args = new PopupLevelInfoArgs()
        {
            target = "popup_level_info",
            chapterID = chapterID,
            levelID = levelID,
        };
        PopupManager.Instance.OnPopup(args);*/
    }
    #endregion
}
