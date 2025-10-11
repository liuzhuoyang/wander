using BattleSummon;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearDataSummon", menuName = "RTS_Demo/Gear/GearDataSummon")]
    public class GearDataSummon : GearData
    {
        public override GearType gearType => GearType.SummonGear;
        [BoxGroup("召唤参数")] public SummonData summonData_SO;
    }
}