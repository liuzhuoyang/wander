using UnityEngine;

namespace BattleActor.Unit
{
    [CreateAssetMenu(fileName = "UnitDataCollection", menuName = "RTS_Demo/Actor/Unit/UnitDataCollection")]
    public class UnitDataCollection : DataCollection<UnitData>
    {
        public override UnitData GetDataByKey(string key) => DataList.Find(x => x.m_actorKey == key);
    }
}