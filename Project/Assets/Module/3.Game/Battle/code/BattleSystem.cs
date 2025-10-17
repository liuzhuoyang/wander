using UnityEngine;
using Cysharp.Threading.Tasks;

using BattleLaunch.Bullet;
using SimpleAudioSystem;
using CameraUtility;
using RTSDemo.Unit;
using BattleGear;
using RTSDemo.Zone;
using RTSDemo.Basement;
using RTSDemo.Grid;
using RTSDemo.Spawn;

public class BattleSystem : BattleSystemBase<BattleSystem>
{
    private GameObject battleControllerPrefab;
    private GameObject battleController;
    private EnemySpawner enemySpawner;

    public override async UniTask Init()
    {
        await base.Init();
        battleControllerPrefab = await GameAsset.GetPrefabAsync("projectwander_battle_controller");
    }

    public override void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void OnBattleAction(BattleActionArgs args)
    {
        switch (args.battleAction)
        {
            case BattleActionType.EnemyKilled: OnEnemyKilled(args); break;
            case BattleActionType.WaveStart: OnWaveStart(args); break;
            case BattleActionType.WaveEnd: OnWaveEnd(args); break;
            case BattleActionType.LevelUp: OnLevelUp(args); break;
            case BattleActionType.PlayerDead: OnPlayerDead(args); break;
        }
    }

    #region 抽象方法 - 子类必须实现
    //读取关卡数据
    protected override async UniTask OnLoadLevel()
    {
        //获取 章节1关卡1作为测试
        currentLevelData = AllLevel.GetLevelData(LevelType.Main, 0, 0);

        await TransitControl.OnTransit();
        await GameAssetManagerBattle.Instance.OnLoadBattleAsset();

        UIMain.Instance.OnModeUI("battle");
        BattleFormationMangaer.Instance.CraftFormatian(currentLevelData.formationName);
        await BattleScenesMangaer.Instance.LoadScene(currentLevelData);
        await BasementControl.Instance.CreateBasement(currentLevelData.basementData.m_basementKey, Vector2.zero);

        //加载完各项地图后，刷新flowfield
        RTSGridEvent.Call_OnGridNodeChange();
        //关闭过场
        TransitControl.CloseTransit();
    }

    //播放准备阶段音乐
    protected override void OnPlayBattlePrepareBGM()
    {
        //播放准备音乐
        AudioManager.Instance.PlayBGM("bgm_battle_prepare_001");
    }

    //播放战斗阶段音乐
    protected override void OnPlayBattleFightBGM()
    {
        //播放战斗音乐
        AudioManager.Instance.PlayBGM("bgm_battle_fight_001");
    }

    #endregion

    #region 状态机虚方法 - 子类可以重写
    //准备阶段开始
    protected override async UniTask OnBattleStartPhaseEnter()
    {
        //战斗开始 - 进入阶段
        await base.OnBattleStartPhaseEnter();

        //创建必要的战斗内控制器
        battleController = Instantiate(battleControllerPrefab, this.transform);
        enemySpawner = gameObject.AddComponent<EnemySpawner>();

        ActingSystem.Instance.OnActing(this.name);
        await OnLoadLevel();

        //开启各项游戏系统
        enemySpawner.InitSpawner(currentLevelData);
        UnitManager.Instance.StartBattle();
        BulletManager.Instance.StartBattle();
        GearManager.Instance.StartBattle();
        BuffZoneManager.Instance.StartBattle();
        BattleShopSystem.Instance.StartBattle();

        //读取玩家数据，然后创建对应场景人物
        CameraManager.Instance.OnBattleEnter();
        BattleScenesMangaer.Instance.LoadUserData();

        OnChangeBattleState(BattleStates.PrepareStart);
        ActingSystem.Instance.StopActing(this.name);
    }
    protected override async UniTask OnBattleStartPhaseExit()
    {
        //战斗开始 - 退出阶段
        await base.OnBattleStartPhaseExit();
    }

    protected override async UniTask OnPrepareStartPhaseEnter()
    {
        //准备开始 - 进入阶段
        ModeBattleControl.OnOpen("prepare");
        AudioManager.Instance.PlayBGM("bgm_battle_prepare_001");
        CameraManager.Instance.OnPrepareStart();
        BattleShopSystem.Instance.RefreshShopItem();

        await base.OnPrepareStartPhaseEnter();
    }

    protected override async UniTask OnPrepareRunPhaseEnter()
    {
        //准备运行 - 进入阶段
        await base.OnPrepareRunPhaseEnter();
    }
    protected override async UniTask OnPrepareEndPhaseEnter()
    {
        //准备结束 - 进入阶段
        ModeBattleControl.OnCloseActive();
        await base.OnPrepareEndPhaseEnter();
    }

    protected override async UniTask OnFightStartPhaseEnter()
    {
        //波段战斗开始 - 进入阶段
        ModeBattleControl.OnOpen("fight");
        AudioManager.Instance.PlayBGM("bgm_battle_fight_001");
        CameraManager.Instance.OnFightStart();
        enemySpawner.StartSpawning(1);
        await base.OnFightStartPhaseEnter();
    }
    protected override async UniTask OnFightRunPhaseEnter()
    {
        //波段战斗进行 - 进入阶段
        await base.OnFightRunPhaseEnter();
    }

    protected override async UniTask OnFightRunPhaseExit()
    {
        //波段战斗进行 - 退出阶段
        await base.OnFightRunPhaseExit();
    }
    protected override async UniTask OnFightEndPhaseEnter()
    {
        //波段战斗结束 - 进入阶段
        enemySpawner.StopSpawning();
        ModeBattleControl.OnCloseActive();
        UnitManager.Instance.CleanUpUnit();
        BuffZoneManager.Instance.CleanUpBuffZone();
        BulletManager.Instance.CleanUpBullet();
        
        await base.OnFightEndPhaseEnter();

    }
    protected override async UniTask OnFightEndPhaseExit()
    {
        //波段战斗结束 - 退出阶段
        await base.OnFightEndPhaseExit();
    }
    protected override async UniTask OnPausePhaseEnter()
    {
        //暂停 - 进入阶段
        await base.OnPausePhaseEnter();
    }
    protected override async UniTask OnPausePhaseExit()
    {
        //暂停 - 退出阶段
        await base.OnPausePhaseExit();
    }
    protected override async UniTask OnBattleEndPhaseEnter()
    {
        //战斗结束 - 进入阶段
        await base.OnBattleEndPhaseEnter();

        //这里出结算面板
        await EndSystem.Instance.OnOpen(new EndArgs()
        {

        });
        CameraManager.Instance.OnBattleEnd();
        BattleScenesMangaer.Instance.CleanUpScene();
        //战斗结束阶段
        Destroy(battleController);
        Destroy(enemySpawner);
        //清理各项游戏系统
        UnitManager.Instance.CleanUpUnit();
        BulletManager.Instance.CleanUpBullet();
        GearManager.Instance.CleanUpBattle();
        BuffZoneManager.Instance.CleanUpBuffZone();
    }
    protected override async UniTask OnBattleEndPhaseExit()
    {
        //退出关卡
        await TransitControl.OnTransit();
        //清除用户战斗数据
        Game.Instance.OnChangeState(GameStates.Home);

        TransitControl.CloseTransit();
        //战斗结束 - 退出阶段
        await base.OnBattleEndPhaseExit();
    }
    #endregion

    #region 战斗事件处理
    void OnEnemyKilled(BattleActionArgs args)
    {
        //敌人击杀
    }
    void OnWaveStart(BattleActionArgs args)
    {
        //波段开始
    }
    void OnWaveEnd(BattleActionArgs args)
    {
        GameData.userData.userStats.OnWavePassed();
        //波段结束
    }
    void OnLevelUp(BattleActionArgs args)
    {
        //升级获取能力
    }
    void OnPlayerDead(BattleActionArgs args)
    {
        //玩家死亡
    }
    #endregion
}