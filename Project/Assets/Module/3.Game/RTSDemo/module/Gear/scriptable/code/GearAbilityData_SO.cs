using BattleBuff;
using BattleBuff.Ability;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearAbilityData_SO", menuName = "RTS_Demo/Gear/GearAbilityData_SO")]
    public class GearAbilityData_SO : AbilityData_SO
    {
        [SerializeField] private GearAbilityTriggerType gearAbilityTriggerType;
        [SerializeField] private GearAbilityTargetType gearAbilityTargetType;

        protected override bool IsArea() => gearAbilityTargetType == GearAbilityTargetType.TargetsInHitArea;
        protected override Buff GetBuffInstance()
        {
            var buffIDs = GetAddBuffIDs();
            if (gearAbilityTargetType == GearAbilityTargetType.TargetsInHitArea)
            {
                return new GearAbility(m_buffID, buffIDs, gearAbilityTriggerType, areaRadius, areaTeamMask, triggerCount);
            }
            else
            {
                return new GearAbility(m_buffID, buffIDs, gearAbilityTriggerType, gearAbilityTargetType, triggerCount);
            }
        }
    }
}
