using System;
using UnityEngine;

using BattleLaunch;
using BattleBuff;
using BattleSummon;

namespace BattleGear
{
    /// <summary>
    /// 能够执行战斗，但不具有血量，不能够被索敌或消灭的实体
    /// </summary>
    public abstract class GearBase : BattleBehaviour
    {
        public enum GearState
        {
            Inactive = 0,
            Cooling = 1,
            Ready = 2,
            Firing = 3,
        }

        [SerializeField] protected GearDynamicArgs gearDynamicArgs;
        [SerializeField, ShowOnly] protected GearState state;
        protected int currentLevel;

        protected BuffHandler buffHandler;
        protected GearViewBasic gearView;

        protected float attackSpeed => gearDynamicArgs.attackSpeed.cachedValue;
        protected float searchRange => gearDynamicArgs.attackRange.cachedValue;
        
        protected ElementType damageElement => gearDynamicArgs.damageElement;
        protected bool penetrateArmor => gearDynamicArgs.penetrateArmor;
        protected float damage => gearDynamicArgs.damage.cachedValue;
        protected float criticRate => gearDynamicArgs.criticRate.cachedValue;
        protected float criticDamageMulti => gearDynamicArgs.criticDamageMultiplier.cachedValue;
        protected float damageMultiToShield => gearDynamicArgs.damageMultiToShield;
        protected float damageMultiToBuilding => gearDynamicArgs.damageMultiToBuilding;

        protected float normalizedTimer = 0;

        public string m_gearKey => gearDynamicArgs.gearKey;
        public BuffHandler m_buffHandler => buffHandler;

        #region 武器事件
        public event Action onGearRestart; //当武器启动时触发
        public event Action onGearBeginFire; //当武器发射时触发
        public event Action<ISummonnee> onGearSummon; //当武器召唤时触发
        public event Action<BattleHitData> onGearHitTarget; //当武器击中目标时触发
        #endregion

        #region 武器表现
        public string vfx_beginFire => gearDynamicArgs.vfx_beginFire;
        public string sfx_beginFire => gearDynamicArgs.sfx_beginFire;
        #endregion

        #region Gears生命周期
        public void Init(GearDynamicArgs gearArgs)
        {
            gearView = GetComponent<GearViewBasic>();
            gearView.Init(this);

            buffHandler = gameObject.AddComponent<BuffHandler>();

            gearDynamicArgs = gearArgs;

            if (gearArgs.gearAbilities != null && gearArgs.gearAbilities.Length > 0)
            {
                foreach (var ability in gearArgs.gearAbilities)
                {
                    buffHandler.TryAddBuff(ability.m_buffID);
                }
            }

            state = GearState.Inactive;
        }
        public void CleanUp()
        {
            Destroy(gameObject);
        }
        public override void BattleUpdate()
        {
            //发射以及延时发射逻辑
            switch (state)
            {
                case GearState.Inactive:
                    return;
                case GearState.Cooling:
                    normalizedTimer -= Time.deltaTime * attackSpeed;
                    if (normalizedTimer <= 0)
                    {
                        ChangeState(GearState.Ready);
                    }
                    break;
                case GearState.Ready:
                    ReadyState();
                    break;
                case GearState.Firing:
                    FiringState();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 武器状态
        //改变武器的状态
        protected void ChangeState(GearState nextState)
        {
            if (state == nextState)
                return;

            state = nextState;
            switch (nextState)
            {
                case GearState.Inactive:
                    break;
                case GearState.Cooling:
                    normalizedTimer = 1;
                    break;
                case GearState.Ready:
                    OnReady();
                    break;
            }
        }
        protected virtual void OnReady() { }
        protected abstract void ReadyState();
        protected abstract void FiringState();
        public virtual void RestartGear()
        {
            normalizedTimer = 1;
            ChangeState(GearState.Cooling);
            onGearRestart?.Invoke();
        }
        public void StopGear()
        {
            ChangeState(GearState.Inactive);
            gearView.ResetView();
        }
        #endregion

        #region 点位获取
        protected LaunchLayer GetLaunchLayer() => gearView.GetLaunchLayer();
        #endregion

        #region Buff支持
        public virtual void ApplyAttributeModify(float modifier, AttributeModifyType modifyType, GearModifiableAttributeType attributeType)
        {
            switch (attributeType)
            {
                case GearModifiableAttributeType.Damage:
                    gearDynamicArgs.damage.ModifiValue(modifier, modifyType);
                    break;
                case GearModifiableAttributeType.CritRate:
                    gearDynamicArgs.criticRate.ModifiValue(modifier, modifyType);
                    break;
                case GearModifiableAttributeType.CritDamage:
                    gearDynamicArgs.criticDamageMultiplier.ModifiValue(modifier, modifyType);
                    break;
                case GearModifiableAttributeType.AttackRange:
                    gearDynamicArgs.attackRange.ModifiValue(modifier, modifyType);
                    break;
                case GearModifiableAttributeType.AttackSpeed:
                    gearDynamicArgs.attackSpeed.ModifiValue(modifier, modifyType);
                    break;
            }
        }
        #endregion

        #region 攻击
        protected AttackData GetAttackData()
        {
            return new AttackData(damage,
                                  damage * damageMultiToBuilding,
                                  damage * damageMultiToShield,
                                  penetrateArmor,
                                  criticRate,
                                  criticDamageMulti,
                                  damageElement);
        }
        #endregion

        #region 武器事件调用
        protected void CallGearSummon(ISummonnee summonnee) => onGearSummon?.Invoke(summonnee);
        protected void CallGearHitTarget(BattleHitData battleHitData) => onGearHitTarget?.Invoke(battleHitData);
        protected void CallGearFire() => onGearBeginFire?.Invoke();
        #endregion
    }
}