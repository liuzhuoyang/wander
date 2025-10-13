using UnityEngine;
using System;
using BattleLaunch.Bullet;
using BattleBuff;

namespace RTSDemo.Unit
{
    using BattleActor;
    //单位种族
    public enum UnitRace
    {
        None = 0, //无种族特性
        Zreep = 1, //异虫
        Iron = 2, //铁卫
    }

    //实例化的单位原始数据，一般在单位创建前，应用全局加成时做修改
    public struct UnitObjectArgs
    {
        private readonly UnitData unitData; //单位静态数据，禁止修改
        private readonly int unitLevel; //单位等级，此等级并非小兵的合成等级，需要区分
        private float baseMaxHealth;
        private float baseMaxShield;
        private float baseMaxArmor;
        private float baseDamage;
        private float baseAttackSpeed;
        private float baseCriticRate;
        private float baseCriticDamage;

        public BattleActorMotionLayerMask AttackLayer => unitData.attackLayer;
        public string UnitName => unitData.m_actorKey;
        public bool HasBullet => unitData.m_bulletData!=null;
        public bool IsRange => unitData.isRange;
        public bool PenetrateArmor => unitData.penetrateArmor;
        //数值计算：属性值 = 基础值 + (目标值 - 基础值) * ((等级-1)/(最大等级-1))²
        public int Level => unitLevel;
        public float MaxHealth => baseMaxHealth;
        public float MaxArmor => baseMaxArmor;
        public float MaxShield => baseMaxShield;
        public float Damage => baseDamage;
        public float DamageMultiToStructure => unitData.damageMultiToBuilding; //对基地增伤率
        public float DamageMultiToShield => unitData.damageMultiToShield; //对护盾增伤率
        public float CriticRate => baseCriticRate;
        public float CriticDamageMulti => baseCriticDamage;
        public float AttackSpeed => baseAttackSpeed;
        public float MoveSpeed => unitData.moveSpeed;
        public float AttackRange => IsRange ? unitData.attackRange : 0.5f; //近战攻击先统一用最小攻击范围，远程攻击用远程范围
        public float ReloadSpeed => unitData.reloadSpeed; //获取当前装填速度
        public int MaxAmmo => unitData.maxAmmo; //获取基础的最大携弹量
        public bool HasAmmoLimit => unitData.maxAmmo > 0 && unitData.isRange;

        public UnitRace Race => unitData.unitRace;
        public ElementType DamageElementType => unitData.damageType;
        public BattleActorMotionLayer MotionLayer => unitData.motionLayer;
        public BulletData bulletData => unitData.m_bulletData;
        public BuffData[] abilities => unitData.m_unitAbilites;
        
        public UnitObjectArgs(UnitData unitData, int level)
        {
            this.unitData = unitData;
            unitLevel = Mathf.Max(1, level);

            //以下数值会需要全局加成，因此需要记录，而不能只通过基础数据获取
            int maxLevel = unitData.maxLevel;

            baseMaxHealth = UnitService.GetUnitAttributeByLevel(this.unitData.healthRange, unitLevel, maxLevel);
            baseMaxArmor = UnitService.GetUnitAttributeByLevel(this.unitData.armorRange, unitLevel, maxLevel);
            baseMaxShield = UnitService.GetUnitAttributeByLevel(this.unitData.shieldRange, unitLevel, maxLevel);
            baseDamage = UnitService.GetUnitAttributeByLevel(this.unitData.attackDamage, unitLevel, maxLevel);

            baseAttackSpeed = this.unitData.attackSpeed;
            baseCriticRate = this.unitData.criticRate;
            baseCriticDamage = this.unitData.criticDamageMultiplier;
        }
    }

    //单位动态数据，在战斗中动态修改
    [Serializable]
    public struct UnitDynamicArgs
    {
        //以下数值会有降低到0以下的可能，在处理逻辑时会以0来计算，但处理加成和恢复加成时会以真实值计算
        //例如当被施加移动速度降低100%，与移动速度降低50%后，移除状态时恢复的程度不能比原速度大，因此需要一个真实值进行记录
        public BuffProperty rawSpeed;
        public BuffProperty rawAttackSpeed;
        
        public BuffProperty currentMaxHealth;
        public BuffProperty currentMaxShield;

        public BuffProperty currentDamage;
        public BuffProperty currentCriticRate;
        public BuffProperty currentCriticDamageMulti;
        public BuffProperty currentAttackRange;

        public float currentHealth;
        public float currentShield;

        public float currentMaxArmor;
        public float currentArmor;
        public float currentReloadSpeed;
        public int currentMaxAmmo;
        public int currentAmmo;

        public int generation; //世代计数，每次分裂单位时，新生成的单位splitCounter=this.splitCounter+1，当分裂计数>=分裂技能上限时不能触发分裂

        #region 只读数据
        public readonly string[] abilityIDs;
        public float healthRatio { get { return (currentMaxHealth.cachedValue == 0) ? 0 : currentHealth / currentMaxHealth.cachedValue; } }
        public float armorRatio { get { return (currentMaxArmor == 0) ? 0 : currentArmor / currentMaxArmor; } }
        public float shieldRatio { get { return (currentMaxShield.cachedValue == 0) ? 0 : currentShield / currentMaxShield.cachedValue; } }
        public float currentMoveSpeed => rawSpeed.cachedValue;
        public float currentAttackSpeed => rawAttackSpeed.cachedValue;
        #endregion

        public UnitDynamicArgs(UnitObjectArgs args)
        {
            rawAttackSpeed = new BuffProperty(args.AttackSpeed, args.AttackSpeed * 4);
            rawSpeed = new BuffProperty(args.MoveSpeed, args.MoveSpeed * 4);
            currentMaxHealth = new BuffProperty(args.MaxHealth);
            currentMaxShield = new BuffProperty(args.MaxShield);
            currentDamage = new BuffProperty(args.Damage);

            currentAttackRange = new BuffProperty(args.AttackRange);
            currentCriticDamageMulti = new BuffProperty(args.CriticDamageMulti);
            currentCriticRate = new BuffProperty(args.CriticRate);

            currentReloadSpeed = args.ReloadSpeed;

            currentMaxAmmo = currentAmmo = args.HasAmmoLimit ? args.MaxAmmo : 1;
            currentHealth = currentMaxHealth.cachedValue;
            currentShield = currentMaxShield.cachedValue;
            currentMaxArmor = currentArmor = args.MaxArmor;

            //记录单位技能
            if (args.abilities != null && args.abilities.Length > 0)
            {
                abilityIDs = new string[args.abilities.Length];
                for (int i = 0; i < args.abilities.Length; i++)
                {
                    abilityIDs[i] = args.abilities[i].m_buffID;
                }
            }
            else
            {
                abilityIDs = null;
            }

            //单位世代数，用于计算分裂次数
                generation = 0;
        }
    }
}