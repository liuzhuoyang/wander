using MonsterLove.StateMachine;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using DG.Tweening;


public class Game : Singleton<Game>
{
    public StateMachine<GameStates> fsm;
    bool isGameInit = false;

    void Start()
    {
        Debug.Log("=== Game: start game ===");
        DOTween.logBehaviour = LogBehaviour.ErrorsOnly;

        Application.targetFrameRate = 60;
        Application.runInBackground = false;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        fsm = StateMachine<GameStates>.Initialize(this);
        fsm.ChangeState(GameStates.Init);

        //正式发布环境需要进行的强制设置，避免手动修改错误忘记修改回来导致问题
        if (GameConfig.main.productMode == ProductMode.Release)
        {
            //Debug.unityLogger.logEnabled = TGameSDK.UTGetGameDebug();
            GameConfig.main.debugTool = DebugTool.Off;
        }

        InitManager.Instance.Init();
    }

    public void OnStartGame()
    {
        Debug.Log("=== Game: game started ===");
        if (GameData.userData.userProgress.isFirstGame)
        {
            GameData.userData.userProgress.isFirstGame = false;

            //如果第一次进入游戏，可能需要进入第0关
            //LevelControl.OnPlayLevel(LevelType.Normal, 0, 0, ChapterMode.Normal);

            //这里还要考虑，根据情况是否需要进入Home
            OnChangeState(GameStates.Home);
        }
        else
        {
            OnChangeState(GameStates.Home);
        }

        //每日第一次登陆
        if (GameData.userData.userMisc.isNewDayLogin)
        {
            GameData.userData.userMisc.isNewDayLogin = false;
            EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs() { action = ActionType.DailyLogin });
        }

        Application.targetFrameRate = 60;
        isGameInit = true;
    }

    #region 交互
    public void Release() { }

    public async void Restart()
    {
        MessageManager.Instance.OnLoading();

        OnChangeState(GameStates.Pause);

        // 清除场景中所有物件
        DestroyDynamicObject();

        // 取消所有异步任务（如网络请求、动画等）
        ResetStaticData();

        GameAssetGenericManager.Instance.ResetDynamicFont();

        // 停止所有的事件监听器
        EventManager.StopAllListening();

        // 释放未使用的资源，确保内存中的无用资源被卸载
        await Resources.UnloadUnusedAssets();

        // 延迟 500 毫秒，给玩家足够的提示时间
        await UniTask.Delay(500);

        // 可选：手动调用垃圾回收，确保最大限度回收内存
        GC.Collect();

        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }

    void ResetStaticData()
    {
        GameData.Reset();
    }

    void DestroyDynamicObject()
    {
        // 获取当前活动场景中的所有根游戏对象
        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        // 遍历所有对象，销毁除名字为 "UI" 的对象之外的其他对象
        foreach (GameObject obj in allObjects)
        {
            // 如果对象的名字不是 "UI"，销毁它
            if (obj.name != "UI")
            {
                Destroy(obj);
            }
        }

        Debug.Log("=== Game: All dynamic objects except 'UI' have been destroyed. ===");
    }

    #endregion

    #region 前后台切换
    public void SwitchToFrontend()
    {
        //根据之前切换后台的临时标记，更新一共离线的时间
        TimeManager.Instance?.UpdateTempAFKTime();
        Debug.Log("=== Game: back to FRONT end ===");
        EnergySystem.Instance?.OnApplyBackendTime();
    }

    public void OfflineRestart()
    {

    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!isGameInit) return;

        if (hasFocus)
        {
            // 焦点回来（下拉通知栏收起、弹窗关闭、回到编辑器）
            SwitchToFrontend();
        }
        else
        {
            // 焦点丢失（下拉通知栏、弹窗、跳转外部SDK登录页面）
            SwitchToBackend();
        }
    }

    void OnApplicationPause(bool isPause)
    {
        if (!isGameInit) return;

        if (!isPause)
        {
            SwitchToFrontend();
        }
        else
        {
            SwitchToBackend();
        }
    }
    #endregion

    #region 切换去后台
    public void SwitchToBackend()
    {
        //切换到后台时候更新离线时间的临时标记
        TimeManager.Instance?.SetTempAFK();
        Debug.Log("=== Game: switch to BACK end ===");
        //GameSaver.Instance.OnSave();
    }
    #endregion

    #region FSM
    void Init_Enter()
    {

    }

    void Init_Exit()
    {

    }

    void Home_Enter()
    {
        //清理资源缓存
        GameAssetGenericManager.Instance.ResetDynamicFont();
        //开启外围主页面
        UIMain.Instance.OnModeUI("home");
        // 显示背景
        UIBackgroundControl.OnShowUIBackground();
        //打开Lobby系统
        LobbySystem.Instance.Open();
        //触发剧情/弹窗的等待序列
        SequenceTaskSystem.Instance.OnStarTriggerSeq();
        //初始化HeaderControl
        HeaderControl.Init();
        //刷新战力
        //PowerSystem.Instance.OnTotalPowerChanged();
        //播放Lobby背景音乐
        AudioControl.Instance.PlayBGM("bgm_lobby");

        //每次回到大厅，必定要执行一次读取广告
        AdControl.Instance.LoadAD();
    }

    void Home_Exit()
    {
        //清理资源缓存
        GameAssetGenericManager.Instance.ResetDynamicFont();
        // 隐藏背景
        UIBackgroundControl.OnHideUIBackground();
    }

    void Battle_Enter()
    {
        UIMain.Instance.OnModeUI("battle");
        HeaderControl.OnHide();
        FooterControl.OnHide();
        BattleSystem.Instance.OnChangeBattleState(BattleStates.BattleStart);
    }

    void Battle_Exit()
    {
        HeaderControl.OnShow();
        FooterControl.OnShow();
        BattleSystem.Instance.OnChangeBattleState(BattleStates.BattleEnd);
    }
    public void OnChangeState(GameStates s)
    {
        fsm.ChangeState(s);

        Debug.Log("=== Game: change FSM: " + s + " ===");
    }
    #endregion

    #region DEBUG
    public void OnDebugBattle()
    {
        UIMain.Instance.OnModeUI("sandbox_battle");
        HeaderControl.OnHide();
        FooterControl.OnHide();
        //SandboxBattleSystem.Instance.OnSandboxBattleStart();
    }
    #endregion

    public bool CheckIsGameInit()
    {
        return isGameInit;
    }
}

//游戏状态
public enum GameStates
{
    Acting,                     //表演状态
    Init,                       //初始化状态
    Home,                       //大厅
    Battle,                     //战斗中
    Pause                       //
}



