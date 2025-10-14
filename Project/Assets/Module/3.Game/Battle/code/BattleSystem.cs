using UnityEngine;
using Cysharp.Threading.Tasks;
using SimpleAudioSystem;
using RTSDemo.Unit;
using BattleLaunch.Bullet;
using BattleGear;
using RTSDemo.Zone;
using CameraUtility;


public class BattleSystem : BattleSystemBase<BattleSystem>
{
    private GameObject battleControllerPrefab;
    private GameObject battleController;
    public override async UniTask Init()
    {
        await base.Init();
        battleControllerPrefab = await GameAsset.GetPrefabAsync("rts_battle_controller");
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

    #region 战斗开始状态机

    #endregion


    #region 抽象方法 - 子类必须实现
    //读取关卡数据
    protected override async UniTask OnLoadLevel()
    {
        await LevelControl.OnLoadLevel(LevelType.Main, 1, 1);
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
        ActingSystem.Instance.OnActing(this.name);
        await OnLoadLevel();

        //开启各项游戏系统
        UnitManager.Instance.StartBattle();
        BulletManager.Instance.StartBattle();
        GearManager.Instance.StartBattle();
        BuffZoneManager.Instance.StartBattle();

        //读取玩家数据，然后创建对应场景人物
        CameraManager.Instance.OnBattleEnter();
        BattleScensMangaer.Instance.LoadUserData();

        //创建战斗内控制器
        battleController = Instantiate(battleControllerPrefab, this.transform);

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
        ModeBattleControl.OnCloseActive();
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
        //战斗结束阶段
        Destroy(battleController);
        //清理各项游戏系统
        UnitManager.Instance.CleanUpBattle();
        BulletManager.Instance.CleanUpBattle();
        GearManager.Instance.CleanUpBattle();
        BuffZoneManager.Instance.CleanUpBattle();
    }
    protected override async UniTask OnBattleEndPhaseExit()
    {
        //退出关卡
        LevelControl.OnQuitLevel();
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