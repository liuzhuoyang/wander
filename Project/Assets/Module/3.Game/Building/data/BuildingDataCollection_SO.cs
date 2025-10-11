using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleActor.Building
{
    [CreateAssetMenu(fileName = "BuildingDataCollection_SO", menuName = "RTS_Demo/Actor/Building/BuildingDataCollection_SO")]
    public class BuildingDataCollection_SO : DataCollection<BuildingData_SO>
    {
        public override BuildingData_SO GetDataByKey(string key) => DataList.Find(x => x.m_actorKey == key);
    }
}
