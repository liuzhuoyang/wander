using UnityEngine;
using BattleBuff;
using BattleBuff.Ability;
using Sirenix.OdinInspector;

namespace RTSDemo.Basement.Skill
{
    using BattleActor;
    public enum BasementAbilityType
    {
        [LabelText("直接触发")]
        Direct, //直接触发，无需选择目标
        [LabelText("指定区域触发")]
        ApplyToArea, //作用于指定区域
    }
    public class BasementSkill : Ability
    {
        protected BasementBasic host;
        protected BasementAbilityType basementAbilityTargetType;
        //是否需要选择目标才能触发
        public bool requireTarget{ get; private set; } = false;

        public BasementSkill(string buffTypeID, string[] buffIDs, BasementAbilityType targetType)
        {
            this.buffTypeID = buffTypeID;
            this.buffIDs = buffIDs;
            triggerCount = 0;
            basementAbilityTargetType = targetType;
            //基地技能没有固定的使用限制
            maxTriggerCount = -1;
        }
        public BasementSkill(string buffTypeID, string[] buffIDs, float areaRadius, TeamMask areaTeamMask)
        {
            requireTarget = true;

            this.buffTypeID = buffTypeID;
            this.buffIDs = buffIDs;
            triggerCount = 0;
            this.areaRadius = areaRadius;
            this.areaTeamMask = areaTeamMask;
            basementAbilityTargetType = BasementAbilityType.ApplyToArea;
            //基地技能没有固定的使用限制
            maxTriggerCount = -1;
        }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            host = parent.GetComponent<BasementBasic>();
        }
        protected void ExcuteAbility()
        {
            switch (basementAbilityTargetType)
            {
                case BasementAbilityType.Direct:
                    ExcuteAbilityToTarget(host.m_buffHandler);
                    break;
                case BasementAbilityType.ApplyToArea:
                    ExcuteAbilityOnTargetsNearPos(host.transform.position);
                    break;
            }
            OnAbilityExcute();
        }
        public void ExcuteBasementAbility()
        {
            ExcuteAbility();
        }
        public virtual void ExcuteBasementAbilityOnNearPos(Vector2 targetPos)
        {
            ExcuteAbilityOnTargetsNearPos(targetPos);
        }
    }
}