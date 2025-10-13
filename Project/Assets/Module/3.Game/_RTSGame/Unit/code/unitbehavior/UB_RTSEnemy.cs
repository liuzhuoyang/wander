using BattleActor.Unit;
using RTSDemo.Grid;
using UnityEngine;

namespace RTSDemo.Unit
{
    public class UB_RTSEnemy : UB_RTSBase
    {
        protected UnitMovement unitMovement;
        public override void Init(UnitBase _unit)
        {
            base.Init(_unit);
            unitMovement = _unit.GetComponent<UnitMovement>();
        }
        protected override UnitState WanderUpdate()
        {
            //FlowField
            Vector2 direction = RTSGridWorldSystem.Instance.GetNodeFromWorldPos(self.position).bestDirection.Vector;
            unitMovement.SlerpVelocity(direction);

            //剩余步骤和Idle一致
            detectTimer += Time.deltaTime;
            if (detectTimer > UnitService.UNIT_SCAN_INTERSECT)
            {
                detectTimer = 0;
                //扫描范围内是否有敌人，有则在 下一帧 开始接近
                if (self.TrySearchOpponentActor(out potentialTarget, self.currentAttackRange * UnitService.UNIT_SEARCH_RANGE_MULTIPLIER))
                {
                    return UnitState.GoToTarget;
                }
            }
            return UnitState.Empty;
        }
        protected override UnitState IdleUpdate()
        {
            return UnitState.Wander;
        }
    }
}