using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleActor.Building
{
    [CreateAssetMenu(fileName = "BuildingDataCollection", menuName = "RTS_Demo/Actor/Building/BuildingDataCollection")]
    public class BuildingDataCollection : DataCollection<BuildingData>
    {
        public override BuildingData GetDataByKey(string key) => DataList.Find(x => x.m_actorKey == key);
    }
}
