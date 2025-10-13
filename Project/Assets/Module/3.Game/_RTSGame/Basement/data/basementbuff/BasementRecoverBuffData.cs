using BattleBuff;
using UnityEngine;

namespace RTSDemo.Basement
{
    [CreateAssetMenu(fileName = "BasementRecoverBuffData", menuName = "RTS_Demo/Actor/Basement/Buff/BasementRecoverBuffData")]
    public class BasementRecoverBuffData : BuffData
    {
        public float recoverValue;
        public AttributeModifyType recoverType;
        protected override Buff GetBuffInstance()
        {
            return new BasementRecoverBuff(m_buffID, recoverValue, recoverType);
        }
    }
}
