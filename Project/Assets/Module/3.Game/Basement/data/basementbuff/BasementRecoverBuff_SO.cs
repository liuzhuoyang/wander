using BattleBuff;
using UnityEngine;

namespace BattleActor.Basement
{
    [CreateAssetMenu(fileName = "BasementRecoverBuff_SO", menuName = "RTS_Demo/Actor/Basement/Buff/BasementRecoverBuff_SO")]
    public class BasementRecoverBuff_SO : BuffData_SO
    {
        public float recoverValue;
        public AttributeModifyType recoverType;
        protected override Buff GetBuffInstance()
        {
            return new BasementRecoverBuff(m_buffID, recoverValue, recoverType);
        }
    }
}
