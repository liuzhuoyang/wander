using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 战斗系统基类，提供状态机框架和单例支持
/// 
/// 功能特性：
/// - 提供完整的状态机框架（准备、战斗、结束等状态）
/// - 支持单例模式，确保全局唯一实例
/// - 提供抽象方法和虚方法供子类重写
/// - 处理战斗事件（敌人击杀、波次开始/结束等）
/// 
/// 使用方式：
/// public class BattleSystem : BattleSystemBase&lt;BattleSystem&gt;
/// {
///     // 实现具体的业务逻辑
/// }
/// </summary>
/// <typeparam name="T">继承此基类的具体战斗系统类型，必须继承自BattleSystemBase&lt;T&gt;</typeparam>
public abstract class BattleSystemBase<T> : Singleton<T> where T : BattleSystemBase<T>
{
    //必须要创建一个战斗状态机，因为BattleSystem继承这个基类，而状态机不支持继承结构
    private BattleSystemFSM fsm;
    protected LevelData currentLevelData;

    public virtual async UniTask Init()
    {
        await UIMain.Instance.CreateModeSubPage("prepare", "battle");
        await UIMain.Instance.CreateModeSubPage("fight", "battle");

        FeatureUtility.OnAddFeature(FeatureType.Battle, () => {
            Game.Instance.OnChangeState(GameStates.Battle);
        });

        //需要添加一个状态机组件才能运作
        fsm = gameObject.AddComponent<BattleSystemFSM>();
        fsm.Init();
        SetupFSMCallback();
    }

    //设置状态机里的回调方法，子类通过重写这些方法来实现自己的业务逻辑
    void SetupFSMCallback()
    {   
        fsm.OnBattleStartEnterCallback = OnBattleStartPhaseEnter;
        fsm.OnBattleStartExitCallback = OnBattleStartPhaseExit;
        fsm.OnPrepareStartEnterCallback = OnPrepareStartPhaseEnter;
        fsm.OnPrepareRunEnterCallback = OnPrepareRunPhaseEnter;
        fsm.OnPrepareEndEnterCallback = OnPrepareEndPhaseEnter;
        fsm.OnFightStartEnterCallback = OnFightStartPhaseEnter;
        fsm.OnFightRunEnterCallback = OnFightRunPhaseEnter;
        fsm.OnFightRunExitCallback = OnFightRunPhaseExit;
        fsm.OnFightEndEnterCallback = OnFightEndPhaseEnter;
        fsm.OnFightEndExitCallback = OnFightEndPhaseExit;
        fsm.OnPauseEnterCallback = OnPausePhaseEnter;
        fsm.OnPauseExitCallback = OnPausePhaseExit;
        fsm.OnBattleEndEnterCallback = OnBattleEndPhaseEnter;
        fsm.OnBattleEndExitCallback = OnBattleEndPhaseExit;
    }

    public virtual void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    #region 战斗系统的开始结束唯一出入口
    public void OnBattleStart()
    {
        Debug.Log("=== BattleSystem: 进入战斗 ===");
        OnChangeBattleState(BattleStates.BattleStart);
    }

    public void OnBattleEnd()
    {
        Debug.Log("=== BattleSystem: 离开战斗 ===");
        OnChangeBattleState(BattleStates.BattleEnd);
    }

    public void OnPause()
    {
        OnChangeBattleState(BattleStates.Pause);
    }
    #endregion

    #region 状态机框架 - 子类可以重写这些方法

    public async UniTask BattleStart_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 BattleStart_Enter 战斗开始状态机 ===");
        await OnBattleStartPhaseEnter();
        OnChangeBattleState(BattleStates.PrepareStart);
    }

    async void BattleStart_Exit()
    {
        await OnBattleStartPhaseExit();
    }

    async void PrepareStart_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 Prepare_Enter 波段准备状态机 ===");
        OnPlayBattlePrepareBGM();
        ModeBattleControl.OnOpen("prepare");
        await OnPrepareStartPhaseEnter();
        OnChangeBattleState(BattleStates.PrepareRun);
    }

    async void PrepareRun_Enter()
    {
        await OnPrepareRunPhaseEnter();
    }

    async void PrepareEnd_Enter()
    {
        await OnPrepareEndPhaseEnter();
        OnChangeBattleState(BattleStates.FightStart);
    }

    async void FightStart_Enter()
    {
        await OnFightStartPhaseEnter();
    }

    async void FightRun_Enter()
    {
        await OnFightRunPhaseEnter();
    }

    async void FightRun_Exit()
    {
        await OnFightRunPhaseExit();
    }

    async void FightEnd_Enter()
    {
        await OnFightEndPhaseEnter();
        OnChangeBattleState(BattleStates.PrepareStart);
    }

    async void FightEnd_Exit()
    {
        await OnFightEndPhaseExit();
    }

    async void Pause_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 Pause_Enter 停止状态机 ===");
        await OnPausePhaseEnter();
    }

    async void Pause_Exit()
    {
        await OnPausePhaseExit();
    }

    protected async void BattleEnd_Enter()
    {
        Debug.Log($"=== BattleSystem: 进入 BattleEnd_Enter 战斗结束状态机 ===");
        await OnBattleEndPhaseEnter();
    }

    async void BattleEnd_Exit()
    {
        await OnBattleEndPhaseExit();
    }

    public void OnChangeBattleState(BattleStates state)
    {
        Debug.Log($"=== BattleSystem:  切换到战斗状态 {state} ===");
        fsm.ChangeState(state);
    }
    #endregion

    #region 抽象方法 - 子类必须实现
    protected abstract UniTask OnLoadLevel();
    protected abstract void OnPlayBattlePrepareBGM();
    protected abstract void OnPlayBattleFightBGM();
    #endregion

    #region 虚方法 - 子类可以重写
    protected virtual async UniTask OnBattleStartPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Battle Start Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");
    }
    protected virtual async UniTask OnBattleStartPhaseExit() 
    {
        TipManager.Instance.OnTip("DEBUG: Exit Battle Start Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Exit");
    }
    protected virtual async UniTask OnBattleEndPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Battle End Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");
    }
    protected virtual async UniTask OnBattleEndPhaseExit() 
    {
        TipManager.Instance.OnTip("DEBUG: Exit Battle End Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Exit");
    }

    protected virtual async UniTask OnPrepareStartPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Prepare Start Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");
    }
    protected virtual async UniTask OnPrepareRunPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Prepare Run Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");
    }
    protected virtual async UniTask OnPrepareEndPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Prepare End Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");

        //波段结束进入战斗阶段
        OnChangeBattleState(BattleStates.FightStart);
    }

    protected virtual async UniTask OnPrepareEndPhaseExit() 
    {
        TipManager.Instance.OnTip("DEBUG: Exit Prepare End Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Exit");
    }

    protected virtual async UniTask OnFightStartPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Fight Start Phase");
        await OnDebugTipCountDown(BattleStates.FightStart, "Enter");
    }
    protected virtual async UniTask OnFightRunPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Fight Run Phase");
        await OnDebugTipCountDown(BattleStates.FightRun, "Enter");
    }
    protected virtual async UniTask OnFightRunPhaseExit() 
    { 
        TipManager.Instance.OnTip("DEBUG: Exit Fight Run Phase");
        await OnDebugTipCountDown(BattleStates.FightRun, "Exit");
    }
    protected virtual async UniTask OnFightEndPhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Fight End Phase");
        await OnDebugTipCountDown(BattleStates.FightEnd, "Enter");
    }
    protected virtual async UniTask OnFightEndPhaseExit() 
    {
        TipManager.Instance.OnTip("DEBUG: Exit Fight End Phase");
        await OnDebugTipCountDown(BattleStates.FightEnd, "Exit");
    }
    protected virtual async UniTask OnPausePhaseEnter() 
    {
        TipManager.Instance.OnTip("DEBUG: Enter Pause Phase");
        await OnDebugTipCountDown(BattleStates.Pause, "Enter");
    }
    protected virtual async UniTask OnPausePhaseExit() 
    {
        TipManager.Instance.OnTip("DEBUG: Exit Pause Phase");
        await OnDebugTipCountDown(BattleStates.Pause, "Exit");
    }

    #endregion

    #region 调试方法
    protected async UniTask OnDebugTipCountDown(BattleStates currentBattleState, string phase)
    {
        int delayTime = 400;
        await UniTask.Delay(delayTime);
        Debug.Log($"DEBUG: OnDebugTipCountDown: 当前状态 {currentBattleState} {phase} 切换状态倒计时: 3");
        //TipManager.Instance.OnTip("3");
        await UniTask.Delay(delayTime);
        //TipManager.Instance.OnTip("2");
         Debug.Log($"DEBUG: OnDebugTipCountDown: 当前状态 {currentBattleState} {phase} 切换状态倒计时: 2");
        await UniTask.Delay(delayTime);
        //TipManager.Instance.OnTip("1");
         Debug.Log($"DEBUG: OnDebugTipCountDown: 当前状态 {currentBattleState} {phase} 切换状态倒计时: 1");
        await UniTask.Delay(delayTime);
    }
    #endregion
}