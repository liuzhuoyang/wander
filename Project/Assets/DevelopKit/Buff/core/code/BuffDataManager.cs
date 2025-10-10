using UnityEngine;

namespace BattleBuff
{
    public class BuffDataManager : Singleton<BuffDataManager>
    {
        [SerializeField] private BuffDataCollection_SO buffDataCollection_SO;
        public Buff GetBuff(string buffID)
        {
            var buffData = buffDataCollection_SO.GetDataByKey(buffID);
            if (buffData == null)
                return null;
            else
                return buffData.GetBuff();
        }
        public bool IsPositionBuff(string buffID)
        {
            var buffData = buffDataCollection_SO.GetDataByKey(buffID);
            return buffData.m_positionbasedBuff;
        }
    }
}