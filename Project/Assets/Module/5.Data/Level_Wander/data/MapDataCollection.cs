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
            int id = int.Parse(key);
            return allMapData.Find(mapData => mapData.levelID == id);
        }
        public MapData GetMapDataByID(int id)
        {
            return allMapData.Find(mapData => mapData.levelID == id);
        }
    }
}
