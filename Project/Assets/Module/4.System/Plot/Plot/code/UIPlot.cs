using UnityEngine;
public class UIPlot : UIBase
{
    public GameObject objBattle;
    public GameObject objLobby;
    public GameObject objBattleBubble;
    PlotBattleView plotBattleView;
    PlotLobbyView plotLobbyView;
    PlotBattleBubbleView plotBattleBubbleView;
    PlotItem currentPlotItem;

    private void Awake()
    {
        EventManager.StartListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_UI, OnPlotStart);
        EventManager.StartListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_NEXT_STEP_UI, OnNextStep);
        EventManager.StartListening<UIPlotLobbyRewardArgs>(PlotEventName.EVENT_ON_PLOT_LOBBY_REWARD_UI, OnLobbyRewardStep);
        EventManager.StartListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_END_UI, OnPlotEnd);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_UI, OnPlotStart);
        EventManager.StopListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_NEXT_STEP_UI, OnNextStep);
        EventManager.StopListening<UIPlotLobbyRewardArgs>(PlotEventName.EVENT_ON_PLOT_LOBBY_REWARD_UI, OnLobbyRewardStep);
        EventManager.StopListening<UIPlotArgs>(PlotEventName.EVENT_ON_PLOT_END_UI, OnPlotEnd);
    }

    //开始剧情
    void OnPlotStart(UIPlotArgs args)
    {
        objLobby.SetActive(args.sceneType == PlotSceneType.Lobby);
        objBattle.SetActive(args.sceneType == PlotSceneType.Battle);
        objBattleBubble.SetActive(args.sceneType == PlotSceneType.BattleBubble);

        switch (args.sceneType)
        {
            case PlotSceneType.Lobby:
                plotLobbyView = objLobby.GetComponent<PlotLobbyView>();
                plotLobbyView.Init();
                break;
            case PlotSceneType.Battle:
                plotBattleView = objBattle.GetComponent<PlotBattleView>();
                plotBattleView.Init();
                break;
            case PlotSceneType.BattleBubble:
                plotBattleBubbleView = objBattleBubble.GetComponent<PlotBattleBubbleView>();
                plotBattleBubbleView.Init();
                break;
        }
    }

    //下一步
    void OnNextStep(UIPlotArgs args)
    {
        this.currentPlotItem = args.currentPlotItem;

        if (plotLobbyView != null)
        {
            plotLobbyView.OnNext(currentPlotItem);
            return;
        }

        if (plotBattleView != null)
        {
            plotBattleView.OnNext(currentPlotItem);
            return;
        }

        if (plotBattleBubbleView != null)
        {
            plotBattleBubbleView.OnNext(currentPlotItem);
            return;
        }
    }

    void OnPlotEnd(UIPlotArgs args)
    {
        switch (args.sceneType)
        {
            case PlotSceneType.Battle:
                CloseUI();
                break;
            case PlotSceneType.BattleBubble:
                CloseUI();
                break;
            case PlotSceneType.Lobby:
                CloseUI();
                break;
            default:
                CloseUI();
                break;
        }
        Reset();
    }

    void OnLobbyRewardStep(UIPlotLobbyRewardArgs args)
    {
        plotLobbyView.OnRewardStep(args);
    }

    //3个对象设置为null，否则下次更换条目类型会报错
    void Reset()
    {
        plotLobbyView = null;
        plotBattleView = null;
        plotBattleBubbleView = null;
    }
}
