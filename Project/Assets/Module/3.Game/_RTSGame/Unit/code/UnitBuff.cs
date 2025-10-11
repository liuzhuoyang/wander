using BattleBuff;
using SimpleVFXSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BattleActor.Unit
{
    public enum UnitModifiableAttributeType
    {
        Damage = 0,
        CritRate = 1,
        CritDamage = 2,
        Speed = 3,
        AttackRange = 4,
        AttackSpeed = 5,
        MaxHealth = 6,
        MaxShield = 7,
    }
    [System.Serializable]
    public struct UnitAttributeModifier
    {
        public float modifier;
        public AttributeModifyType attributeModifyType;
        public UnitModifiableAttributeType attributeType;
        public UnitAttributeModifier(float modifier, AttributeModifyType attributeModifyType, UnitModifiableAttributeType attributeType)
        {
            this.modifier = modifier;
            this.attributeModifyType = attributeModifyType;
            this.attributeType = attributeType;
        }
    }

    //对单位参数永久性修改，并立刻销毁
    public class UnitAttributeTriggerBuff : AttributeModifierBuff
    {
        protected UnitModifiableAttributeType unitModifiableAttribute;
        public UnitAttributeTriggerBuff(string buffID, UnitAttributeModifier attributeModifier, BuffTag buffTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.buffTag = buffTag;
            this.modifier = attributeModifier.modifier;
            this.attributeModifyType = attributeModifier.attributeModifyType;
            this.unitModifiableAttribute = attributeModifier.attributeType;
            this.buffTag = buffTag;
        }
        protected override void BuffBegin()
        {
            parent.GetComponent<UnitBase>().ApplyAttributeModify(modifier, attributeModifyType, unitModifiableAttribute);
            ChangeBuffState(BuffState.Complete);
        }
    }
    //在限定时间内修改单位参数
    public class UnitStatusBuff : AttributeModifierBuff
    {
        private UnitBase unit;
        protected UnitModifiableAttributeType attributeType;
        protected string vfxKey;
        protected float duration;
        protected float timer;
        protected bool isPermanent;
        
        public UnitStatusBuff(string buffID, UnitAttributeModifier modifier, BuffTag buffTag = BuffTag.None, BuffTag buffImmuneTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.modifier = modifier.modifier;
            this.attributeType = modifier.attributeType;
            this.attributeModifyType = modifier.attributeModifyType;
            this.buffTag = buffTag;
            this.buffImmuneTag = buffImmuneTag;

            this.isPermanent = true;
            this.duration = -1;
            this.timer = 0;
        }
        public UnitStatusBuff(string buffID, UnitAttributeModifier modifier, float duration, BuffTag buffTag = BuffTag.None, BuffTag buffImmuneTag = BuffTag.None)
        {
            this.buffTypeID = buffID;
            this.modifier = modifier.modifier;
            this.attributeType = modifier.attributeType;
            this.attributeModifyType = modifier.attributeModifyType;
            this.buffTag = buffTag;
            this.buffImmuneTag = buffImmuneTag;

            this.isPermanent = false;
            this.duration = duration;
            this.timer = 0;
        }
        public void SetVFXData(string vfxKey)
        {
            this.vfxKey = vfxKey;
        }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            unit = parent.GetComponent<UnitBase>();
        }
        public override void UpdateBuff()
        {
            //永久性Buff不计时，除非手动移除
            if (isPermanent)
                return;
            timer += Time.deltaTime;
            if (timer > duration)
            {
                ChangeBuffState(BuffState.Complete);
            }
        }
        protected override void BuffBegin()
        {
            timer = 0;
            unit.ApplyAttributeModify(modifier, attributeModifyType, attributeType);
            VFXManager.Instance.PlayVFX(vfxKey, unit.position, 0, 1, null, new GameObject[] { unit.gameObject })
                               .GetComponent<VFXBuff<UnitStatusBuff>>()
                               .SetControlBuff(this);
        }
        protected override void BuffRemove()
        {
            float recover;
            if (attributeModifyType == AttributeModifyType.Multiply)
                recover = 1 / modifier;
            else
                recover = -modifier;

            unit.ApplyAttributeModify(recover, attributeModifyType, attributeType);
        }
    }
}