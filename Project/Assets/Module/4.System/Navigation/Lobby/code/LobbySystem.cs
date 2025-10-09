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
        FeatureUtility.OnAddFeature(FeatureType.Lobby, () =>
        {
            Open();
        });
    }

    public void Open()
    {
        ModeHomeControl.OnOpen("lobby");

        List<string> listShowHub = new List<string>() { "gem", "coin", "profile" };

        //获得屏幕比例，如果屏幕比例超过2:1，则不显示coin
        if (UIUtility.IsScreenOverRatio())
        {
            listShowHub.Remove("coin");
        }

        HeaderControl.OnShowMainHideHub(listShowHub);
        FooterControl.OnSelect("lobby");

        Refresh();

        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs
        {
            action = ActionType.OnBackToLobby,
        });
    }

    //刷新lobby界面
    public void Refresh()
    {
        return;
    }


    #region 展示下方槽位
    public void OnShowBottom()
    {
        EventManager.TriggerEvent<UILobbyArgs>(LobbyEventName.EVENT_LOBBY_SHOW_BOTTOM, null);
    }
    #endregion
}
