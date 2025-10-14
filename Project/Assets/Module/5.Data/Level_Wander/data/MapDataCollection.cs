using System.Collections.Generic;
using UnityEngine;

namespace ProjectWander.Map
{
    [CreateAssetMenu(fileName = "all_map", menuName = "ProjectWander/Data/Map/MapCollectionData")]
    public class MapDataCollection : GameDataCollectionBase
    {
        [SerializeField] private List<MapData> allMapData;
    }
}
