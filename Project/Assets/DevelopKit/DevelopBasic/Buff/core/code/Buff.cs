using System;
using UnityEngine;

namespace BattleBuff
{
    [System.Flags]
    public enum BuffTag
    {
        None = 0, //无Tag
        Buff = 1 << 1, //增益
        Debuff = 1 << 2, //减益
        Stun = 1 << 3, //控制
        AttributeModify = 1 << 4, //移动速度修改
        AttackModify = 1 << 5, //攻击数据修改
    }
    public enum BuffState
    {
        Detached,   //挂起
        Pending,    //载入
        Working,    //生效中
        Complete,   //结束
    }
    public abstract class Buff
    {
        protected string buffTypeID;
        protected BuffTag buffTag = BuffTag.None;
        protected BuffTag buffImmuneTag = BuffTag.None;
        protected BuffHandler parent = null; //buff作用者
        protected BuffState buffState = BuffState.Detached;//buff生命状态

        public string m_buffTypeID => buffTypeID;
        public BuffTag m_buffTag => buffTag;
        public BuffTag m_buffImmuneTag => buffImmuneTag;
        public bool IsPending => buffState == BuffState.Pending;
        public bool IsDone => buffState == BuffState.Complete;
        public virtual bool m_isAnnonumous => false; //是否为匿名Buff

        public event Action onBuffStart;
        public event Action onBuffComplete;
        public event Action onBuffRemoved;

        public virtual void Initialize(BuffHandler parent) => this.parent = parent; //当buff被添加时，首先执行初始化，此时buff还未执行
        public void SetTag(BuffTag tag) => buffTag = tag;
        public void SetImmuneTag(BuffTag tag) => buffImmuneTag = tag;
        public void ChangeBuffState(BuffState newState) //修改buff的状态，标记buff完成状态=修改buff为complete，中途终止buff=修改buff为detached
        {
            if (buffState == newState) return;
            buffState = newState;
            switch (buffState)
            {
                case BuffState.Working:
                    onBuffStart?.Invoke();
                    BuffBegin();
                    break;
                case BuffState.Complete:
                    onBuffComplete?.Invoke();
                    BuffComplete();
                    break;
                case BuffState.Detached:
                    onBuffRemoved?.Invoke();
                    BuffRemove();
                    break;
                default:
                    break;
            }
        }
        public virtual void RefreshBuff() { } //buff刷新后的行为，e.g：更新倒计时，延长buff时间
        public virtual void UpdateBuff() { } //buff生效的执行方式，e.g：执行倒计时

        protected virtual void BuffBegin() { } //buff生效后，e.g：修改属性
        protected virtual void BuffComplete() { } //buff销毁前，e.g：延迟爆炸
        protected virtual void BuffRemove() { } //buff销毁后，e.g：还原属性修改
    }

    #region 基本buff范例
    public abstract class AttackBuff : Buff
    {
        protected AttackData baseAttackData;
        protected float damageMultiplier; //Final Damage = damageMultiplier * caster.GetAttackData().damage + baseAttackData.damage;
    }
    public abstract class AttributeModifierBuff : Buff
    {
        protected float modifier;
        protected AttributeModifyType attributeModifyType;
    }
    public class AddBuffBuff : Buff
    {
        private string buffID;
        public AddBuffBuff(string buffID, string additionBuffID)
        {
            this.buffTypeID = buffID;
            this.buffID = additionBuffID;
        }
        protected override void BuffBegin()
        {
            base.BuffBegin();
            parent.TryAddBuff(buffID);
            ChangeBuffState(BuffState.Complete);
        }
    }
    public class CountBuff : Buff
    {
        protected int counter;
        protected int totalCount;
        protected Action<int> onBuffCount;

        public CountBuff(string buffID, int count, Action<int> RefreshCallback = null)
        {
            this.counter = totalCount = count;
            this.buffTypeID = buffID;
            this.onBuffCount = RefreshCallback;
        }
        protected override void BuffBegin()
        {
            counter = totalCount;
        }
        public override void RefreshBuff()
        {
            counter = totalCount;
        }
        public void Count()
        {
            Debug.Log("Count Buff:" + buffTypeID);
            counter--;
            onBuffCount?.Invoke(counter);
            if (counter <= 0)
            {
                ChangeBuffState(BuffState.Complete);
            }
        }
    }
    public class TagBuff : Buff
    {
        public TagBuff(string buffID, BuffTag buffTag, BuffTag buffImmuneTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.buffTag = buffTag;
            this.buffImmuneTag = buffImmuneTag;
        }
    }
    #endregion

    #region 匿名Buff
    public abstract class AnnonomusBuff : Buff
    {
        public override bool m_isAnnonumous => true;
        public abstract void ExcuteBuff();
    }
    //基于地点执行的Buff
    public abstract class PositionBasedBuff : AnnonomusBuff
    {
        protected Vector2 controlPosition;
        protected GameObject caster; //用于buff执行的对象
        public void ExcuteBuffOnPosition(GameObject caster, Vector2 pos)
        {
            this.caster = caster;
            controlPosition = pos;
            ExcuteBuff();
        }
    }
    //基于地点创建Prefab的简易Buff
    public class BuffSpawnObj : PositionBasedBuff
    {
        private GameObject prefab;
        private int spawnCount;
        private float spawnRadius;
        public BuffSpawnObj(string buffID, GameObject prefab, int spawnCount = 1, float spawnRadius = 0)
        {
            this.buffTypeID = buffID;
            this.prefab = prefab;
            this.spawnCount = spawnCount;
            this.spawnRadius = spawnRadius;
        }
        public override void ExcuteBuff()
        {
            if (prefab != null)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector2 spawnPos = controlPosition;
                    if (spawnRadius > 0)
                    {
                        spawnPos += UnityEngine.Random.insideUnitCircle * spawnRadius;
                    }
                    GameObject.Instantiate(prefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }
    #endregion
}