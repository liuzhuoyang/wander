using BattleActor;
using BattleActor.Unit;
using UnityEngine;

public class UB_BombDrone : UB_DroneBasic
{
[Header("Bomb Basic")]
    [SerializeField] private float bombDropPhaseDelay = 0.5f;

    protected override void DroneAttack()
    {
        if(self.needReload) 
        {
            base.self.StopAttack();
            return;
        };

        base.self.AttackAtActor(potentialTarget);
    }
    protected override DroneState TakeOffUpdate()
    {
    //向上飞行一小段时间
        self.unitMovement.SetVelocityVector(Vector3.up);
        stateTimer += Time.deltaTime;
        if(stateTimer>takeOffTime) {
        //尝试寻找目标
            self.TrySearchOpponentActor(out potentialTarget, self.currentAttackRange * UnitService.UNIT_SEARCH_RANGE_MULTIPLIER, true);
        //在结束的时候，前往巡逻轨道
            return DroneState.Cruise;
        }
        return base.TakeOffUpdate();
    }
    protected override DroneState CruiseUpdate()
    {
    //检查是否巡航超时,弹药是否耗尽
        if(self.needReload){
            return DroneState.PreReturn;
        }
    //巡航环绕移动
        stateTimer += Time.deltaTime;
        target = circleMovement.UpdatePointOnCircle(self.currentMoveSpeed);
        self.unitMovement.SlerpVelocity(target-self.position, rotateLerpSpeed);
    //目标处理
        if(!IBattleActor.IsInvalid(potentialTarget))
        {
            if(circleMovement.GetPhase()>bombDropPhaseDelay)
            {
                self.StartAttack();
            }
        }
        else
        {
            if(stateTimer>scanTime+UnitService.UNIT_SCAN_INTERSECT/scanFreqMulti){
                scanTime = stateTimer;
                self.TrySearchOpponentActor(out potentialTarget, self.currentAttackRange * UnitService.UNIT_SEARCH_RANGE_MULTIPLIER, true);
            }
        }
        return base.CruiseUpdate();
    }
    void OnDrawGizmos()
    {
        if(circleMovement==null) return;
        Gizmos.DrawSphere(circleMovement.rotatingCenter, 0.2f);
        Gizmos.DrawSphere(target, 0.2f);
    }
}