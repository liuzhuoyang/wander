using BattleActor.Unit;
using UnityEngine;

public class UB_Drone : UB_DroneBasic
{
    protected override DroneState TakeOffUpdate()
    {
    //向上飞行一小段时间
        self.unitMovement.SetVelocityVector(Vector3.up);
        stateTimer += Time.deltaTime;
        if(stateTimer>takeOffTime) {
        //在结束的时候，随机锁定范围内的一个敌人，并接近
            if(self.TrySearchOpponentActor(out potentialTarget, searchRadius))
                return DroneState.Approach;
        //若未能找到敌人，则巡航
            else
                return DroneState.Cruise;
        }        
        return base.TakeOffUpdate();
    }
    protected override DroneState CruiseUpdate()
    {
    //巡航移动
        stateTimer+=Time.deltaTime;
        target = circleMovement.UpdatePointOnCircle(self.currentMoveSpeed);
        self.unitMovement.SlerpVelocity(target-(Vector2)transform.position, rotateLerpSpeed);
    //每过一段时间，扫描范围内是否有敌人，有则攻击
        if(stateTimer>scanTime+UnitService.UNIT_SCAN_INTERSECT/scanFreqMulti){
            scanTime = stateTimer;
            if(self.TrySearchOpponentActor(out potentialTarget, searchRadius)){
                return DroneState.Approach;
            }
        }
        return base.CruiseUpdate();
    }
    protected override DroneState ApproachUpdate()
    {
    //单位是否已经死亡或消失
        if(potentialTarget==null||potentialTarget.IsDead){
            return DroneState.Cruise;
        }
    //TODO接近目标
        stateTimer+=Time.deltaTime;
        self.unitMovement.SlerpVelocity(potentialTarget.position-(Vector2)transform.position, rotateLerpSpeed);
    //检查目标是否在范围内
        if(self.IsActorInAttackRange(potentialTarget))
        {
            return DroneState.SurroundTarget;
        }
        return base.ApproachUpdate();
    }
    protected override DroneState SurroundTargetUpdate()
    {
    //单位是否已经死亡或消失
        if(potentialTarget==null||potentialTarget.IsDead){
            return DroneState.Cruise;
        }
    //持续跟踪目标
        stateTimer+=Time.deltaTime;
        circleMovement.rotatingCenter = potentialTarget.position;
        target = circleMovement.UpdatePointOnCircle(self.currentMoveSpeed);
        self.unitMovement.SlerpVelocity(target-(Vector2)transform.position, rotateLerpSpeed);
    //检查目标是否在范围内
        if(!self.IsActorInAttackRange(potentialTarget, 1)){
            return DroneState.Approach;
        }
        return base.SurroundTargetUpdate();
    }
    void OnDrawGizmos(){
        if(circleMovement==null) return;
        Gizmos.DrawSphere(circleMovement.rotatingCenter, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(target, 0.2f);
        Gizmos.DrawRay(transform.position, self.unitMovement.GetVector()*5);
    }
}
