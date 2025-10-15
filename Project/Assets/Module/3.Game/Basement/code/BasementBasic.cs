using BattleBuff;
using UnityEngine;

using Random = UnityEngine.Random;

namespace RTSDemo.Basement
{
    using BattleActor;
    using Skill;
    public class BasementBasic : MonoBehaviour, IBattleActor
    {
        [SerializeField] protected BasementDynamicArgs dynamicArgs;
        protected Collider2D hitBox;
        protected BuffHandler buffHandler;

        protected bool isDead = false;
        protected TeamMask teamMask;
        protected BasementSkill[] abilities;

        public float currentHealth => dynamicArgs.currentHealth;
        public float currentHealthRatio => dynamicArgs.currentHealth / currentMaxHealth;
        public float currentMaxHealth => dynamicArgs.maxHealth.cachedValue;
        public float currentShield => dynamicArgs.currentShield;
        public float currentShieldRatio => dynamicArgs.currentShield / currentMaxShield;
        public float currentMaxShield => dynamicArgs.maxShield.cachedValue;
        public float currentMaxMana => dynamicArgs.maxMana.cachedValue;
        //Others
        public bool IsDead => isDead;
        public bool IsTargetable => !isDead;
        public TeamMask teamType => teamMask;
        public Vector2 position => transform.position;
        public BattleActorType battleActorType => BattleActorType.Basement;
        public BattleActorMotionLayer motionLayer => BattleActorMotionLayer.Ground;
        public BuffHandler m_buffHandler => buffHandler;

        public const float COLLISION_EXTRUDE = 0.5f;

        #region 基地生命周期
        public void Init(BasementData basementData, Vector2 basementSize)
        {
            dynamicArgs = new BasementDynamicArgs(basementData);
            teamMask = TeamMask.Player;

            //添加初始组件
            buffHandler = gameObject.AddComponent<BuffHandler>();
            hitBox = GetComponent<Collider2D>();

            //设置碰撞体
            if (hitBox == null)
            {
                hitBox = gameObject.AddComponent<BoxCollider2D>();
            }
            (hitBox as BoxCollider2D).size = basementSize + Vector2.one * COLLISION_EXTRUDE;

            //初始化技能
            if (basementData.basementAbilities != null && basementData.basementAbilities.Length > 0)
            {
                abilities = new BasementSkill[basementData.basementAbilities.Length];
                for (int i = 0; i < abilities.Length; i++)
                {
                    var abilityData = basementData.basementAbilities[i];
                    buffHandler.TryAddBuff(abilityData.m_buffID);
                    abilities[i] = buffHandler.GetBuff(abilityData.m_buffID) as BasementSkill;
                }
            }
        }

        public void Revive()
        {
            dynamicArgs.currentHealth = currentMaxHealth;
            dynamicArgs.currentShield = currentMaxShield;
            dynamicArgs.currentMana = currentMaxMana;
            isDead = false;
        }
        #endregion

        #region Mana支持
        public bool IsManaEnough(float manaCost)
        {
            return dynamicArgs.currentMana >= manaCost;
        }
        public void UseMana(float manaCost)
        {
            dynamicArgs.currentMana -= manaCost;
        }
        public void GainMana(AttributeModifyType modifyType, float value)
        {
            switch (modifyType)
            {
                case AttributeModifyType.Add:
                    dynamicArgs.currentMana += value;
                    break;
                case AttributeModifyType.AddPercentage:
                    dynamicArgs.currentMana += currentMaxMana * value;
                    break;
                case AttributeModifyType.Multiply:
                    dynamicArgs.currentMana *= value;
                    break;
            }

            dynamicArgs.currentMana = Mathf.Min(dynamicArgs.currentMana, dynamicArgs.maxMana.cachedValue);
        }
        #endregion

        #region 战斗支持
        public void Recover(float health, AttributeModifyType attributeModifyType)
        {
            switch (attributeModifyType)
            {
                case AttributeModifyType.Add:
                    dynamicArgs.currentHealth += health;
                    break;
                case AttributeModifyType.AddPercentage:
                    dynamicArgs.currentHealth += currentMaxHealth * health;
                    break;
                case AttributeModifyType.Multiply:
                    dynamicArgs.currentHealth *= health;
                    break;
            }
            dynamicArgs.currentHealth = Mathf.Min(dynamicArgs.currentHealth, dynamicArgs.maxHealth.cachedValue);
        }
        public void TakeDamage(AttackData attackData, Vector2 hitPos)
        {
            if (isDead) return;

            float attackDamage = attackData.damage;
            bool isCrit = attackData.criticRate > Random.value;
            if (isCrit)
            {
                attackDamage *= AttackData.BASIC_CRITIC_MULTI + attackData.criticDamageMulti;
            }
            float shield = dynamicArgs.currentShield;

            //护盾增伤不超过护盾值
            attackDamage += Mathf.Min(shield, attackData.damageToShield);

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

            //死亡检测
            if (dynamicArgs.currentHealth <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                }
            }
        }
        #endregion

        #region 索敌支持
        public bool IsPosInRange(Vector2 pos, float range) => GeometryUtil.IsPointInCircle(GetClosestPointTo(pos), position, range);
        public float GetSqDistanceTo(Vector2 pos) => (pos - GetClosestPointTo(pos)).sqrMagnitude;
        public float GetDistanceTo(Vector2 pos) => (pos - GetClosestPointTo(pos)).magnitude;
        public float GetClosestSqDistanceTo(Vector2 pos) => (pos - GetClosestPointTo(pos)).sqrMagnitude;
        public Vector2 GetClosestPointTo(Vector2 pos)
        {
            Vector2 result = hitBox.ClosestPoint(pos);
            return result;
        }
        public Vector2 GetHitPos(Vector2 attackObjPos) => GetClosestPointTo(attackObjPos);
        #endregion

        #region 基地技能支持
        public void TriggerAbility(int abilityIndex)
        {
            if (abilityIndex < abilities.Length)
            {
                var ability = abilities[abilityIndex];
                if (ability.requireTarget)
                {
                    BasementControl.Instance.OnChooseAbilityTargetPosition(ability.ExcuteBasementAbilityOnNearPos);
                }
                else
                {
                    ability.ExcuteBasementAbility();
                }
            }
        }
        #endregion
    }
}
