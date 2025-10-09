using BattleBuff;
using BattleBuff.Ability;
using UnityEngine;

namespace BattleActor.Unit
{
    [CreateAssetMenu(fileName = "UnitAbilityData_SO", menuName = "RTS_Demo/Actor/Unit/UnitAbilityData_SO")]
    public class UnitAbilityData_SO : AbilityData_SO
    {
        [SerializeField] private UnitAbilityTriggerType abilityTriggerType;
        [SerializeField] private UnitAbilityTargetType abilityTargetType;

        protected override bool IsArea() => abilityTargetType == UnitAbilityTargetType.TargetsInArea;
        protected override Buff GetBuffInstance()
        {
            // 获取所有Buff的ID
            string[] buffIDs = GetAddBuffIDs();
            // 如果是范围技能，则需要传入范围和队伍信息
            if (abilityTargetType == UnitAbilityTargetType.TargetsInArea)
            {
                return new UnitAbility(m_buffID, buffIDs, abilityTriggerType, areaRadius, areaTeamMask, triggerCount);
            }
            else
            {
                return new UnitAbility(m_buffID, buffIDs, abilityTriggerType, abilityTargetType, triggerCount);
            }
        }
    }
}