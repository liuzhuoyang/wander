using UnityEngine;
using BattleActor;
using BattleActor.Unit;
using Sirenix.OdinInspector;

public class UB_Simple : MonoBehaviour, IUnitBehaviour
{
    protected enum UnitState
    {
        Idle,
        Approach,
        Attack
    }
    [ReadOnly, SerializeField] protected UnitState currentState;
    protected IBattleActor potentialTarget;
    protected float detectTimer = 0;
    protected UnitBase self;

    public void Init(UnitBase _unit)
    {
        detectTimer = 0;
        potentialTarget = null;
        currentState = UnitState.Idle;
        this.self = _unit;
        _unit.OnUnitAttackExcute += UnitAttack;
    }
    public void UnitUpdate()
    {
        switch(currentState)
        {
            case UnitState.Attack:
                if(IBattleActor.IsInvalid(potentialTarget))
                {
                    ChangeState(UnitState.Idle);
                }
                else if(!self.IsActorInAttackRange(potentialTarget, 1))
                {
                    ChangeState(UnitState.Approach);
                }
                break;
            case UnitState.Idle:
                if(IBattleActor.IsInvalid(potentialTarget))
                {
                    detectTimer += Time.deltaTime;
                    if (detectTimer > UnitService.UNIT_SCAN_INTERSECT) {
                        detectTimer = 0;
                        //扫描范围内是否有敌人，有则在 下一帧 开始接近
                        if (self.TrySearchOpponentActor(out potentialTarget, self.currentAttackRange * UnitService.UNIT_SEARCH_RANGE_MULTIPLIER))
                        {
                            if (self.IsActorInAttackRange(potentialTarget))
                            {
                                ChangeState(UnitState.Attack);
                            }
                            else
                            {
                                ChangeState(UnitState.Approach);
                            }
                        }
                    }
                }
                break;
            case UnitState.Approach:
                if(IBattleActor.IsInvalid(potentialTarget))
                {
                    ChangeState(UnitState.Idle);
                }
                else
                {
                    self.MoveToActor(potentialTarget);
                    if(self.IsActorInAttackRange(potentialTarget))
                    {
                        ChangeState(UnitState.Attack);
                    }
                }
                break;
        }
    }
    void ChangeState(UnitState nextState)
    {
        if(this.currentState == nextState) return;
        if(this.currentState == UnitState.Attack) self.StopAttack();
        this.currentState = nextState;

        switch(nextState)
        {
            case UnitState.Idle:
                self.StopMotion();
                break;
            case UnitState.Attack:
                self.StopMotion();
                self.StartAttack();
                break;
            case UnitState.Approach:
                self.StopAttack();
                self.StartMoving();
                break;
        }
    }
    public void CleanUp()
    {
        detectTimer = 0;
        potentialTarget = null;
        self.OnUnitAttackExcute -= UnitAttack;
    }
    void UnitAttack()
    {
        if(!IBattleActor.IsInvalid(potentialTarget))
        {
            self.AttackAtActor(potentialTarget);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(!IBattleActor.IsInvalid(potentialTarget))
            Gizmos.DrawSphere(potentialTarget.GetClosestPointTo(self.position), 0.2f);
    }
}
