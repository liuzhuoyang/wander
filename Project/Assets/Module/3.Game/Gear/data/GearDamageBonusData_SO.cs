using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearDamageBonus", menuName = "RTS_Demo/Gear/GearDamageBonus")]
    public class GearDamageBonusData_SO : ScriptableObject
    {
        [Space(20)]
        [LabelText("Gears等级伤害倍率")] public List<GearDamageBonus> gearDamageBonus;
        public float GetGearDamageByLevel(string gearKey, int level)
        {
            level = Mathf.Max(0, level - 1);
            var damageBonus = gearDamageBonus.Find(x => x.gear.m_gearKey == gearKey);
            if (damageBonus == null)
                return damageBonus.gear.baseDamage;
            else
                return damageBonus.gear.baseDamage * (1 + Mathf.Pow(level, 1.7f) * damageBonus.levelPowerMulti + level * damageBonus.levelMulti);
        }
    }

    [System.Serializable]
    public class GearDamageBonus
    {
        public GearData_SO gear;
        public float levelPowerMulti = 0.1f;
        public float levelMulti = 1f;
    }
}
