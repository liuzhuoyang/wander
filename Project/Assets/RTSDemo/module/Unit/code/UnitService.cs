using UnityEngine;

namespace BattleActor.Unit
{
    public static class UnitService
    {
        public const string DEFAULT_SORTING = "Default";
        public const string FLYER_SORTING = "Flyer";
        public const float UNIT_SCAN_INTERSECT = 1f / UNIT_SCAN_RATE; //单位索敌时间间隔
        public const float UNIT_ATTACK_RANGE_SHRINK = 0.8f; //为避免追踪移动单位时，单位频繁切换于攻击和追踪状态，将追踪范围判定缩小一定数值。(或将攻击范围扩大一定数值)
        public const float UNIT_SEARCH_RANGE_MULTIPLIER = 5f; //单位的索敌范围默认为攻击范围的一点五倍，也即单位可以提前搜索目标，追踪目标，直到接近攻击距离
        private const float UNIT_SCAN_RATE = 1; //单位索敌频率

        public static float GetUnitAttributeByLevel(Vector2 valueRange, int level, int maxLevel)
            => valueRange.x + (valueRange.y - valueRange.x) * Mathf.Pow((level - 1f) / (maxLevel - 1f), 1.6f);
    }
}