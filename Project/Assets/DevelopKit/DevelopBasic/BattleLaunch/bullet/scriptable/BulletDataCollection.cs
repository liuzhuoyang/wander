using UnityEngine;

namespace BattleLaunch.Bullet
{
    [CreateAssetMenu(fileName = "BulletDataCollection", menuName = "Assets/Bullet/BulletCollection")]
    public class BulletDataCollection : DataCollection<BulletData_SO>
    {
        public override BulletData_SO GetDataByKey(string key) => DataList.Find(x => x.BulletKey == key);
    }
}
