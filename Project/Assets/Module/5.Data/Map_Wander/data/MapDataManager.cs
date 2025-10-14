using UnityEngine;

namespace ProjectWander.Map
{
    public class MapDataManager : Singleton<MapDataManager>
    {
        [SerializeField] private MapDataCollection mapDataCollection;
        public MapData GetMapData(int chapterID, int levelID)
        {
            return mapDataCollection.GetDataByKey(GetMapKey(chapterID, levelID));
        }
        public static string GetMapKey(int chapterID, int levelID)
        {
            return $"map_{chapterID:D3}_{levelID:D3}";
        }
    }
}