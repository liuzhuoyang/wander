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
        private static Dictionary<string, UnitData> dictUnitData;

        //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictUnitData = new Dictionary<string, UnitData>();
            UnitDataCollection collection = GameDataControl.Instance.Get("all_unit") as UnitDataCollection;
            foreach (var data in collection.GetUnitCollection())
            {
                dictUnitData.Add(data.name, data);
            }
        }
        public static UnitData GetUnitData(string key)
        {
            if (dictUnitData.ContainsKey(key))
                return dictUnitData[key];
            else
            {
                Debug.LogError($"未找到 {key} 的单位数据.");
                return null;
            }
        }
        public static Dictionary<string, UnitData> GetUnitDict() => dictUnitData;
    }
}