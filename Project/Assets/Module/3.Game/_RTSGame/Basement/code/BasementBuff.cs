using BattleBuff;

namespace RTSDemo.Basement
{
    public class BasementRecoverBuff : AttributeModifierBuff
    {
        public BasementBasic basement;
        public BasementRecoverBuff(string buffID, float recoverValue, AttributeModifyType recoverType)
        {
            this.buffTypeID = buffID;
            this.attributeModifyType = recoverType;
            this.modifier = recoverValue;
        }
        public override void Initialize(BuffHandler parent)
        {
            base.Initialize(parent);
            basement = parent.GetComponent<BasementBasic>();
        }
        protected override void BuffBegin()
        {
            basement.Recover(modifier, attributeModifyType);
            ChangeBuffState(BuffState.Complete);
        }
    }
}