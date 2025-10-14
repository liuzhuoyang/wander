using UnityEngine;

namespace ProjectWander.Map
{
    public class MapDataManager : Singleton<MapDataManager>
    {
        [SerializeField] private MapDataCollection mapDataCollection;
        public MapData GetMapData(int mapID) => mapDataCollection.GetMapDataByID(mapID);
    }
}