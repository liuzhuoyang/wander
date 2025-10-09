using System.Collections.Generic;
using BattleSummon;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BattleActor.Building
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] private BuildingDataCollection_SO buildingDataCollection;

        private Transform buildingRoot;
        private HashSet<BuildingBase> playerBuildings;
        private HashSet<BuildingBase> enemyBuildings;
        private Dictionary<string, GameObject> buildingPrefabDict;

        #region 数据获取
        async UniTask LoadBuildingPrefab(BuildingData_SO data)
        {
            //单位身体素材获取
            GameObject go = await GameAssets.GetAssetAsync<GameObject>(data.bodyRef);
            if (go == null)
            {
                Debug.LogError($"未找到 {data.m_actorKey} 的身体素材.");
            }
            if (!buildingPrefabDict.ContainsKey(data.m_actorKey))
                buildingPrefabDict.Add(data.m_actorKey, go);
        }
        #endregion

        #region 生命周期
        //进入游戏后，初始化
        public async UniTask Init()
        {
            enemyBuildings = new HashSet<BuildingBase>();
            playerBuildings = new HashSet<BuildingBase>();

            buildingPrefabDict = new Dictionary<string, GameObject>();
            await GameAssets.LoadAssets(buildingDataCollection.GetDataCollection(), LoadBuildingPrefab);
        }
        void OnEnable()
        {
            BattleSummonEventSystem.E_OnSummonBuilding += HandleSummonBuilding;
        }
        void OnDisable()
        {
            BattleSummonEventSystem.E_OnSummonBuilding -= HandleSummonBuilding;
        }
        #endregion

        #region 建筑物管理
        public void RemoveAllBuilding()
        {
            CleanUpHashSet(ref playerBuildings);
            CleanUpHashSet(ref enemyBuildings);
        }
        void CleanUpHashSet(ref HashSet<BuildingBase> constructList) //ref关键词强调会清空hashset
        {
            foreach (var construct in constructList)
            {
                construct.CleanUp();
            }
            constructList.Clear();
        }
        public BuildingBase CreateBuilding(string buildingKey, int buildingLevel, Vector2 worldPos, bool isEnemy, bool autoActivate = true)
        {
            BuildingData_SO buildingData = buildingDataCollection.GetDataByKey(buildingKey);

            GameObject buildingObj = Instantiate(buildingPrefabDict[buildingData.m_actorKey], buildingRoot);
            buildingObj.transform.position = worldPos;
            buildingObj.layer = isEnemy ? BattleActorService.EnemyLayer : BattleActorService.FriendlyLayer;

            BuildingBase building = buildingObj.GetComponent<BuildingBase>();
            if (building == null)
            {
                building = buildingObj.AddComponent<BuildingBase>();
            }
            building.Init(buildingData, buildingLevel, autoActivate);
            
            //注册建筑物
            if (building.IsPlayerSide)
            {
                playerBuildings.Add(building);
            }
            else
            {
                enemyBuildings.Add(building);
            }

            return building;
        }
        public void RemoveBuilding(BuildingBase construct)
        {
            if (construct.IsPlayerSide)
            {
                playerBuildings.Remove(construct);
            }
            else
            {
                enemyBuildings.Remove(construct);
            }
            construct.CleanUp();
        }
        #endregion

        #region 战斗节点处理
        //进入战斗时
        public void StartBattle()
        {
            enemyBuildings = new HashSet<BuildingBase>();
            playerBuildings = new HashSet<BuildingBase>();
            buildingRoot = new GameObject("[Building]").transform;
        }
        //退出战斗时
        public void CleanUpBattle()
        {
            RemoveAllBuilding();
            Destroy(buildingRoot.gameObject);
        }
        #endregion

        #region 召唤支持
        public void OnSummonedBuildingRemoved(BuildingBase building)
        {
            BattleSummonEventSystem.Call_OnSummonneeRemoved(building);
        }
        ISummonnee HandleSummonBuilding(BattleSummonArg buildingSummonArg)
            => CreateBuilding(buildingSummonArg.summonneeKey, buildingSummonArg.summonneeLevel, buildingSummonArg.summonPosition, buildingSummonArg.isEnemy, true);
        #endregion

        #region Debug Function
        public void DebugOnCreateConstruct(BuildingBase construct)
        {
            if (construct.IsPlayerSide)
            {
                playerBuildings.Add(construct);
            }
            else
            {
                enemyBuildings.Add(construct);
            }
            construct.SwitchBuilding(true);
        }
        #endregion
    }
}
