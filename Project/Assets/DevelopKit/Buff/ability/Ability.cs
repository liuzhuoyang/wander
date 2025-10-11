using BattleActor;
using UnityEngine;

namespace BattleBuff.Ability
{
    //技能是一种特殊的buff，它负责设定特定的触发时机，并在触发时给特定对象赋予一系列特定buff
    public abstract class Ability : Buff
    {
        protected string[] buffIDs;
        protected int maxTriggerCount;
        protected int triggerCount;
        protected TeamMask areaTeamMask;
        protected float areaRadius;

        protected void ExcuteAbilityToTarget(BuffHandler buffHandler)
        {
            foreach (var buff in buffIDs)
            {
                buffHandler.TryAddBuff(buff);
            }
        }
        protected void ExcuteAbilityOnTargetsNearPos(Vector2 targetPos)
        {
            //针对范围的技能，则获取范围内的目标
            //注意：该范围为瞬间范围，持续性范围采用生成zone的方式去处理
            var targets = BattleActorScanSystem.Instance.FindTargets<IBattleActor>(targetPos, areaRadius, ActorScanOrder.Default, areaTeamMask);
            foreach (var actor in targets)
            {
                var handler = actor.gameObject.GetComponent<BuffHandler>();
                if (handler != null)
                    ExcuteAbilityToTarget(handler);
            }
        }
        protected void ExcuteAbilityOnPos(GameObject caster, Vector2 targetPos)
        {
            foreach (var buff in buffIDs)
            {
                if (BuffDataManager.Instance.IsPositionBuff(buff))
                {
                    (BuffDataManager.Instance.GetBuff(buff) as PositionBasedBuff).ExcuteBuffOnPosition(caster, targetPos);
                }
            }
        }
        protected void OnAbilityExcute()
        {
            if (maxTriggerCount > 0)
            {
                triggerCount++;
                if (triggerCount >= maxTriggerCount)
                {
                    ChangeBuffState(BuffState.Complete);
                }
            }
        }
    }
}
