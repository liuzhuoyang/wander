using UnityEngine;

namespace BattleBuff
{
    [CreateAssetMenu(fileName = "BuffDataCollection_SO", menuName = "Assets/Buff/BuffDataCollection_SO")]
    public class BuffDataCollection_SO : DataCollection<BuffData_SO>
    {
        public override BuffData_SO GetDataByKey(string key) => DataList.Find(x => x.m_buffID == key);
    }
}
