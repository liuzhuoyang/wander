using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

using BattleLaunch;
using RTSDemo.Grid;
using BattleMap.Grid;
using BattleSummon;

namespace BattleGear
{
    /// <summary>
    /// Gear战斗管理，该管理只负责支持战斗内的Gear部分
    /// </summary>
    public class GearManager : Singleton<GearManager>
    {
        [SerializeField] private GearDataCollection gearDataCollection;
        [SerializeField] private GearDamageBonusData gearDamageBonus;

        private Transform gearRoot;
        private HashSet<GearBase> playerGears;
        private Dictionary<string, GameObject> gearPrefabDict; //记录武器prefab
        private Dictionary<string, GridShape> gearShapeDict; //记录武器形状

        #region 数据获取
        async UniTask LoadGearPrefab(GearData data)
        {
            //单位身体素材获取
            GameObject go = await GameAsset.GetAssetAsync<GameObject>(data.gearPrefab);
            if (go == null)
            {
                Debug.LogError($"未找到 {data.m_gearKey} 的身体素材.");
            }
            if (!gearPrefabDict.ContainsKey(data.m_gearKey))
                gearPrefabDict.Add(data.m_gearKey, go);

            if (!gearShapeDict.ContainsKey(data.m_gearKey))
                gearShapeDict.Add(data.m_gearKey, data.gearShape);
        }
        public float GetGearDamageByLevel(string gearKey, int level) => gearDamageBonus.GetGearDamageByLevel(gearKey, level);
        public async Task<Sprite> GetGearIcon(string gearKey) => await GameAsset.GetSpriteAsync(GameAsset.ICON_TAG + gearKey);
        #endregion

        #region 自身生命周期
        //加载初始资源，游戏进入时执行
        public async UniTask Init()
        {
            gearPrefabDict = new Dictionary<string, GameObject>();
            gearShapeDict = new Dictionary<string, GridShape>();

            await GameAsset.LoadAssets(gearDataCollection.GetDataCollection(), LoadGearPrefab);
        }
        #endregion

        #region 战斗节点
        public void StartBattle()
        {
            playerGears = new HashSet<GearBase>();
            gearRoot = new GameObject("[Gears]").transform;
        }
        public void RemoveAllGear()
        {
            CleanUpHashSet(ref playerGears);
        }
        //战斗结束后清理
        public void CleanUpBattle()
        {
            //清理单位
            RemoveAllGear();
            Destroy(gearRoot.gameObject);
        }
        #endregion

        #region Gears管理
        void CleanUpHashSet(ref HashSet<GearBase> gearList) //ref关键词强调会清空hashset
        {
            foreach (var gear in gearList)
            {
                gear.CleanUp();
            }
            gearList.Clear();
        }
        public Vector2 GetGearPlaceOffset(string gearKey) => gearDataCollection.GetDataByKey(gearKey).placeOffset;
        public GearBase CreateGear(string gearKey, Vector2 worldPos, int gearLevel)
        {
            GearData gearData = gearDataCollection.GetDataByKey(gearKey);
            GearDynamicArgs gearDynamicArgs = new GearDynamicArgs(gearData, gearLevel);

            GameObject gearObj = Instantiate(gearPrefabDict[gearKey], gearRoot);
            gearObj.transform.position = worldPos + gearData.placeOffset;

            //创建gear
            GearBase gear = null;
            switch (gearData.gearType)
            {
                case GearType.LaunchGear:
                    gearObj.AddComponent<BattleLaunchTargetFinder>();
                    var gear_launch = gearObj.AddComponent<Gear_Launch>();
                    gear_launch.Init(gearDynamicArgs);
                    gear_launch.InitLaunchData((gearData as GearDataLaunch).launchConfig);
                    gear = gear_launch;
                    break;
                case GearType.SummonGear:
                    var gear_summon = gearObj.AddComponent<Gear_Summon>();
                    gear_summon.Init(gearDynamicArgs);
                    gear_summon.InitSummonArg((gearData as GearDataSummon).summonData_SO);
                    gear = gear_summon;
                    break;
            }

            //注册gear
            playerGears.Add(gear);
            return gear;
        }
        #endregion

        #region 召唤支持
        public ISummonnee TrySummon(BattleSummonArg summonArg)
        {
            return BattleSummonManage.Instance.TryCreateSummon(summonArg);
        }
        #endregion

        #region Grid支持
        public bool IsGearPlaceableOnGridPoint(string gearKey, Vector2Int center)
        {
            var shape = gearShapeDict[gearKey];
            var localGridPoints = GridService.ShapeToGridOffset(shape);

            foreach (var offset in localGridPoints)
            {
                Vector2Int gridPoint = offset + center;
                if (!RTSGridWorldSystem.Instance.GetGridNode(gridPoint).isMountable)
                {
                    return false;
                }
            }
            return true;
        }
        public Vector2 GridPointToWorldPos(Vector2Int gridPoint) => RTSGridWorldSystem.Instance.GetWorldPosFromGrid(gridPoint);
        public Vector2Int WorldPosToGridPoint(Vector2 wrdPos) => RTSGridWorldSystem.Instance.GetGridPointFromWorld(wrdPos);
        #endregion
    }
}