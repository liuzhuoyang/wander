using BattleLaunch;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "Gear_Launch", menuName = "RTS_Demo/Gear/Gear_Launch")]
    public class GearData_Launch_SO : GearData_SO
    {
        public override GearType gearType => GearType.LaunchGear;
        [BoxGroup("发射参数")] public LaunchConfig_SO launchConfig_SO;
    }
}
