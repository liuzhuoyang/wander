using UnityEngine;

namespace ProjectWander.Map
{
    public class MapDataManager : Singleton<MapDataManager>
    {
        [SerializeField] private MapDataCollection mapDataCollection;
    }
}