using BattleActor.Basement.Skill;
using BattleBuff;
using BattleLaunch;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleActor.Basement.Skill
{
    [CreateAssetMenu(fileName = "BasementLaunchAbilityData_SO", menuName = "RTS_Demo/Actor/Basement/BasementLaunchAbilityData_SO")]
    public class BasementLaunchSkillData_SO : BasementSkillData_SO
    {
        [InfoBox("注意！发射类基地技能，“Ability Type”与额外“Buffs”无效",InfoMessageType.Warning)]
        [Header("发射配置")] public LaunchConfig_SO launchConfig;

        [BoxGroup("战斗参数")] public bool penetrateArmor = false;
        [BoxGroup("战斗参数")] public ElementType damageType = ElementType.Physical;
        [BoxGroup("战斗参数")] public float attackDamage = 10; //伤害范围
        [BoxGroup("战斗参数")] public float damageMultiToBuilding = 0;
        [BoxGroup("战斗参数")] public float damageMultiToShield = 0;
        [BoxGroup("战斗参数")] public float criticRate; //暴击率
        [BoxGroup("战斗参数")] public float criticDamageMultiplier; //暴击伤害倍率

        protected override bool IsArea() => true;
        protected override Buff GetBuffInstance()
        {
            var attackData = new AttackData(attackDamage, damageMultiToBuilding, damageMultiToShield, penetrateArmor, criticRate, criticDamageMultiplier, damageType);
            return new BasementLaunchSkill(launchConfig, attackData, 0, m_buffID, null, areaRadius, areaTeamMask);
        }
    }
}
