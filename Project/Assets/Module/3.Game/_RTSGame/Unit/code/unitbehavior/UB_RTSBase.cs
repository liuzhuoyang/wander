using UnityEngine;
using BattleActor;
using BattleActor.Unit;
using Sirenix.OdinInspector;

namespace RTSDemo.Unit
{
    public class UB_RTSBase : MonoBehaviour, IUnitBehaviour
    {
        protected enum UnitState
        {
            Empty = 0,
            Idle = 1,
            Wander = 2, //随机游荡，非必要状态
            GoToTarget = 3,
            Attack = 4
        }
        [ReadOnly, SerializeField] protected UnitState currentState;
        protected float detectTimer = 0;
        protected IBattleActor potentialTarget;
        protected UnitBase self;

        public virtual void Init(UnitBase _unit)
        {
            detectTimer = 0;
            potentialTarget = null;
            currentState = UnitState.Idle;
            this.self = _unit;
            _unit.OnUnitAttackExcute += UnitAttack;
        }
        public void UnitUpdate()
        {
            UnitState nextState = UnitState.Empty;
            switch (currentState)
            {
                case UnitState.Idle:
                    nextState = IdleUpdate();
                    break;
                case UnitState.Wander:
                    nextState = WanderUpdate();
                    break;
                case UnitState.GoToTarget:
                    nextState = GoToTargetUpdate();
                    break;
                case UnitState.Attack:
                    nextState = AttackUpdate();
                    break;
            }
            if (nextState != UnitState.Empty)
                ChangeState(nextState);
        }
        void ChangeState(UnitState nextState)
        {
            if (this.currentState == nextState) return;
            if (this.currentState == UnitState.Attack) self.StopAttack();
            this.currentState = nextState;

            switch (nextState)
            {
                case UnitState.Idle:
                    self.StopMotion();
                    break;
                case UnitState.Attack:
                    self.StopMotion();
                    self.StartAttack();
                    break;
                case UnitState.GoToTarget:
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

        #region UnitState
        protected virtual UnitState IdleUpdate()
        {
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
        protected virtual UnitState WanderUpdate() => UnitState.Empty;
        protected virtual UnitState GoToTargetUpdate()
        {
            if (IBattleActor.IsInvalid(potentialTarget))
            {
                return UnitState.Idle;
            }
            else
            {
                self.MoveToActor(potentialTarget);
                if (self.IsActorInAttackRange(potentialTarget))
                {
                    return UnitState.Attack;
                }
            }
            return UnitState.Empty;
        }
        protected virtual UnitState AttackUpdate()
        {
            if (IBattleActor.IsInvalid(potentialTarget))
            {
                return UnitState.Idle;
            }
            else if (!self.IsActorInAttackRange(potentialTarget, 1))
            {
                return UnitState.GoToTarget;
            }
            return UnitState.Empty;
        }
        #endregion

        void UnitAttack()
        {
            if (!IBattleActor.IsInvalid(potentialTarget))
            {
                self.AttackAtActor(potentialTarget);
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (!IBattleActor.IsInvalid(potentialTarget))
                Gizmos.DrawSphere(potentialTarget.GetClosestPointTo(self.position), 0.2f);
        }
    }
}