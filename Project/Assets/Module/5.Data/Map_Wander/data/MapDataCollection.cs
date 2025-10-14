using System.Collections.Generic;
using UnityEngine;

namespace ProjectWander.Map
{
    [CreateAssetMenu(fileName = "all_map", menuName = "ProjectWander/Data/Map/MapCollectionData")]
    public class MapDataCollection : DataCollection<MapData>
    {
        [SerializeField] private List<MapData> allMapData;
        public override MapData GetDataByKey(string key)
        {
            return allMapData.Find(mapData => mapData.name == key);
        }
    }
}
