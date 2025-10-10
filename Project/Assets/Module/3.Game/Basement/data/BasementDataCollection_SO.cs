using UnityEngine;

namespace BattleActor.Basement
{
    [CreateAssetMenu(fileName = "AllBasementData", menuName = "RTS_Demo/Actor/Basement/BasementDataCollection_SO")]
    public class BasementDataCollection_SO : DataCollection<BasementData_SO>
    {
        public override BasementData_SO GetDataByKey(string key)
        {
            return DataList.Find(data => data.m_basementKey == key);
        }
    }
}
