using BattleGear;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearDataCollection", menuName = "RTS_Demo/Gear/GearDataCollection")]
    public class GearDataCollection : DataCollection<GearData>
    {
        public override GearData GetDataByKey(string key) => DataList.Find(x => x.m_gearKey == key);
    }
}