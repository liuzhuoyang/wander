using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using BattleSummon;
using HUD_TEXT;

namespace BattleActor.Unit
{
    /// <summary>
    /// 单位系统
    /// 管理战场上所有可在地图中移动的单位
    /// 管理的单位主要有
    /// 1.英雄
    /// 2.敌人
    /// 3.小兵
    /// </summary>
    public class UnitManager : Singleton<UnitManager>
    {
        [SerializeField] private UnitDataCollection_SO unitDataCollection_SO;
        [SerializeField] private UnitConfig_SO unitViewConfig_SO; //单位表现信息集

        private Transform unitRoot;
        private HashSet<UnitBase> enemyUnitList;
        private HashSet<UnitBase> playerUnitList;
        
        public HashSet<UnitBase> m_enemyUnitList => enemyUnitList;
        public HashSet<UnitBase> m_playerUnitList => playerUnitList;

        private int SpawnedCount = 0; //记录当前已创建单位的数量，作为单位的标识

        private Dictionary<string, GameObject> unitPrefabDict;

        #region 数据获取
        public Material GetHitFeedbackMat(bool isBoss)
        {
            if (isBoss) return unitViewConfig_SO.boss_Hit;
            else return unitViewConfig_SO.default_Hit;
        }
        async UniTask LoadUnitPrefab(UnitData_SO data)
        {
            //单位身体素材获取
            GameObject go = await GameAsset.GetAssetAsync<GameObject>(data.m_bodyRef);
            if (go == null)
            {
                Debug.LogError($"未找到 {data.m_actorKey} 的身体素材.");
            }
            if (!unitPrefabDict.ContainsKey(data.m_actorKey))
                unitPrefabDict.Add(data.m_actorKey, go);
        }
        #endregion

        #region 自身生命周期
        //加载初始资源，游戏进入时执行
        public async UniTask Init()
        {
            enemyUnitList = new HashSet<UnitBase>();
            playerUnitList = new HashSet<UnitBase>();

            unitPrefabDict = new Dictionary<string, GameObject>();

            await GameAsset.LoadAssets(unitDataCollection_SO.GetDataCollection(), LoadUnitPrefab);
        }
        void OnEnable()
        {
            BattleSummonEventSystem.E_OnSummonUnit += HandleSummon;
        }
        void OnDisable()
        {
            BattleSummonEventSystem.E_OnSummonUnit -= HandleSummon;
        }
        #endregion

        #region 战斗节点处理
        public void StartBattle()
        {
            SpawnedCount = 0;
            enemyUnitList = new HashSet<UnitBase>();
            playerUnitList = new HashSet<UnitBase>();
            unitRoot = new GameObject("[Unit]").transform;
        }
        public void RemoveAllUnit()
        {
            CleanUpHashSet(ref enemyUnitList);
            CleanUpHashSet(ref playerUnitList);
        }
        //战斗结束后清理
        public void CleanUpBattle()
        {
            //清理单位
            RemoveAllUnit();
            Destroy(unitRoot.gameObject);
        }
        #endregion

        #region 索敌帮助
        public bool TryGetAnyTargetInRange(out IBattleActor target, Vector2 center, float searchRadius, TeamMask teamType)
            => BattleActorScanSystem.Instance.TryGetAnyTargetInRange(out target, center, searchRadius, teamType);
        public bool TryGetClosestTargetInRange(out IBattleActor target, Vector2 center, float searchRadius, TeamMask teamType)
            => BattleActorScanSystem.Instance.TryGetClosestTargetInRange(out target, center, searchRadius, teamType);
        #endregion

        #region 召唤处理
        public void OnSummonedUnitRemoved(UnitBase unit)
        {
            BattleSummonEventSystem.Call_OnSummonneeRemoved(unit);
        }
        ISummonnee HandleSummon(BattleSummonArg unitSummonArgs) => CreateUnit(unitSummonArgs.summonneeKey, unitSummonArgs.summonPosition, unitSummonArgs.isEnemy, unitSummonArgs.summonneeLevel, true);
        #endregion

        #region Unit管理
        void CleanUpHashSet(ref HashSet<UnitBase> unitList) //ref关键词强调会清空hashset
        {
            foreach (var unit in unitList)
            {
                unit.CleanUp();
            }
            unitList.Clear();
        }
        /// <summary>
        /// 一般创建单位的方法，手动赋予敌我
        /// </summary>
        /// <param name="单位配置的名称"></param>
        /// <param name="生成地点"></param>
        /// <param name="是否为敌人"></param>
        /// <param name="单位等级"></param>
        /// <param name="是否自动启动单位"></param>
        /// <param name="伤害统计索引"></param>
        /// <param name="是否增加敌人计数器">若配置了修改器，则根据修改器配置 "与" 此配置设置的结果进行设置</param>
        /// <returns></returns>
        public UnitBase CreateUnit(string unitName, Vector3 worldPos, bool isEnemy, int unitLevel = 1, bool autoActivate = true)
        {
            //基础单位数据
            UnitData_SO unitData = unitDataCollection_SO.GetDataByKey(unitName);
            var objectArgs = new UnitObjectArgs(unitData, unitLevel);
            return CreateUnitRaw(objectArgs, worldPos, isEnemy, autoActivate);
        }
        /// <summary>
        /// 通过UnitArgs直接创建单位，此方法适用于需要在创建单位前添加全局加成的情况
        /// </summary>
        /// <param name="单位基础参数"></param>
        /// <param name="生成地点"></param>
        /// <param name="是否为敌人"></param>
        /// <param name="是否自动启动单位"></param>
        /// <param name="伤害统计索引"></param>
        /// <param name="是否增加敌人计数器">若配置了修改器，则根据修改器配置 "与" 此配置设置的结果进行设置</param>
        /// <returns></returns>
        public UnitBase CreateUnitRaw(UnitObjectArgs unitObjectArgs, Vector3 worldPos, bool isEnemy, bool autoActivate = true)
        {
            //创建单位实体
            string unitKey = unitObjectArgs.UnitName;
            GameObject unit = Instantiate(unitPrefabDict[unitKey], unitRoot);
            unit.name = unitKey + SpawnedCount.ToString("f2");
            unit.transform.position = worldPos;

            //设置单位层级
            unit.layer = isEnemy?BattleActorService.EnemyLayer:BattleActorService.FriendlyLayer;

            //初始化单位脚本
            UnitBase unitBase = unit.AddComponent<UnitBase>().Init(unitObjectArgs);

            //注册单位阵营表
            if (isEnemy)
            {
                enemyUnitList.Add(unitBase);
            }
            else
            {
                playerUnitList.Add(unitBase);
            }

            //启用单位
            if (autoActivate)
                unitBase.SwitchUnitBehavior(true);
            SpawnedCount++;

            return unitBase;
        }
        //销毁单位，可跳过单位死亡的步骤，或单位死亡后执行
        public void RemoveUnitImmediately(UnitBase unit)
        {
            if (unit.teamType == TeamMask.Player)
            {
                if (playerUnitList.Contains(unit))
                {
                    playerUnitList.Remove(unit);
                }
                else
                {
                    Debug.LogWarning($"Unit {unit.gameObject.name} has been removed or never added to player unit list");
                }
            }
            else
            {
                if (enemyUnitList.Contains(unit))
                {
                    enemyUnitList.Remove(unit);
                }
                else
                {
                    Debug.LogWarning($"Unit {unit.gameObject.name} has been removed or never added to enemy unit list");
                }
            }

            unit.CleanUp();
        }
        public void RemoveAllUnitsImmediately()
        {
            foreach (var unit in enemyUnitList)
            {
                unit.CleanUp();
            }
            enemyUnitList.Clear();
            foreach (var unit in playerUnitList)
            {
                unit.CleanUp();
            }
            playerUnitList.Clear();
        }
        //直接杀死所有单位，单位会先死亡
        public void KillAllUnits()
        {
            foreach (var unit in enemyUnitList)
            {
                if (unit == null)
                    continue;
                KillUnit(unit);
            }
            enemyUnitList.Clear();
            foreach (var unit in playerUnitList)
            {
                if (unit == null)
                    continue;
                KillUnit(unit);
            }
            playerUnitList.Clear();
        }
        //杀死某一个单位
        public void KillUnit(UnitBase unit) => unit.TakeDamage(AttackData.InstantKillAttack, Vector2.zero);
        public int GetCurrentPlayerUnitCount() => playerUnitList.Count;
        public int GetCurrentEnemyCount() => enemyUnitList.Count;
        public HashSet<UnitBase> GetEnemyList() => enemyUnitList;
        #endregion

        #region Unit HUD
        public void ShowDamage(float damage, ElementType damageType, bool isCritic, Vector2 hudPos)
        {
            if(HUD.Instance!=null)
                HUD.Instance.OnHudDamage(hudPos, new AttackResultData(damage, damageType, isCritic));
        }
        #endregion
    }
}