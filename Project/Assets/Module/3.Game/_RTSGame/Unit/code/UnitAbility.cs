using BattleBuff;
using BattleBuff.Ability;
using BattleLaunch;

namespace RTSDemo.Unit
{
    using BattleActor;
    public enum UnitAbilityTriggerType
    {
        OnUnitSpawned,
        OnUnitFindTarget,
        OnUnitAttackState,
        OnUnitAttack,
        OnUnitHitTarget,
        OnUnitGetHit,
        OnUnitHealthZero,
        OnUnitDie
    }
    public enum UnitAbilityTargetType
    {
        Self,
        Assign,
        TargetsInArea,
        OnAssignPosition,
    }
    public class UnitAbility : Ability
    {
        protected UnitBase host;
        protected UnitAbilityTriggerType unitAbilityTriggerType;
        protected UnitAbilityTargetType unitAbilityTargetType;

        public UnitAbility(string buffTypeID, string[] buffIDs, UnitAbilityTriggerType triggerType, UnitAbilityTargetType targetType, int _maxTriggerCount)
        {
            this.buffTypeID = buffTypeID;
            this.buffIDs = buffIDs;
            unitAbilityTriggerType = triggerType;
            unitAbilityTargetType = targetType;
            maxTriggerCount = _maxTriggerCount;
            triggerCount = 0;
        }
        public UnitAbility(string buffTypeID, string[] buffIDs, UnitAbilityTriggerType triggerType, float radius, TeamMask teamMask, int _maxTriggerCount):
            this(buffTypeID, buffIDs, triggerType, UnitAbilityTargetType.TargetsInArea, _maxTriggerCount)
        {
            areaRadius = radius;
            areaTeamMask = teamMask;
        }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            host = parent.GetComponent<UnitBase>();
        }
        protected override void BuffBegin()
        {
            switch (unitAbilityTriggerType)
            {
                case UnitAbilityTriggerType.OnUnitSpawned:
                    host.OnUnitSpawn += ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitFindTarget:
                    host.OnFindTargetUnit += ExcuteAbilityOnAsignTarget;
                    break;
                case UnitAbilityTriggerType.OnUnitAttackState:
                    host.OnUnitEnterAttackMode += ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitAttack:
                    host.OnUnitAttackExcute += ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitHitTarget:
                    host.OnUnitHitTarget += ExcuteAbilityOnHit;
                    break;
                case UnitAbilityTriggerType.OnUnitGetHit:
                    host.OnUnitGetHit += ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitHealthZero:
                    host.OnUnitHealthZero += ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitDie:
                    host.OnPostUnitDie += ExcuteAbility;
                    break;
            }
        }
        protected override void BuffRemove()
        {
            switch (unitAbilityTriggerType)
            {
                case UnitAbilityTriggerType.OnUnitSpawned:
                    host.OnUnitSpawn -= ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitFindTarget:
                    host.OnFindTargetUnit -= ExcuteAbilityOnAsignTarget;
                    break;
                case UnitAbilityTriggerType.OnUnitAttackState:
                    host.OnUnitEnterAttackMode -= ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitAttack:
                    host.OnUnitAttackExcute -= ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitHitTarget:
                    host.OnUnitHitTarget -= ExcuteAbilityOnHit;
                    break;
                case UnitAbilityTriggerType.OnUnitGetHit:
                    host.OnUnitGetHit -= ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitHealthZero:
                    host.OnUnitHealthZero -= ExcuteAbility;
                    break;
                case UnitAbilityTriggerType.OnUnitDie:
                    host.OnPostUnitDie -= ExcuteAbility;
                    break;
            }
        }
        protected void ExcuteAbility()
        {
            switch (unitAbilityTargetType)
            {
                case UnitAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case UnitAbilityTargetType.Assign:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case UnitAbilityTargetType.TargetsInArea:
                    ExcuteAbilityOnTargetsNearPos(host.position);
                    break;
                case UnitAbilityTargetType.OnAssignPosition:
                    ExcuteAbilityOnPos(host.gameObject, host.position);
                    break;
            }
            OnAbilityExcute();
        }
        void ExcuteAbilityOnHit(BattleHitData hitData)
        {
            bool excuteFlag = true;
            switch (unitAbilityTargetType)
            {
                case UnitAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case UnitAbilityTargetType.Assign:
                    if (hitData.hitActor != null && hitData.hitActor.gameObject.GetComponent<BuffHandler>() != null)
                    {
                        ExcuteAbilityToTarget(hitData.hitActor.gameObject.GetComponent<BuffHandler>());
                    }
                    else
                    {
                        excuteFlag = false;
                    }
                    break;
                case UnitAbilityTargetType.TargetsInArea:
                    if(!hitData.isHitByRange)
                        ExcuteAbilityOnTargetsNearPos(hitData.hitPoint);
                    break;
                case UnitAbilityTargetType.OnAssignPosition:
                    if(!hitData.isHitByRange)
                        ExcuteAbilityOnPos(hitData.hitCaster, hitData.hitPoint);
                    break;
            }
            if (excuteFlag)
            {
                OnAbilityExcute();
            }
        }
        void ExcuteAbilityOnAsignTarget(IBattleActor hit)
        {
            switch (unitAbilityTargetType)
            {
                case UnitAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case UnitAbilityTargetType.Assign:
                    var handler = hit.gameObject.GetComponent<BuffHandler>();
                    if (handler != null)
                    {
                        ExcuteAbilityToTarget(handler);
                    }
                    break;
                case UnitAbilityTargetType.TargetsInArea:
                    ExcuteAbilityOnTargetsNearPos(hit.position);
                    break;
                case UnitAbilityTargetType.OnAssignPosition:
                    ExcuteAbilityOnPos(host.gameObject, hit.position);
                    break;
            }
            OnAbilityExcute();
        }
    }
}
