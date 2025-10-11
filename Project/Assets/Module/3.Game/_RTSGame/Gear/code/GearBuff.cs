using System;
using BattleBuff;
using UnityEngine;

namespace BattleGear
{
    public enum GearModifiableAttributeType
    {
        Damage = 0,
        CritRate = 1,
        CritDamage = 2,
        AttackRange = 4,
        AttackSpeed = 5,
        BurstCount = 6,
    }
    [System.Serializable]
    public struct GearAttributeModifier
    {
        public float modifier;
        public AttributeModifyType attributeModifyType;
        public GearModifiableAttributeType attributeType;
        public GearAttributeModifier(float modifier, AttributeModifyType attributeModifyType, GearModifiableAttributeType attributeType)
        {
            this.modifier = modifier;
            this.attributeModifyType = attributeModifyType;
            this.attributeType = attributeType;
        }
    }
    public class GearStatusBuff : AttributeModifierBuff
    {
        protected float duration;
        protected float timer;
        protected bool isPermanent;

        protected GearModifiableAttributeType unitModifiableAttribute;
        public GearStatusBuff(string buffID, GearAttributeModifier attributeModifier, float duration, BuffTag buffTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.buffTag = buffTag;
            this.modifier = attributeModifier.modifier;
            this.attributeModifyType = attributeModifier.attributeModifyType;
            this.unitModifiableAttribute = attributeModifier.attributeType;
            this.buffTag = buffTag;

            this.isPermanent = false;
            this.duration = duration;
        }
        public GearStatusBuff(string buffID, GearAttributeModifier attributeModifier, BuffTag buffTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.buffTag = buffTag;
            this.modifier = attributeModifier.modifier;
            this.attributeModifyType = attributeModifier.attributeModifyType;
            this.unitModifiableAttribute = attributeModifier.attributeType;
            this.buffTag = buffTag;

            this.isPermanent = true;
            this.duration = -1;
        }
        protected override void BuffBegin()
        {
            timer = 0;
            parent.GetComponent<GearBase>().ApplyAttributeModify(modifier, attributeModifyType, unitModifiableAttribute);
        }
        public override void RefreshBuff()
        {
            timer = 0;
        }
        public override void UpdateBuff()
        {
            if (isPermanent)
                return;
            timer += Time.deltaTime;
            if (timer > duration)
            {
                ChangeBuffState(BuffState.Complete);
            }
        }
        protected override void BuffRemove()
        {
            float recover = 0;
            if (attributeModifyType == AttributeModifyType.Multiply)
            {
                recover = 1 / modifier;
            }
            else
            {
                recover = -modifier;
            }
            parent.GetComponent<GearBase>().ApplyAttributeModify(recover, attributeModifyType, unitModifiableAttribute);
        }
    }
    public class GearCountBuff : CountBuff
    {
        private Gear_Launch gear;
        public GearCountBuff(string buffID, int count, Action<int> RefreshCallback = null)
            : base(buffID, count, RefreshCallback) { }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            gear = parent.GetComponent<Gear_Launch>();
        }
        protected override void BuffBegin()
        {
            gear.onGearBeginFire += Count;
            base.BuffBegin();
        }
        protected override void BuffRemove()
        {
            gear.onGearBeginFire -= Count;
            base.BuffRemove();
        }
    }
}