using BattleGear;
using UnityEngine;

namespace BattleGear
{
    [CreateAssetMenu(fileName = "GearDataCollection_SO", menuName = "RTS_Demo/Gear/GearDataCollection")]
    public class GearDataCollection_SO : DataCollection<GearData_SO>
    {
        public override GearData_SO GetDataByKey(string key) => DataList.Find(x => x.m_gearKey == key);
    }
}