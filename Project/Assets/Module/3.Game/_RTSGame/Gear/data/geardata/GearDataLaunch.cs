using BattleLaunch;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearDataLaunch", menuName = "RTS_Demo/Gear/GearDataLaunch")]
    public class GearDataLaunch : GearData
    {
        public override GearType gearType => GearType.LaunchGear;
        [BoxGroup("发射参数"), PreviouslySerializedAs("launchConfig_SO")] public LaunchConfig launchConfig;
    }
}
