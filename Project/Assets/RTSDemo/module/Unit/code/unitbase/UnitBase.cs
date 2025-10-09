using System;
using UnityEngine;

using BattleLaunch;
using BattleSummon;
using BattleBuff;

using Random = UnityEngine.Random;

namespace BattleActor.Unit
{
    /// <summary>
    /// 能够执行战斗，并能够在战场内移动的，与被消灭的复杂实体
    /// /// </summary>
    public class UnitBase : BattleBehaviour, IBattleActor, ISummonnee
    {
        [SerializeField] protected UnitDynamicArgs dynamicArgs; //单位动态战斗数据，一般会实时修改
        protected UnitObjectArgs objectArgs; //单位进入战场后的基础战斗数据，无法修改

        #region 基础组件
        public UnitMovement unitMovement;
        protected Collider2D hitBox;
        protected Rigidbody2D rigid2D;
        protected IUnitBehaviour unitBehavior;
        protected UnitViewBasic unitViewBasic;
        protected BuffHandler buffHandler;
        #endregion

        public TeamMask teamType { get; protected set; }
        protected float lastAttackTime = 1; //标准化攻击间隔，1为最大攻击间隔，0为没有攻击间隔
        protected float reloadTimer = 0; //装填计时器
        protected bool isAttacking = false;
        protected bool isDead = false;
        protected bool isSpawned = false;
        protected bool isActivated = false;
        protected IBattleActor trackingTarget; //当前攻击目标

        public bool IsTargetable => gameObject != null && !IsDead && isActivated && isSpawned; //是否是可以被瞄准的目标，由于IBattleActor可能晚于gameobject回收，需要判断gameobject是否为空
        public bool IsAttacking => isAttacking; //是否正在攻击
        public bool needReload => objectArgs.HasAmmoLimit && dynamicArgs.currentAmmo == 0;
        public bool IsRange => objectArgs.IsRange;
        public bool IsDead => isDead;
        public float hitBoxRadius => hitBox.bounds.size.x * 0.5f; //hitbox 大小的一半作为半径，hitbox 大小会随单位本身大小改变而自动改变
        public string UnitKey => objectArgs.UnitName;
        public float currentHealth => dynamicArgs.currentHealth;
        public float currentMoveSpeed => dynamicArgs.currentMoveSpeed;
        public float currentAttackRange => dynamicArgs.currentAttackRange.cachedValue;
        public float maxHealth => dynamicArgs.currentMaxHealth.cachedValue;
        public float currentHealthRatio => dynamicArgs.healthRatio;
        public float ArmorRatio => dynamicArgs.armorRatio;
        public float ShieldRatio => dynamicArgs.shieldRatio;
        public BuffHandler m_buffHandler => buffHandler;
        public BattleActorType battleActorType => BattleActorType.Unit;
        public UnitViewBasic m_unitViewBasic => unitViewBasic;
        public Vector2 position => hitBox.bounds.center; //单位的中心位置
        public UnitRace unitRace => objectArgs.Race;
        public BattleActorMotionLayer motionLayer => objectArgs.MotionLayer;
        public BattleActorMotionLayerMask attackLayer => objectArgs.AttackLayer;
        public ElementType damageType => objectArgs.DamageElementType;
        public int currentLevel => objectArgs.Level;

        #region 召唤参数
        private string summonnerID = null;
        public string m_summonnerID => summonnerID;
        public string m_summonneeName => gameObject.name;
        #endregion

        #region 单位事件
        public event Action OnUnitSpawn; //当单位初始时触发
        public event Action OnUnitHealthZero; //当单位血量归零时触发，注意，此时可能触发某类效果，导致单位无法死亡
        public event Action OnPostUnitDie; //当单位死亡之后，且执行了移除后触发，比OnUnitDie更晚执行
        public event Action OnUnitEnterAttackMode; //当进入攻击模式时触发
        public event Action OnUnitGetHit; //单位受到攻击时触发
        public event Action<BattleHitData> OnUnitHitTarget; //实际命中单位触发
        public event Action OnUnitAttackExcute; //当单位执行攻击动作时触发，注意，执行攻击动作时，不代表立马就能攻击到目标
        public event Action<IBattleActor> OnFindTargetUnit; //当单位寻找到攻击目标时触发
        #endregion

        #region 生命周期
        //利用单位元数据与等级进行初始化
        public UnitBase Init(UnitObjectArgs args)
        {
            teamType = gameObject.layer == BattleActorService.FriendlyLayer ? TeamMask.Player : TeamMask.Enemy;
            //初始化实例基础数据
            objectArgs = args;
            dynamicArgs = new UnitDynamicArgs(objectArgs);

            //初始化组件
            unitMovement = gameObject.AddComponent<UnitMovement>();
            buffHandler = gameObject.AddComponent<BuffHandler>();
            unitViewBasic = gameObject.GetComponent<UnitViewBasic>();
            hitBox = gameObject.GetComponent<Collider2D>();
            rigid2D = gameObject.GetComponent<Rigidbody2D>();
            //设置physics位置否则phsyics可能还处于worldPosition Zero
            rigid2D.position = transform.position;
            //空中单位不触发碰撞，注意之后添加buff或被索敌的方式
            if (motionLayer == BattleActorMotionLayer.Air)
            {
                hitBox.isTrigger = true;
            }

            //初始化技能
            if (dynamicArgs.abilityIDs != null && dynamicArgs.abilityIDs.Length > 0)
            {
                foreach (var ability in dynamicArgs.abilityIDs)
                    m_buffHandler.TryAddBuff(ability);
            }

            //初始化行为组件，若没有预设组件，则自动创建组件
            unitBehavior = gameObject.GetComponent<IUnitBehaviour>();

            unitMovement.Init(rigid2D);
            unitViewBasic.Init(this);
            unitBehavior.Init(this);

            //初始化状态
            RefreshMoveSpeed(dynamicArgs.currentMoveSpeed);
            RefreshAttackSpeed();

            //自定义初始化方式
            OnInit();

            return this;
        }
        public void CleanUp()
        {
            unitBehavior.CleanUp();
            unitViewBasic.CleanUp();
            isAttacking = false;
            isSpawned = false;

            OnCleanUp();
            Destroy(gameObject);
        }
        protected override void OnDestroy()
        {
            //清空单位事件
            OnUnitSpawn = null;
            OnUnitHealthZero = null;
            OnPostUnitDie = null;
            OnUnitHitTarget = null;
            OnUnitEnterAttackMode = null;
            OnUnitGetHit = null;
            OnFindTargetUnit = null;
            OnUnitAttackExcute = null;
            //触发召唤物死亡事件
            if (!string.IsNullOrEmpty(m_summonnerID) && UnitManager.Instance!=null)
            {
                UnitManager.Instance.OnSummonedUnitRemoved(this);
            }
            buffHandler.CleanUpAllBuff();
            base.OnDestroy();
        }
        public override void BattleUpdate()
        {
            if (!isActivated) return;
            if (!isSpawned)
            {
                if (unitViewBasic.IsSpawnAnimationFinish())
                {
                    isSpawned = true;
                    OnUnitSpawn?.Invoke();
                    return;
                }
                return;
            }
            if (isDead)
            {
                //如果编辑时候配置了播放死亡动画，则播放完死亡动画后才调用OnKill
                //一般情况只有大boss才需要配置，小怪直接干掉播放特效加快节奏
                if (unitViewBasic.PlayDeathAnimation)
                {
                    if (unitViewBasic.IsDeathAnimationFinish())
                    {
                        OnKill();
                    }
                }
                else
                {
                    OnKill();
                }
                //单位死亡时不会执行剩余部分
                return;
            }
            UpdateAttack(); //更新攻击

            unitBehavior.UnitUpdate(); //更新单位行为
            unitViewBasic.UpdateView(); //更新单位的View
        }
        public override void BattleFixedUpdate()
        {
            if(!isDead)
                unitMovement.UpdateMovement();
        }
        //在LateUpdate中检查死亡状态，因为有些事件提前与单位死亡触发，可能导致单位不再会死亡
        public override void BattleLateUpdate()
        {
            if (dynamicArgs.currentHealth <= 0 && !isDead)
            {
                CheckDeath();
            }
        }
        protected virtual void OnInit() { }
        protected virtual void OnCleanUp() { }
        #endregion

        #region 其他
        public void SwitchBody(bool isOn) => unitViewBasic.SwitchBody(isOn); //用于切换单位的显示与隐藏，但不关闭单位行为
        public void SwitchUnitBehavior(bool isEnable) => isActivated = isEnable;
        public void DisableTriggerBox() { hitBox.enabled = false; }
        public void EnableTriggerBox() { hitBox.enabled = true; }
        #endregion

        #region 索敌支持
        /// <summary>
        /// 检测目标是否在攻击范围内，考虑了单位大小与攻击方式
        /// 近战单位的攻击范围按1.5倍hitbox计算，远程单位的攻击范围按hitbox+攻击范围*缩放系数计算
        /// </summary>
        /// <param name="target">目标单位</param>
        /// <param name="shrinkFactor">攻击范围缩减系数，默认为UNIT_ATTACK_RANGE_SHRINK</param>
        /// <returns>是否在攻击范围内</returns>
        public bool IsActorInAttackRange(IBattleActor target, float shrinkFactor = UnitService.UNIT_ATTACK_RANGE_SHRINK)
        {
            Vector2 unitPos = target.GetClosestPointTo(position);
            float combineRange = hitBoxRadius; //补正敌方单位和自身单位大小
            if (!IsRange)
            {
                combineRange = hitBoxRadius * 1.5f; //近战单位的攻击范围按1.5倍hitbox计算
            }
            else
            {
                combineRange += dynamicArgs.currentAttackRange.cachedValue * shrinkFactor;
            }
            return IsPosInRange(unitPos, combineRange);
        }

        public bool IsPosInRange(Vector2 targetPos, float range) => GeometryUtil.IsPointInCircle(targetPos, position, range);
        public float GetSqDistanceTo(Vector2 pos) => (pos - position).sqrMagnitude;
        public float GetDistanceTo(Vector2 pos) => (pos - position).magnitude;
        public Vector2 GetClosestPointTo(Vector2 pos) => (pos - position).normalized * hitBoxRadius + position;
        public Vector2 GetHitPos(Vector2 attackObjPos) => position;
        public bool TrySearchOpponentActor(out IBattleActor enemy, float searchRadius, bool randomPick = false)
        {
            TeamMask oppositeTeamType = BattleActorService.GetOppositeTeam(teamType);
            //扫描范围内是否有敌人
            if (randomPick)
            {
                if (UnitManager.Instance.TryGetAnyTargetInRange(out enemy, position, searchRadius, oppositeTeamType))
                {
                    OnFindTargetUnit?.Invoke(enemy);
                    trackingTarget = enemy;
                    return true;
                }
            }
            else
            {
                if (UnitManager.Instance.TryGetClosestTargetInRange(out enemy, position, searchRadius, oppositeTeamType))
                {
                    OnFindTargetUnit?.Invoke(enemy);
                    trackingTarget = enemy;
                    return true;
                }
            }
            trackingTarget = null;
            enemy = null;
            return false;
        }
        #endregion

        #region 移动支持
        public void SwitchMovementModule(bool isOn) => unitMovement.SwitchModule(isOn);
        public void StopMotion()
        {
            unitViewBasic.PlayIdleAnimation();
            unitMovement.StopMovement();
        }
        public void StartMoving()
        {
            unitViewBasic.PlayRunAnimation();
            unitMovement.BeginMovement();
        }
        protected void RefreshMoveSpeed(float speed)
        {
            unitMovement.SetMoveSpeed(speed);
            if (objectArgs.MoveSpeed == 0)
            {
                unitViewBasic.SetMoveAnimationSpeed(speed);
            }
            else
            {
                unitViewBasic.SetMoveAnimationSpeed(speed / objectArgs.MoveSpeed);
            }
        }
        protected void RefreshAttackSpeed()
        {
            if (objectArgs.AttackSpeed != 0)
            {
                unitViewBasic.SetAttackAnimationSpeed(dynamicArgs.currentAttackSpeed / objectArgs.AttackSpeed);
            }
        }
        protected void MoveToPoint(Vector2 targetPoint)
        {
            Vector2 diff = targetPoint - (Vector2)transform.position;
            unitMovement.SetVelocityVector(diff);
        }
        public void MoveToActor(IBattleActor target)
        {
            switch (target.battleActorType)
            {
                case BattleActorType.Unit:
                    MoveToPoint(target.position);
                    break;
                case BattleActorType.Building:
                    MoveToPoint(target.GetClosestPointTo(position));
                    break;
                case BattleActorType.Basement:
                    MoveToPoint(target.GetClosestPointTo(position));
                    break;
            }
        }
        #endregion

        #region 战斗支持
        protected void OnKill()
        {
            UnitManager.Instance.RemoveUnitImmediately(this);
            OnPostUnitDie?.Invoke();
        }
        public void AE_Attack()
        {
            if (isAttacking)
            {
                OnUnitAttackExcute?.Invoke(); //此方法由动画帧调用，直接调用攻击采用AttackAtUnit或AttackAtPos方法
            }
        }
        //默认进入攻击后，不改变攻击倒计时
        public void StartAttack() => StartAttack(lastAttackTime / dynamicArgs.currentAttackSpeed);
        //进入攻击时的攻击倒计时
        public void StartAttack(float attackTime)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                lastAttackTime = attackTime * dynamicArgs.currentAttackSpeed; //进入攻击状态后，立马攻击
                OnUnitEnterAttackMode?.Invoke();
            }
        }
        public void StopAttack()
        {
            if (isAttacking)
            {
                isAttacking = false;
            }
        }
        protected void UpdateAttack()
        {
            if (!isDead)
            {
                if (needReload && dynamicArgs.currentReloadSpeed > 0)
                {
                    reloadTimer += Time.deltaTime * dynamicArgs.currentReloadSpeed;
                    if (reloadTimer >= 1)
                    {
                        dynamicArgs.currentAmmo = dynamicArgs.currentMaxAmmo;
                        reloadTimer = 0;
                    }
                    return;
                }
                if (isAttacking)
                {
                    lastAttackTime -= Time.deltaTime * dynamicArgs.currentAttackSpeed;
                    if (lastAttackTime <= 0)
                    {
                        unitViewBasic.PlayAttackAnimation();
                        lastAttackTime = 1;
                    }
                }
            }
        }
        public void AttackAtActor(IBattleActor targetActor)
        {
            //当目标不存在时，舍弃
            if (IBattleActor.IsInvalid(targetActor))
                return;
            if (IsRange)
            {
                //没有配置子弹时，不发射，但仍然会记录弹药与更新朝向
                if (objectArgs.HasBullet)
                {
                    BattleLaunchCommand_Direct launch = new BattleLaunchCommand_Direct(objectArgs.bulletData, GetAttackData(), unitViewBasic.GetLaunchTrans(), targetActor);
                    launch.OnHitTarget(OnUnitHitTarget);
                    BattleLaunchControl.ExcuteSingleLaunchImmediate(launch);
                }

                //更新弹药量
                if (objectArgs.HasAmmoLimit)
                {
                    dynamicArgs.currentAmmo--;
                }
            }
            else
            {
                //近战攻击的攻击点，以被攻击者对于攻击者最近的位置计算
                targetActor.TakeDamage(GetAttackData(), targetActor.GetHitPos(position));
                HitCallback(new BattleHitData(gameObject, targetActor, targetActor.position, false));
            }
            //更新单位朝向
            Vector2 diff = targetActor.position - position;
            unitViewBasic.UpdateFacing(diff);
        }
        protected AttackData GetAttackData()
        {
            return new AttackData(dynamicArgs.currentDamage.cachedValue,
                                  objectArgs.Damage * objectArgs.DamageMultiToStructure,
                                  objectArgs.Damage * objectArgs.DamageMultiToShield,
                                  objectArgs.PenetrateArmor,
                                  dynamicArgs.currentCriticRate.cachedValue,
                                  dynamicArgs.currentCriticDamageMulti.cachedValue,
                                  damageType);
        }
        protected void HitCallback(BattleHitData hitData) => OnUnitHitTarget?.Invoke(hitData); //攻击实际击中指定位置时触发
        protected void CheckDeath()
        {
            if (!isDead)
            {
                isDead = true;
                OnUnitHealthZero?.Invoke();
                DisableTriggerBox();

                if (isAttacking)
                    StopAttack();

                unitViewBasic.ResetHitFeedback();
                unitViewBasic.PlayDieAnimation();
                unitMovement.SetVelocityVector(Vector2.zero);
            }
        }
        public void TakeDamage(AttackData attackData, Vector2 hitPos)
        {
            if (isDead) return; //死亡后不再计算伤害
            float attackDamage = attackData.damage;
            bool isCrit = attackData.criticRate > Random.value;
            if (isCrit)
            {
                attackDamage *= AttackData.BASIC_CRITIC_MULTI + attackData.criticDamageMulti;
            }

            float shield = dynamicArgs.currentShield;

            //护盾增伤不超过护盾值
            attackDamage += Mathf.Min(shield, attackData.damageToShield);

            //根据当前护甲值计算伤害,目前修改为只要有护甲 就减少50%伤害
            if (!attackData.penetrateArmor && dynamicArgs.currentArmor > 0)
            {
                attackDamage *= 0.5f;
            }

            float totalDamage = attackDamage;
            //伤害统计 后续性能测试
            float tempDamage = dynamicArgs.currentHealth + dynamicArgs.currentShield > attackDamage ? attackDamage : dynamicArgs.currentHealth + dynamicArgs.currentShield;

            //有护盾，扣除护盾值，计算额外伤害
            if (dynamicArgs.currentShield > 0)
            {
                dynamicArgs.currentShield -= attackDamage;
                if (dynamicArgs.currentShield < 0)
                {
                    attackDamage = -dynamicArgs.currentShield;
                    dynamicArgs.currentShield = 0;
                }
                else
                {
                    attackDamage = 0;
                }
            }

            //计算生命值
            dynamicArgs.currentHealth -= attackDamage;
            OnUnitGetHit?.Invoke();

            //死亡检测
            if (dynamicArgs.currentHealth <= 0)
            {
                OnUnitHealthZero?.Invoke();
            }

            UnitManager.Instance.ShowDamage(totalDamage, attackData.damageElement, isCrit, unitViewBasic.GetHUDPivotPos());
        }
        #endregion

        #region 召唤支持    
        public void OnSummon(string gearID) => this.summonnerID = gearID;
        public void ApplySummonModify<T>(T buffArgs){}
        #endregion

        #region Buff支持
        //直接修改简易数据，例如恢复护盾，恢复血量，不可修改buffProperty
        protected float ModifiValue(float modifier, float currentValue, AttributeModifyType attributeModifyType)
        {
            switch (attributeModifyType)
            {
                case AttributeModifyType.Add:
                    return currentValue + modifier;
                case AttributeModifyType.AddPercentage:
                    return currentValue * (1 + modifier);
                case AttributeModifyType.Multiply:
                    return currentValue * modifier;
                default:
                    return currentValue + modifier;
            }
        }
        //修改Buff数据
        public void ApplyAttributeModify(float modifier, AttributeModifyType modifyType, UnitModifiableAttributeType attributeType)
        {
            switch (attributeType)
            {
                //最大值可修改的属性，参考值不是自身基础值，而是动态最大值
                case UnitModifiableAttributeType.Damage:
                    dynamicArgs.currentDamage.ModifiValue(modifier, modifyType);
                    break;
                case UnitModifiableAttributeType.CritRate:
                    dynamicArgs.currentCriticRate.ModifiValue(modifier, modifyType);
                    break;
                case UnitModifiableAttributeType.CritDamage:
                    dynamicArgs.currentCriticDamageMulti.ModifiValue(modifier, modifyType);
                    break;
                case UnitModifiableAttributeType.AttackSpeed:
                    dynamicArgs.rawAttackSpeed.ModifiValue(modifier, modifyType);
                    RefreshAttackSpeed();
                    break;
                case UnitModifiableAttributeType.AttackRange:
                    dynamicArgs.currentAttackRange.ModifiValue(modifier, modifyType);
                    break;
                case UnitModifiableAttributeType.Speed:
                    dynamicArgs.rawSpeed.ModifiValue(modifier, modifyType);
                    RefreshMoveSpeed(dynamicArgs.currentMoveSpeed);
                    break;
                case UnitModifiableAttributeType.MaxHealth:
                    dynamicArgs.currentMaxHealth.ModifiValue(modifier, modifyType);
                    break;
                case UnitModifiableAttributeType.MaxShield:
                    dynamicArgs.currentMaxShield.ModifiValue(modifier, modifyType);
                    break;
                default:
                    Debug.LogError("未实现的属性修改类型：" + attributeType + "，请检查属性修改配置");
                    break;
            }
        }
        #endregion

        #region HUD支持
        public Transform GetHUDPivotTrans() => unitViewBasic.GetHUDPivotTrans(); 
        #endregion
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, dynamicArgs.currentAttackRange.cachedValue + hitBoxRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, dynamicArgs.currentAttackRange.cachedValue * UnitService.UNIT_SEARCH_RANGE_MULTIPLIER + hitBoxRadius);
        }
    }
}