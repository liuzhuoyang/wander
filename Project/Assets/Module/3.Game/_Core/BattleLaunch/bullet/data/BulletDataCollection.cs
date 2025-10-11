using UnityEngine;

namespace BattleLaunch.Bullet
{
    [CreateAssetMenu(fileName = "BulletDataCollection", menuName = "Assets/Bullet/BulletCollection")]
    public class BulletDataCollection : DataCollection<BulletData>
    {
        public override BulletData GetDataByKey(string key) => DataList.Find(x => x.BulletKey == key);
    }
}
