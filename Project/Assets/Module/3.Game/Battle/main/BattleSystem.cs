using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using System.Threading.Tasks;


public enum BattleStates
{
    Init,
    Pause, //暂停
    BattleStart, //战斗开始
    PrepareStart,    //准备阶段开始
    PrepareRun, //准备阶段运行中
    PrepareEnd, //准备阶段结束
    FightStart, //战斗阶段
    FightRun,   //战斗阶段运行中
    FightEnd,   //战斗阶段结束
    BattleEnd,  //战斗结束
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

        OnChangeBattleState(BattleStates.PrepareStart);
        //这个允许用户输入的位置，还需要考虑后续添加的动画
        ActingSystem.Instance.StopActing(this.name);
    }

    // 退出战斗开始状态
    void BattleStart_Exit()
    {

    }

    // 进入波段准备状态
    async void PrepareStart_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 Prepare_Enter 波段准备状态机 ===");

        //播放准备音乐
        PlayBattlePrepareBGM();
    
        //打开准备界面
        ModeBattleControl.OnOpen("prepare");

        TipManager.Instance.OnTip("DEBUG: Enter Prepare Start Phase");
        await OnDebugTipCountDown();

        //准备结束，进入PrepareRun状态
        OnChangeBattleState(BattleStates.PrepareRun);
    }

    void PrepareRun_Enter()
    {
        TipManager.Instance.OnTip("DEBUG: Enter Prepare Run Phase");
    }

    //进入准备结束状态
    async void PrepareEnd_Enter()
    {
        //这是一个很微妙的状态，但需要有
        //比如说装备状态结束，需要有一个状态是拉远镜头，可以放这里，虽然也可以放到FightStart里面，但假设FightStart里已经需要做一些战斗相关操作
        //那么这个PrepareEnd就可以作为一个缓冲地带，在做真正的波段战斗之前的一些表现等操作
        TipManager.Instance.OnTip("DEBUG: Enter Prepare End Phase");   
        await OnDebugTipCountDown();

        //准备结束，进入FightStart状态
        OnChangeBattleState(BattleStates.FightStart);
    }

    // 进入波段战斗开始状态
    async void FightStart_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 FightStart_Enter 波段战斗开始状态机 ===");

        //播放战斗音乐
        //PlayBattleFightBGM();

        //打开战斗界面
        ModeBattleControl.OnOpen("fight");

        TipManager.Instance.OnTip("DEBUG: Enter Fight Start Phase");
        //await是Debug观察状态用，实际使用可以替换为一些动画表现，拉远镜头等
        await OnDebugTipCountDown();
        
        //战斗结束，进入FightRun状态
        OnChangeBattleState(BattleStates.FightRun);
    }

    //FightRun 状态
    //用途：为什么需要和FightStart状态区分，比如中间切入到升级的状态，接着要回来战斗，就是FightRun状态，而不是FightStart状态
    void FightRun_Enter()
    {
        TipManager.Instance.OnTip("DEBUG: Enter Fight Run Phase");
    }

    void FightRun_Exit()
    {
        //Fight Run阶段一般来说，不需要做任何操作
    }

    async void FightEnd_Enter()
    {
        //进入波段战斗结束的状态，这里可以做一些波段结算工作，比如说掉落宝箱，跳出金币等
        TipManager.Instance.OnTip("DEBUG: Enter Fight End Phase");

        await  OnDebugTipCountDown();

        //波段结束后，进入准备阶段
        //如果是胜利的话，不会进入这个阶段，而是进入战斗结束阶段
        OnChangeBattleState(BattleStates.PrepareStart);
    }

    void FightEnd_Exit()
    {
        //这里离开FightEnd状态，可以做一些操作，比如停止战斗音乐，单位等
        TipManager.Instance.OnTip("DEBUG: Exit Fight End Phase");
    }

    // 进入暂停状态
    void Pause_Enter()
    {
        //
        Debug.Log($"=== BattleSystem: 进入 Pause_Enter 停止状态机 ===");
    }

    // 退出暂停状态
    void Pause_Exit()
    {
        //这里离开Pause状态，可以做一些操作，比如恢复战斗音乐，单位等
        TipManager.Instance.OnTip("DEBUG: Exit Pause Phase");
    }

    // 进入整场战斗结束状态
    void BattleEnd_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 BattleEnd_Enter 战斗结束状态机 ===");
        //战斗结束，进入BattleEnd状态
        //整个战斗正式结束，这时候可以进行一些清理，结算等工作
        //清理也可以根据情况放到BattleEnd_Exit中
        TipManager.Instance.OnTip("DEBUG: Enter Battle End Phase");
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

    async UniTask OnDebugTipCountDown()
    {
        int delayTime = 800;
        await UniTask.Delay(delayTime);
        TipManager.Instance.OnTip("3");
        await UniTask.Delay(delayTime);
        TipManager.Instance.OnTip("2");
        await UniTask.Delay(delayTime);
        TipManager.Instance.OnTip("1");
        await UniTask.Delay(delayTime);
    }
}