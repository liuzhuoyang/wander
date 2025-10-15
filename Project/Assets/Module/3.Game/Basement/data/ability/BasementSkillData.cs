using BattleBuff;
using BattleBuff.Ability;
using UnityEngine;

namespace RTSDemo.Basement.Skill
{
    [CreateAssetMenu(fileName = "BasementSkillData", menuName = "RTS_Demo/Actor/Basement/BasementSkillData")]
    public class BasementSkillData : AbilityData
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
