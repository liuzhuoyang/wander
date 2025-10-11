using BattleSummon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "Gear_Summon", menuName = "RTS_Demo/Gear/Gear_Summon")]
    public class GearData_Summon_SO : GearData_SO
    {
        public override GearType gearType => GearType.SummonGear;
        [BoxGroup("召唤参数")] public SummonData_SO summonData_SO;
    }
}