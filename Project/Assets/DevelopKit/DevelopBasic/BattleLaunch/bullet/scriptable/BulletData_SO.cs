using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BattleLaunch.Bullet
{
    [CreateAssetMenu(fileName = "BulletData_SO", menuName = "Assets/Bullet/BulletData")]
    public class BulletData_SO : Launchable_SO
    {
        [BoxGroup("子弹基本参数"), SerializeField] private AssetReference bulletRef;
        public AssetReference m_bulletRef => bulletRef;
        public string BulletKey => this.name;
    }
}
