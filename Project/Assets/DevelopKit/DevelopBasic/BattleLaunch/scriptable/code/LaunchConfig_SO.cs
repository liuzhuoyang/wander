using Sirenix.OdinInspector;
using UnityEngine;

using BattleActor;

namespace BattleLaunch
{
    [CreateAssetMenu(fileName = "LaunchConfig_SO", menuName = "Assets/Launch/LaunchConfig")]
    public class LaunchConfig_SO : ScriptableObject
    {
        public string launchName => this.name;
        [BoxGroup("基本发射信息")] public Launchable_SO launchableData_SO;
        [BoxGroup("基本发射信息")] public bool requireTargetToLaunch = true;
        [TabGroup("基本索敌参数")] public ActorScanOrder scanOrder;
        [TabGroup("基本索敌参数")] public bool trackTargetIfCan;
        [TabGroup("基本索敌参数")] public bool retargetPerLaunch;
        [TabGroup("基本索敌参数")] public bool retargetPerCount;
        [TabGroup("弹道参数"), Min(1)] public int burstCount;
        [TabGroup("弹道参数"), Min(0)] public float burstInterval;
        [TabGroup("弹道参数"), Min(1)] public int spreadCount;
        [TabGroup("弹道参数"), Min(0)] public float spreadAngle;
    }
}