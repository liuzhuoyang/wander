using BattleActor;
using UnityEngine;

namespace BattleLaunch
{
    [System.Serializable]
    public struct BattleHitData
    {
        public readonly GameObject hitCaster;
        public readonly Vector2 hitPoint;
        public readonly IBattleActor hitActor;
        public readonly bool isHitByRange; //是否是被范围伤害命中，可根据此数据判定是否是主要命中造成的伤害
        
        public BattleHitData(GameObject _hitcaster, IBattleActor _hitActor, Vector2 _hitPoint, bool _isHitByRange)
        {
            hitCaster = _hitcaster;
            hitActor = _hitActor;
            hitPoint = _hitPoint;
            isHitByRange = _isHitByRange;
        }
    }
}