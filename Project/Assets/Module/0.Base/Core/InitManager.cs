using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InitManager : Singleton<InitManager>
{
    float waitTime = 0.05f;
    CancellationTokenSource cancellationTokenSource;
    public bool isTimeOut;
    public async void Init()
    {
        OnPreset();
        await UIMain.Instance.Init();
        //await UIMain.Instance.OpenUI("landing", UIPageType.Overlay);
        await OnSetupBasic();
    }

    // 预初始化
    async void OnPreset()
    {

    }

    #region 初始化阶段1: 读取CSV表数据，创建initmanager
    async UniTask OnSetupBasic()
    {
        Debug.Log("=== InitManager: 1.basic setup ===");
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        OnSendLoadingEvent(0.1f);

        //GameObject csvLoader = new GameObject("[CSVLoader]");
        //csvLoader.AddComponent<CSVLoader>();
        //await CSVLoader.Instance.Init();//初始化配置表

        //await GameData.InitData();       //先初始化资源
        await GameData.Init();             //读取用户数据
        

        //BattleData.Init();               //初始化战斗数据，基本上都是映射字典等内容，实际的战斗数据会在进入战斗后才初始化
        //MetaFormula.Init();              //同上
        //BattleFormula.Init();            //同上
        // GlobalData.Init();              //全局数据初始化
        TempData.Init();                   //临时存储数据初始化，使用例子：解锁功能后，功能解锁页面新功能icon需要飞到目标位置，所以在解锁的时候，设置这个位置
        //UserBonusData.Init();            //用户加成数据初始化
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

        GameObject manager = new GameObject("[Manager]");
        await manager.AddComponent<GameAssetGenericManager>().Init(); //加入基础资源管理器
        manager.AddComponent<GameAssetBattleManager>().Init(); //加入战斗资源管理器（需要动态加载释放）

        //加入15%进度，观察上面GameAssets读取是否费时间
        OnSendLoadingEvent(0.15f);

        manager.AddComponent<TimeManager>().Init();
        manager.AddComponent<MessageManager>().Init();
        manager.AddComponent<PopupManager>().Init();
        manager.AddComponent<TipManager>().Init();
        manager.AddComponent<TooltipManager>().Init();
        // manager.AddComponent<PoolManager>().Init();
        //manager.AddComponent<RateManager>().Init();
        manager.AddComponent<SettingManager>().Init();
        manager.AddComponent<TokenManager>().Init();
        manager.AddComponent<VersionManager>().Init();

        // GameObject map = new GameObject("[Map]");
        // map.AddComponent<MapControl>().Init();

        // GameObject control = new GameObject("[Control]");

        GameObject gameSaver = new GameObject("[GameSaver]");
        gameSaver.AddComponent<GameSaver>().Init();

        await OnSetupAuth();
    }
    #endregion

    #region 初始化阶段2: Auth/SDK初始化
    async UniTask OnSetupAuth()
    {
        //离线开发模式
        if (GameConfig.main.productMode == ProductMode.DevOffline)
        {
            GameData.CheckDateTime();
            OnSetupGameSystem();
            return;
        }

        Debug.Log("=== InitManager: 2.init auth ===");

        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

        // 更新进度条
        OnSendLoadingEvent(0.2f, true);

        //TODO 获取JWT Token
        await TokenManager.Instance.OnRefreshToken(
            () =>
            {

            },
            () =>
            {
                //onFailure?.Invoke();
                OnNetError();
                return;
            },
            () =>
            {
                //onTimeout?.Invoke();
                OnTimeout();
                return;
            }
        );

        OnSetupServer();
    }
    void OnSetupServer()
    {
        if (isTimeOut) return;
        bool isUnknownUser = false;
        //ios不判断来源
#if !UNITY_IOS
        isUnknownUser = UtilityMisc.GetStoreName() == "Unknown";
#endif
        //正常来源用户登陆
        GameObject cloud = new GameObject("[Cloud]");
        cloud.AddComponent<CloudControl>().Init(isUnknownUser,
           (isNewRegisterUser) => { OnLoginSucceed(isNewRegisterUser); },
           () => { OnNetError(); },
           () => { OnTimeout(); }
           );
    }

    //登陆成功
    void OnLoginSucceed(bool isNewRegisterUser = false)
    {
        if (isTimeOut) return;
        //初始化分析平台SDK
        AnalyticsControl.Instance.Init(isNewRegisterUser);
        //登陆打点
        AnalyticsControl.Instance.OnLogin();
        //获取时间
        OnInitTime();
    }

    //访问网络失败
    void OnNetError()
    {
        //TODO 这里返回错误不是超时
        isTimeOut = true;
        MessageManager.Instance.OnTimeout();
    }

    //访问超时
    void OnTimeout()
    {
        isTimeOut = true;
        MessageManager.Instance.OnTimeout();
    }

    //初始化时间
    async void OnInitTime()
    {
        //检查是否是作弊用户
        await CloudAccess.Instance.CheckCheatUser();
        await OnGetNetTime(() =>
        {
            OnCheckVersion();
        },
        () =>
        {
            // OnNetError();
        }
        );
        OnSendLoadingEvent(0.3f, true);
    }

    //获取时间
    async UniTask OnGetNetTime(Action onSuccess, Action onFailure)
    {
        //获取时间
        await CloudAccess.Instance.GetNetTime((timespan) =>
        {
            //获取网络时间成功
            //更新时间相关数据，新的一天重置等
            GameData.CheckDateTime();
            onSuccess?.Invoke();
        }, () =>
        {
            onFailure?.Invoke();
            Debug.Log("=== InitManager: failed access net time ===");
            //获取网络时间失败
        }, () =>
        {
            // OnTimeout();
        });
    }

    void OnCheckVersion()
    {
        CloudVersion.Instance.OnCheckLatestVersion(
        () =>
        {
            //MessageManager.Instance.CloseLoading();
            Debug.Log("=== InitManager: version checked ===");
            OnSetupGameSystem();
            OnSendLoadingEvent(0.4f);
        },
        () =>
        {
            //不满足最小版本
            MessageManager.Instance.CloseLoading();
        },
        () =>
        {
            // OnNetError();
        },
        () =>
        {
            // OnTimeout();
        }
        );
    }
    #endregion

    async void OnSetupGameSystem()
    {
        Debug.Log("=== InitManager: 4.start adding game system ===");
        // 初始化 CancellationTokenSource
        cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: cancellationTokenSource.Token);

            // 如果任务被取消，则提前退出
            if (cancellationTokenSource.Token.IsCancellationRequested)
                return;

            OnSendLoadingEvent(0.7f);

            GameObject system = new GameObject("[System]");
            //Generic
            //system.AddComponent<PinSystem>().Init();
            //system.AddComponent<CheckInSystem>().Init();
            //system.AddComponent<TutSystem>().Init();
            //system.AddComponent<PlotSystem>().Init();
            system.AddComponent<PlotSequenceSystem>().Init();
            system.AddComponent<SequenceTaskSystem>().Init();
            system.AddComponent<FeatureSystem>().Init();
            system.AddComponent<ActingSystem>().Init();
            system.AddComponent<ItemSystem>().Init();
            /*system.AddComponent<ItemExpirationSystem>().Init();
            system.AddComponent<RewardSystem>().Init();
            system.AddComponent<ChestSystem>().Init();
            system.AddComponent<ProfileSystem>().Init();
            system.AddComponent<AvatarSystem>().Init();
            system.AddComponent<PawnSystem>().Init();
            system.AddComponent<GroupABSystem>().Init();
            //服务器agent
            // system.AddComponent<AgentSystem>().Init();
            system.AddComponent<DungeonSystem>().Init();
            system.AddComponent<TowerSystem>().Init();
            system.AddComponent<ArenaSystem>().Init();
            system.AddComponent<ExploreSystem>().Init();

            system.AddComponent<SectorSystem>().Init();
            system.AddComponent<RankingSystem>().Init();
            system.AddComponent<WarpSystem>().Init();
            system.AddComponent<MatchSystem>().Init();
            system.AddComponent<ChallengeSystem>().Init();
            system.AddComponent<RelicSystem>().Init();
            system.AddComponent<TaskSystem>().Init();
            system.AddComponent<AchievementSystem>().Init();
            system.AddComponent<TavernSystem>().Init();
            // system.AddComponent<TravelSystem>().Init();
            system.AddComponent<SupplySystem>().Init();
            system.AddComponent<EndSystem>().Init();
            system.AddComponent<BioSystem>().Init();
            system.AddComponent<PetSystem>().Init();
            system.AddComponent<PowerSystem>().Init();
            system.AddComponent<BattleBaseSystem>().Init();
            // system.AddComponent<RecruitSystem>().Init();
            system.AddComponent<ChanceSystem>().Init();
            system.AddComponent<AFKSystem>().Init();
            system.AddComponent<VipSystem>().Init();
            system.AddComponent<ArmorySystem>().Init();
            system.AddComponent<MarketSystem>().Init();
            system.AddComponent<ShopSystem>().Init();
            system.AddComponent<PromoSystem>().Init();
            system.AddComponent<FundSystem>().Init();
            system.AddComponent<WormholeSystem>().Init();
            system.AddComponent<StoryboardSystem>().Init();
            system.AddComponent<LiveEventSystem>().Init();
            system.AddComponent<VentureSystem>().Init();
            system.AddComponent<PrivilegeSystem>().Init();
            system.AddComponent<RedeemSystem>().Init();
*/
            await system.AddComponent<BattleSystem>().Init();

            // 主界面五个页面，需要创建页面并放在mode ui上
            // 注意：这里要按照Footer排列的从左到右顺序来创建，否则动画方向会错乱
            //await system.AddComponent<LootSystem>().Init();
            //await system.AddComponent<GearSystem>().Init();
            await system.AddComponent<LobbySystem>().Init();
            // await system.AddComponent<EquipSystem>().Init();
            //await system.AddComponent<TalentSystem>().Init();
            //await system.AddComponent<WorldSystem>().Init();

/*
            // 初始化外围加成 放到这里是因为EnergySystem需要获取talentsystem的体力加成
            GlobalBonusData.InitGlobalBonus();
            system.AddComponent<EnergySystem>().Init();

            // 终端系统
            system.AddComponent<TerminalSystem>().Init();
            system.AddComponent<MailSystem>().Init();
            system.AddComponent<RoadmapSystem>().Init();
            //system.AddComponent<ArchivesSystem>().Init();
            system.AddComponent<HandbookSystem>().Init();
*/
            // 如果任务被取消，提前退出
            if (cancellationTokenSource.Token.IsCancellationRequested)
                return;

            OnDoneInit();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Operation was cancelled.");
        }
    }

    // 在 Play 模式结束时取消所有异步操作
    void OnDestroy()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }
    }

    async void OnDoneInit()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
        Debug.Log("=== InitManager: done init ===");
        OnSendLoadingEvent(1);
        Game.Instance.OnStartGame();
        EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs { action = ActionType.Login });

        //给一点100%的反应时间
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime + 0.25f));
        EventManager.TriggerEvent<LandingUIArgs>(EventNameLanding.EVENT_LOADING_CLOSE_UI, new LandingUIArgs { });
    }

    void OnSendLoadingEvent(float progress, bool isShowHintText = false)
    {
        EventManager.TriggerEvent<LandingUIArgs>(EventNameLanding.EVENT_LOADING_REFRESH_UI, new LandingUIArgs { currentProgress = progress, isShowHintText = isShowHintText });
    }
}
