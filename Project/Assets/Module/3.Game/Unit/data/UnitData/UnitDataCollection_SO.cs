using UnityEngine;

namespace BattleActor.Unit
{
    [CreateAssetMenu(fileName = "UnitDataCollection_SO", menuName = "RTS_Demo/Actor/Unit/UnitDataCollection_SO")]
    public class UnitDataCollection_SO : DataCollection<UnitData_SO>
    {
        public override UnitData_SO GetDataByKey(string key) => DataList.Find(x => x.m_actorKey == key);
    }
}