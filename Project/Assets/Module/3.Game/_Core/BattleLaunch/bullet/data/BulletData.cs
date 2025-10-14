using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BattleLaunch.Bullet
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Assets/Bullet/BulletData")]
    public class BulletData : LaunchableData
    {
        [BoxGroup("子弹基本参数"), SerializeField] private AssetReferenceGameObject bulletRef;
        public AssetReferenceGameObject m_bulletRef => bulletRef;
        public string BulletKey => this.name;
    }
}
