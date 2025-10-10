using UnityEngine;

using BattleActor;
using BattleBuff;
using BattleBuff.Ability;
using BattleLaunch;
using BattleSummon;

namespace BattleGear
{
    public enum GearAbilityTriggerType
    {
        OnGearRestart, //武器重新启动时
        OnGearFire, //武器执行其对应行为（发射 或 召唤 或 其他）时
        OnGearSummon, //武器执行召唤时
        OnGearHitTarget, //武器命中目标时
    }
    public enum GearAbilityTargetType
    {
        Self,
        Assign,
        TargetsInHitArea,
        OnHitPosition,
    }
    public class GearAbility : Ability
    {
        protected GearBase host;
        protected GearAbilityTriggerType gearAbilityTriggerType;
        protected GearAbilityTargetType gearAbilityTargetType;

        public GearAbility(string buffTypeID, string[] buffIDs, GearAbilityTriggerType triggerType, GearAbilityTargetType targetType, int _maxTriggerCount)
        {
            this.buffTypeID = buffTypeID;
            this.buffIDs = buffIDs;
            maxTriggerCount = _maxTriggerCount;
            triggerCount = 0;
            gearAbilityTriggerType = triggerType;
            gearAbilityTargetType = targetType;
        }
        //在范围内施加技能
        public GearAbility(string buffTypeID, string[] buffIDs, GearAbilityTriggerType triggerType, float radius, TeamMask teamMask, int _maxTriggerCount) :
            this(buffTypeID, buffIDs, triggerType, GearAbilityTargetType.TargetsInHitArea, _maxTriggerCount)
        {
            areaRadius = radius;
            areaTeamMask = teamMask;
        }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            host = parent.GetComponent<GearBase>();
        }
        protected override void BuffBegin()
        {
            switch (gearAbilityTriggerType)
            {
                case GearAbilityTriggerType.OnGearRestart:
                    host.onGearRestart += ExcuteAbility;
                    break;
                case GearAbilityTriggerType.OnGearFire:
                    host.onGearBeginFire += ExcuteAbility;
                    break;
                case GearAbilityTriggerType.OnGearSummon:
                    host.onGearSummon += ExcuteAbilityOnSummon;
                    break;
                case GearAbilityTriggerType.OnGearHitTarget:
                    host.onGearHitTarget += ExcuteAbilityOnHit;
                    break;
            }
        }
        protected override void BuffRemove()
        {
            switch (gearAbilityTriggerType)
            {
                case GearAbilityTriggerType.OnGearRestart:
                    host.onGearRestart -= ExcuteAbility;
                    break;
                case GearAbilityTriggerType.OnGearFire:
                    host.onGearBeginFire -= ExcuteAbility;
                    break;
                case GearAbilityTriggerType.OnGearSummon:
                    host.onGearSummon -= ExcuteAbilityOnSummon;
                    break;
                case GearAbilityTriggerType.OnGearHitTarget:
                    host.onGearHitTarget -= ExcuteAbilityOnHit;
                    break;
            }
        }
        protected void ExcuteAbilityOnHit(BattleHitData hitData)
        {
            bool excuteFlag = true;
            switch (gearAbilityTargetType)
            {
                case GearAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case GearAbilityTargetType.Assign:
                    if (hitData.hitActor != null && hitData.hitActor.gameObject.GetComponent<BuffHandler>() != null)
                    {
                        ExcuteAbilityToTarget(hitData.hitActor.gameObject.GetComponent<BuffHandler>());
                    }
                    else
                    {
                        excuteFlag = false;
                    }
                    break;
                case GearAbilityTargetType.TargetsInHitArea:
                    if(!hitData.isHitByRange)
                        ExcuteAbilityOnTargetsNearPos(hitData.hitPoint);
                    break;
                case GearAbilityTargetType.OnHitPosition:
                    if(!hitData.isHitByRange)
                        ExcuteAbilityOnPos(hitData.hitCaster, hitData.hitPoint);
                    break;

            }
            if (excuteFlag)
            {
                OnAbilityExcute();
            }
        }
        protected void ExcuteAbilityOnSummon(ISummonnee summonnee)
        {
            switch (gearAbilityTargetType)
            {
                case GearAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case GearAbilityTargetType.Assign:
                    var handler = summonnee.gameObject.GetComponent<BuffHandler>();
                    if (handler != null)
                        ExcuteAbilityToTarget(handler);
                    break;
                case GearAbilityTargetType.TargetsInHitArea:
                    ExcuteAbilityOnTargetsNearPos(summonnee.gameObject.transform.position);
                    break;
                case GearAbilityTargetType.OnHitPosition:
                    ExcuteAbilityOnPos(host.gameObject, summonnee.gameObject.transform.position);
                    break;
            }
            OnAbilityExcute();
        }
        protected void ExcuteAbility()
        {
            switch (gearAbilityTargetType)
            {
                case GearAbilityTargetType.Self:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case GearAbilityTargetType.Assign:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case GearAbilityTargetType.TargetsInHitArea:
                    ExcuteAbilityOnTargetsNearPos(host.transform.position);
                    break;
                case GearAbilityTargetType.OnHitPosition:
                    ExcuteAbilityOnPos(host.gameObject, host.transform.position);
                    break;
            }
            OnAbilityExcute();
        }
    }
}