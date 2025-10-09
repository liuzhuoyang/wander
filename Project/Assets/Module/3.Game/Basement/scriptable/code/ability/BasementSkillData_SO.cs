using BattleBuff;
using BattleBuff.Ability;
using UnityEngine;

namespace BattleActor.Basement.Skill
{
    [CreateAssetMenu(fileName = "BasementAbilityData_SO", menuName = "RTS_Demo/Actor/Basement/BasementAbilityData_SO")]
    public class BasementSkillData_SO : AbilityData_SO
    {
        [SerializeField] private BasementAbilityType basementAbilityType;

        protected override bool IsArea() => basementAbilityType == BasementAbilityType.ApplyToArea;
        protected override Buff GetBuffInstance()
        {
            string[] buffIDs = GetAddBuffIDs();
            if(basementAbilityType == BasementAbilityType.ApplyToArea)
            {
                return new BasementSkill(m_buffID, buffIDs, areaRadius, areaTeamMask);
            }
            else
            {
                return new BasementSkill(m_buffID, buffIDs, basementAbilityType);
            }
        }
    }
}
