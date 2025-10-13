using UnityEngine;
using Cysharp.Threading.Tasks;
using MonsterLove.StateMachine;
using System;

public enum BattleStates
{
    Init,
    Pause,
    BattleStart,
    PrepareStart,
    PrepareRun,
    PrepareEnd,
    FightStart,
    FightRun,
    FightEnd,
    BattleEnd,
}

public class BattleSystemFSM : MonoBehaviour
{
    StateMachine<BattleStates> fsm;
    
    // 状态回调委托
    public Func<UniTask> OnBattleStartEnterCallback;
    public Func<UniTask> OnBattleStartExitCallback;
    public Func<UniTask> OnPrepareStartEnterCallback;
    public Func<UniTask> OnPrepareRunEnterCallback;
    public Func<UniTask> OnPrepareEndEnterCallback;
    public Func<UniTask> OnFightStartEnterCallback;
    public Func<UniTask> OnFightRunEnterCallback;
    public Func<UniTask> OnFightRunExitCallback;
    public Func<UniTask> OnFightEndEnterCallback;
    public Func<UniTask> OnFightEndExitCallback;
    public Func<UniTask> OnPauseEnterCallback;
    public Func<UniTask> OnPauseExitCallback;
    public Func<UniTask> OnBattleEndEnterCallback;
    public Func<UniTask> OnBattleEndExitCallback;
    
    public void Init()
    {
        Debug.Log($"=== BattleSystemFSM 战斗状态机模块: 初始化 ===");
        fsm = StateMachine<BattleStates>.Initialize(this);
    }
    
    public void ChangeState(BattleStates state)
    {
        Debug.Log($"=== BattleSystemFSM 战斗状态机模块: 切换到状态 {state} ===");
        fsm.ChangeState(state);
    }
    
    // 状态机方法 - 这些方法会被 MonsterLove 找到
    public async void BattleStart_Enter()
    {
        Debug.Log($"=== BattleSystemFSM 战斗状态机模块: 进入 BattleStart_Enter ===");
        if (OnBattleStartEnterCallback != null)
        {
            await OnBattleStartEnterCallback();
        }
        ChangeState(BattleStates.PrepareStart);
    }

    public async void BattleStart_Exit()
    {
        if (OnBattleStartExitCallback != null)
        {
            await OnBattleStartExitCallback();
        }
    }

    public async void PrepareStart_Enter()
    {
        Debug.Log($"=== BattleSystemFSM 战斗状态机模块: 进入 PrepareStart_Enter ===");
        if (OnPrepareStartEnterCallback != null)
        {
            await OnPrepareStartEnterCallback();
        }
        ChangeState(BattleStates.PrepareRun);
    }

    public async void PrepareRun_Enter()
    {
        if (OnPrepareRunEnterCallback != null)
        {
            await OnPrepareRunEnterCallback();
        }
    }

    public async void PrepareEnd_Enter()
    {
        if (OnPrepareEndEnterCallback != null)
        {
            await OnPrepareEndEnterCallback();
        }
        ChangeState(BattleStates.FightStart);
    }

    public async void FightStart_Enter()
    {
        if (OnFightStartEnterCallback != null)
        {
            await OnFightStartEnterCallback();
        }
    }

    public async void FightRun_Enter()
    {
        if (OnFightRunEnterCallback != null)
        {
            await OnFightRunEnterCallback();
        }
    }

    public async void FightRun_Exit()
    {
        if (OnFightRunExitCallback != null)
        {
            await OnFightRunExitCallback();
        }
    }

    public async void FightEnd_Enter()
    {
        if (OnFightEndEnterCallback != null)
        {
            await OnFightEndEnterCallback();
        }
        ChangeState(BattleStates.PrepareStart);
    }

    public async void FightEnd_Exit()
    {
        if (OnFightEndExitCallback != null)
        {
            await OnFightEndExitCallback();
        }
    }

    public async void Pause_Enter()
    {
        Debug.Log($"=== 状态机模块: 进入 Pause_Enter ===");
        if (OnPauseEnterCallback != null)
        {
            await OnPauseEnterCallback();
        }
    }

    public async void Pause_Exit()
    {
        if (OnPauseExitCallback != null)
        {
            await OnPauseExitCallback();
        }
    }

    public async void BattleEnd_Enter()
    {
        Debug.Log($"=== 状态机模块: 进入 BattleEnd_Enter ===");
        if (OnBattleEndEnterCallback != null)
        {
            await OnBattleEndEnterCallback();
        }
    }

    public async void BattleEnd_Exit()
    {
        if (OnBattleEndExitCallback != null)
        {
            await OnBattleEndExitCallback();
        }
    }
}