using System.Collections.Generic;
using UnityEngine;
using BattleSummon;
using Sirenix.OdinInspector;
using BattleLaunch;

namespace BattleActor.Building
{
    /// <summary>
    /// 能够执行战斗，但不能够移动，且能够被摧毁的实体
    /// </summary>
    public class BuildingBase : BattleBehaviour, IBattleActor, ISummonnee
    {
        [Header("非直接拖拽时，由Assets配置")]
        [SerializeField, ReadOnly] protected BuildingDynamicArgs dynamicArgs;
        [SerializeField] protected bool isActivated = false;
        [Header("生产点")]
        [SerializeField] protected Transform spawnPoint;
        [Header("发射")]
        [SerializeField] protected LaunchLayer launchLayer;

        [Header("UI")]
        [SerializeField] protected Transform HUD_Point;

        #region Component
        protected ProductionCommand productionPipeline;
        protected BuildingProductionManager productionManager;
        protected List<BuildingProductionHandler> productionHandlers; //自动生产模组，建筑物本身并不产生单位，通过外部指令生产，因此可以用自动的方式，也可以手动执行
        protected BattleLaunchTargetFinder targetFinder;
        protected Collider2D hitBox;
        #endregion

        #region Summon
        private string summonnerID = null;
        public string m_summonnerID => summonnerID;
        public string m_summonneeName => gameObject.name;
        #endregion

        protected bool isDestroied = false;
        protected bool isBattleBuilding;

        public TeamMask teamType { get; protected set; }
        public float currentHealthRatio => currentHealth / dynamicArgs.maxHealth;
        public float currentShield
        {
            get => dynamicArgs.currentShield;
            set => dynamicArgs.currentShield = value;
        }
        public float currentHealth
        {
            get => dynamicArgs.currentHealth;
            set => dynamicArgs.currentHealth = value;
        }
        public float productionSpeed => dynamicArgs.currentProduceSpeed;
        public float autoProduceSpeed => dynamicArgs.autoProduceSpeed;
        public BattleActorMotionLayer motionLayer => BattleActorMotionLayer.Ground;
        public BattleActorType battleActorType => BattleActorType.Building;
        public bool IsTargetable => !isDestroied && isActivated;
        public bool IsPlayerSide => gameObject.layer == BattleActorService.FriendlyLayer;
        public Vector2 position => hitBox.bounds.center;
        public bool IsDead => isDestroied;
        public int currentLevel => dynamicArgs.level;

        protected BattleLaunchControl battleLaunchControl;
        protected float launchTime;
        protected const int MAX_QUEUE_SIZE = 16;

        #region 建筑生命周期
        public BuildingBase Init(BuildingData buildingData, int buildingLevel, bool activateOnStart = false)
        {
            teamType = gameObject.layer == BattleActorService.FriendlyLayer ? TeamMask.Player : TeamMask.Enemy;
            isBattleBuilding = buildingData.launchConfigs != null;

            name = gameObject.name + Time.time.ToString("f2");
            dynamicArgs = new BuildingDynamicArgs(buildingData, buildingLevel);

            //生产模组初始化
            var productionList = dynamicArgs.productionArgs;
            if (productionList != null && productionList.Length > 0)
            {
                productionHandlers = new List<BuildingProductionHandler>();
                BuildingProductionHandler productionHandler = null;
                for (int i = 0; i < productionList.Length; i++)
                {
                    productionHandler = gameObject.AddComponent<BuildingProductionHandler>();
                    productionHandler.Init(this, productionList[i]);
                    productionHandlers.Add(productionHandler);
                }
            }
            if (isBattleBuilding)
            {
                battleLaunchControl = gameObject.AddComponent<BattleLaunchControl>();
                targetFinder = gameObject.AddComponent<BattleLaunchTargetFinder>();
                targetFinder.Init(BattleActorService.GetOppositeTeam(teamType), ActorScanOrder.Default, BattleActorMotionLayerMask.All);
            }

            hitBox = GetComponent<Collider2D>();

            if (activateOnStart)
            {
                SwitchBuilding(true);
            }
            return this;
        }

        public override void BattleUpdate()
        {
            if (!isActivated) return;
            //生命值检测
            if (currentHealth <= 0)
            {
                if (!isDestroied)
                {
                    isDestroied = true;
                    OnBroken();
                }
            }
            //生产模块更新，生产模块负责动态添加生产命令，实际执行生产是由productionManager负责
            if (productionHandlers != null && productionHandlers.Count > 0)
            {
                for (int i = productionHandlers.Count - 1; i >= 0; i--)
                {
                    productionHandlers[i].UpdateAutoProduction();
                }
            }

            //生产管线更新。
            if (productionManager != null)
                productionManager.UpdateCommand();

            if (isBattleBuilding)
            {
                launchTime -= Time.deltaTime * dynamicArgs.currentAttackSpeed;
                if (launchTime <= 0)
                {
                    launchTime = 1;
                    targetFinder.FlushTarget(dynamicArgs.currentAttackRange, dynamicArgs.launchData.retargetPerLaunch ? (int)dynamicArgs.launchData.burstCount.cachedValue : 1);
                    var targets = targetFinder.GetActiveTargetList();
                    if (targets.Count > 0)
                    {
                        BattleLaunchCommand_Batch launchBatch = new BattleLaunchCommand_Batch(dynamicArgs.launchData, GetAttackData(), launchLayer.launchTrans);
                        launchBatch.AssignTargets(targets);
                        launchBatch.ExcludeTeam(teamType);
                        battleLaunchControl.AddLaunch(launchBatch);
                    }
                }
                battleLaunchControl.UpdateLaunching();
            }
        }
        void OnBroken()
        {
            if (productionManager != null)
            {
                productionManager.AbortCommands();
            }
            BuildingManager.Instance.RemoveBuilding(this);
        }
        public void CleanUp()
        {
            if (productionManager != null && productionManager.isBusy)
                productionManager.AbortCommands();
            if (battleLaunchControl != null)
                battleLaunchControl.AbortLaunch();
            Destroy(gameObject);
        }
        protected override void OnDestroy()
        {
            //触发召唤物死亡事件
            if (!string.IsNullOrEmpty(m_summonnerID))
            {
                BuildingManager.Instance.OnSummonedBuildingRemoved(this);
            }
            base.OnDestroy();
        }
        public void SwitchBuilding(bool _isActivated) => this.isActivated = _isActivated;
        #endregion

        #region 索敌支持
        public bool IsPosInRange(Vector2 pos, float range) => GeometryUtil.IsPointInCircle(pos, position, range);
        public float GetSqDistanceTo(Vector2 pos) => (pos - position).sqrMagnitude;
        public float GetDistanceTo(Vector2 pos) => (pos - position).magnitude;
        public Vector2 GetClosestPointTo(Vector2 pos) => hitBox.ClosestPoint(pos);
        public Vector2 GetHitPos(Vector2 attackObjPos) => position;
        #endregion

        #region 战斗支持
        public void TakeDamage(AttackData attackData, Vector2 hitPos)
        {
            float damage = attackData.damage;
            bool isCrit = attackData.criticRate > Random.value;
            if (isCrit)
            {
                damage *= AttackData.BASIC_CRITIC_MULTI + attackData.criticDamageMulti;
            }
            damage += attackData.damageToBuilding;
            damage += Mathf.Min(attackData.damageToShield, currentShield);
            //拥有护盾，添加对护盾增伤，增伤不超过护盾本身强度。
            if (currentShield > 0)
            {
                currentShield -= damage;
                if (currentShield < 0)
                {
                    damage = -currentShield;
                    currentShield = 0;
                }
                else
                {
                    damage = 0;
                }
            }
            currentHealth -= damage;
        }
        protected AttackData GetAttackData()
        {
            float baseDamage = dynamicArgs.currentDamage;
            return new AttackData(baseDamage,
                                  baseDamage * dynamicArgs.damageMultiToBuilding,
                                  baseDamage * dynamicArgs.damageMultiToShield,
                                  dynamicArgs.penetrateArmor,
                                  dynamicArgs.currentCriticRate,
                                  dynamicArgs.currentCriticDamageMultiplier,
                                  dynamicArgs.elementType);
        }
        #endregion

        #region 单位生产
        public virtual ISummonnee SpawnBattleActor(BattleActorData summoneeData, int summonLevel, int summonLimit)
        {
            return BattleSummonManage.Instance.TryCreateSummon(new BattleSummonArg(summoneeData,
                                                               summonLimit,
                                                               (Vector2)spawnPoint.position + Random.insideUnitCircle * 0.1f,
                                                               gameObject.name + summoneeData.m_actorKey,
                                                               teamType == TeamMask.Enemy, summonLevel));
        }
        public void AddProductionOrder(in ProductionArgs productionArgs)
        {
            if (productionManager == null)
                productionManager = new BuildingProductionManager(this);

            if (productionPipeline == null)
                productionPipeline = new ProductionCommand(productionArgs);
            else
                productionPipeline = productionPipeline.AppendCommand(new ProductionCommand(productionArgs)) as ProductionCommand;
                
            if (!productionManager.isBusy) productionManager.AddCommand(productionPipeline);
        }
        public void EndProduction(BuildingProductionHandler handler) => productionHandlers.Remove(handler); //移除一条生产线
        #endregion

        #region 召唤支持
        public void OnSummon(string summonnerID) => this.summonnerID = summonnerID;
        public void ApplySummonModify<T>(T buffArgs)
        {

        }
        #endregion

        #region Debug
        [Button("Refresh Data")]
        public void RefreshData()
        {
            dynamicArgs.RefreshData();
        }
        #endregion
    }
}