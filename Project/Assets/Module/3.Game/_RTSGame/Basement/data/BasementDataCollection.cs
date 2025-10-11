using UnityEngine;

namespace BattleActor.Basement
{
    [CreateAssetMenu(fileName = "BasementDataCollection", menuName = "RTS_Demo/Actor/Basement/BasementDataCollection")]
    public class BasementDataCollection : DataCollection<BasementData>
    {
        public override BasementData GetDataByKey(string key)
        {
            return DataList.Find(data => data.m_basementKey == key);
        }
    }
}
