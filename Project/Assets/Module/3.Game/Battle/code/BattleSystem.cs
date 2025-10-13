using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;


public class BattleSystem : BattleSystemBase<BattleSystem>
{
    public override async UniTask Init()
    {
        await base.Init();
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
        // AudioControl.Instance.PlayBGM(battleAudioConfig.GetPrepareBGM());
    }

    //播放战斗阶段音乐
    protected override void OnPlayBattleFightBGM()
    {
        //播放战斗音乐
        // AudioControl.Instance.PlayBGM(battleAudioConfig.GetFightBGM());
    }
    
    #endregion

    #region 状态机虚方法 - 子类可以重写
    //准备阶段开始
    protected override async UniTask OnBattleStartPhaseEnter()
    {   
        await base.OnBattleStartPhaseEnter();
        ActingSystem.Instance.OnActing(this.name);
        await OnLoadLevel();
        OnChangeBattleState(BattleStates.PrepareStart);
        ActingSystem.Instance.StopActing(this.name);
    }
    protected override async UniTask OnBattleStartPhaseExit()
    {
        await base.OnBattleStartPhaseExit();
        //战斗阶段退出
    }

    protected override async UniTask OnPrepareStartPhaseEnter()
    {
        ModeBattleControl.OnOpen("prepare");
        await base.OnPrepareStartPhaseEnter();
    }

    protected override async UniTask OnPrepareRunPhaseEnter()
    {
        //准备阶段运行
        await base.OnPrepareRunPhaseEnter();
    }
    protected override async UniTask OnPrepareEndPhaseEnter()
    {
        ModeBattleControl.OnCloseActive();
        await base.OnPrepareEndPhaseEnter();
        //准备阶段结束
    }

    protected override async UniTask OnFightStartPhaseEnter()
    {
        ModeBattleControl.OnOpen("fight");
        await base.OnFightStartPhaseEnter();
        //战斗阶段开始
    }
    protected override async UniTask OnFightRunPhaseEnter()
    {
        await base.OnFightRunPhaseEnter();
        //战斗阶段运行
    }
    protected override async UniTask OnFightRunPhaseExit()
    {
        await base.OnFightRunPhaseExit();
        //战斗阶段退出
    }
    protected override async UniTask OnFightEndPhaseEnter()
    {
        ModeBattleControl.OnCloseActive();
        await base.OnFightEndPhaseEnter();
        //战斗阶段结束
    }
    protected override async UniTask OnFightEndPhaseExit()
    {
        await base.OnFightEndPhaseExit();
        //战斗阶段退出
    }
    protected override async UniTask OnPausePhaseEnter()
    {
        await base.OnPausePhaseEnter();
        //暂停阶段
    }
    protected override async UniTask OnPausePhaseExit()
    {
        await base.OnPausePhaseExit();
        //暂停阶段退出
    }
    protected override async UniTask OnBattleEndPhaseEnter()
    {
        await base.OnBattleEndPhaseEnter();
        //战斗结束阶段
    }
    protected override async UniTask OnBattleEndPhaseExit()
    {
        await base.OnBattleEndPhaseExit();
        //战斗结束阶段退出
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

    #region 音乐
    protected void PlayBattlePrepareBGM()
    {
        //播放准备音乐
       // AudioControl.Instance.PlayBGM(battleAudioConfig.GetPrepareBGM());
    }
    #endregion
}