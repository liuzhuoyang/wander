using System.Collections.Generic;
using RTSDemo.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ProjectWander.Map
{
    [System.Serializable]
    public struct EnemyData
    {
        public UnitData unitData;
        public int unlockWave;
    }
    [CreateAssetMenu(fileName = "map_asset", menuName = "ProjectWander/Data/Map/MapData")]
    public class MapData : ScriptableObject
    {
        public int levelID;

        [BoxGroup("关卡素材")]
        public AssetReferenceGameObject mapPrefab;

        [BoxGroup("Difficulity"), Min(5)]
        public int totalWave = 15;
        [BoxGroup("Difficulity")]
        [Tooltip("定义初始单位等级, 难度的基础值，定义关卡的难度基础")]
        [Min(1)]
        public int unitStartLevel = 1;

        [BoxGroup("敌人")]
        public UnitData boss;
        [BoxGroup("敌人")]
        public List<EnemyData> enemyList;
    }
}