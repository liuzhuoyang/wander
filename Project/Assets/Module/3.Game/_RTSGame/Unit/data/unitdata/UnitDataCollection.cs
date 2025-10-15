using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RTSDemo.Unit
{
    [CreateAssetMenu(fileName = "UnitDataCollection", menuName = "RTS_Demo/Actor/Unit/UnitDataCollection")]
    public class UnitDataCollection : GameDataCollectionBase
    {
        [SerializeField] protected List<UnitData> listUnitData;
        public UnitData GetDataByKey(string key) => listUnitData.Find(x => x.m_actorKey == key);
        public List<UnitData> GetUnitCollection() => listUnitData;

#if UNITY_EDITOR
        [Button("Init Data")]
        public override void InitData()
        {
            base.InitData();
            listUnitData = FileFinder.FindAllAssetsOfAllSubFolders<UnitData>(path);
        }
#endif
    }
    public static class AllUnit
    {
        public static Dictionary<string, UnitData> dictUnitData;

         //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictUnitData = new Dictionary<string, UnitData>();
            UnitDataCollection collection = GameDataControl.Instance.Get("all_audio") as UnitDataCollection;
            foreach (var data in collection.GetUnitCollection())
            {
                dictUnitData.Add(data.name, data);
            }
        }
    }
}