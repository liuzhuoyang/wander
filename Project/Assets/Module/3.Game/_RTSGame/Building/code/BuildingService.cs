using UnityEngine;

namespace RTSDemo.Building
{
    using BattleActor;
    public static class BuildingService
    {
        public static float GetBuildingAttributeByLevel(Vector2 valueRange, int level, int maxLevel)
            => valueRange.x + (valueRange.y - valueRange.x) * Mathf.Pow((level - 1f) / (maxLevel - 1f), 1.6f);
    }
}
