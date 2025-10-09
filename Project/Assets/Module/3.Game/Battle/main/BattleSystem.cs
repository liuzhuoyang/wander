using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;


public enum BattleStates
{
    Init,
    BattleStart,
    WavePrepare,
    WaveFightStart,
    WaveFightRun,
    WaveFightEnd,
    Pause,
    BattleEnd,
}

public class BattleSystem : Singleton<BattleSystem>
{
    public bool isSandboxBattle = false;

    StateMachine<BattleStates> fsm;

    LevelData currentLevelData; //当前关卡数据

    public async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("prepare", "battle");
        await UIMain.Instance.CreateModeSubPage("fight", "battle");

        //添加功能事件，点击按钮后开启战斗功能
        FeatureUtility.OnAddFeature(FeatureType.Battle, () => {
            Game.Instance.OnChangeState(GameStates.Battle);
        });

        fsm = StateMachine<BattleStates>.Initialize(this);
        fsm.ChangeState(BattleStates.Init);
    }

    public void Clear()
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
            case BattleActionType.EnemyKilled:

                break;
            case BattleActionType.WaveStart:
               
                break;
            case BattleActionType.WaveEnd:
                GameData.userData.userStats.OnWavePassed();
                break;
            case BattleActionType.LevelUp:
                
                break;
            case BattleActionType.PlayerDead:
              
                break;
        }
    }

    #region 战斗系统的开始结束唯一出入口

    #endregion

    #region 状态机
    // 进入战斗开始状态
    async void BattleStart_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 BattleStart_Enter 战斗开始状态机 ===");

         //禁止用户输入
        ActingSystem.Instance.OnActing(this.name); 

        await LevelControl.OnLoadLevel(LevelType.Main, 1, 1);

        OnChangeBattleState(BattleStates.WavePrepare);
        //这个允许用户输入的位置，还需要考虑后续添加的动画
        ActingSystem.Instance.StopActing(this.name);
    }

    // 退出战斗开始状态
    void BattleStart_Exit()
    {

    }

    // 进入波段准备状态
    void WavePrepare_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 WavePrepare_Enter 波段准备状态机 ===");

        //播放准备音乐
        PlayBattlePrepareBGM();
    
        //打开准备界面
        ModeBattleControl.OnOpen("prepare");
    }

    // 退出波段准备状态
    void WavePrepare_Exit()
    {

    }

    // 进入波段战斗开始状态
    void WaveFightStart_Enter()
    {
        //打开战斗界面
        ModeBattleControl.OnOpen("fight");

        Debug.Log($"=== BattleSystem: 进入 WaveFightStart_Enter 波段战斗开始状态机 ===");
    }

    // 退出波段战斗开始状态
    void WaveFightStart_Exit()
    {
        
    }

    void WaveFightRun_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 WaveFightRun_Enter 波段战斗运行中状态机 ===");
    }

    void WaveFightRun_Exit()
    {

    }

    // 进入波段战斗结束状态
    void WaveFightEnd_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 WaveFightEnd_Enter 波段战斗结束状态机 ===");
    }

    // 退出波段战斗结束状态
    void WaveFightEnd_Exit()
    {

    }

    // 进入暂停状态
    void Pause_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 Pause_Enter 停止状态机 ===");
    }

    // 退出暂停状态
    void Pause_Exit()
    {

    }

    // 进入整场战斗结束状态
    void BattleEnd_Enter()
    {
         Debug.Log($"=== BattleSystem: 进入 BattleEnd_Enter 战斗结束状态机 ===");
        OnChangeBattleState(BattleStates.BattleEnd);
    }

    // 退出整场战斗结束状态
    void BattleEnd_Exit()
    {

    }

    public void OnChangeBattleState(BattleStates state)
    {
        fsm.ChangeState(state);
    }
    #endregion



    public void OnRevive()
    {
        //AnalyticsControl.Instance.OnLogLevelRevive(BattleData.levelType, BattleData.chapterID, BattleData.levelID, BattleData.currentWave);
    }

    public void OnRefreshBrick(bool isFree = false)
    {

    }

    public void OnPause()
    {
        
    }

    public void OnResume()
    {

    }

    #region 音乐
    protected void PlayBattlePrepareBGM()
    {
        //播放准备音乐
       // AudioControl.Instance.PlayBGM(battleAudioConfig.GetPrepareBGM());
    }
    #endregion
}