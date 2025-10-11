using BattleBuff;
using UnityEngine;

namespace BattleGear
{
    public enum GearType
    {
        LaunchGear = 0, //发射武器
        SummonGear = 1, //召唤系武器
    }
    [System.Serializable]
    public struct GearDynamicArgs
    {
        //建筑物配置数据，可通过此获得原始数据
        private readonly GearData baseGearData;

        #region 动态数据
        public BuffProperty attackSpeed;
        public BuffProperty attackRange;
        public BuffProperty damage;
        public BuffProperty criticRate;
        public BuffProperty criticDamageMultiplier;
        #endregion

        public readonly string gearKey => baseGearData.m_gearKey;
        public readonly bool penetrateArmor => baseGearData.penetrateArmor;
        public readonly float damageMultiToBuilding => baseGearData.damageMultiToBuilding;
        public readonly float damageMultiToShield => baseGearData.damageMultiToShield;
        public readonly ElementType damageElement => baseGearData.damageElement;
        public readonly GearAbilityData[] gearAbilities => baseGearData.gearAbilites;
        public readonly string vfx_beginFire => baseGearData.vfx_gearBeginFire?.vfxKey;
        public readonly string sfx_beginFire => baseGearData.sfx_gearBeginFire?.name;

        public GearDynamicArgs(GearData gearData_SO, int level)
        {
            baseGearData = gearData_SO;
            float damage = GearManager.Instance.GetGearDamageByLevel(gearData_SO.m_gearKey, level);
            attackSpeed = new BuffProperty(gearData_SO.attackSpeed);
            attackRange = new BuffProperty(gearData_SO.attackRange);
            this.damage = new BuffProperty(damage); //To do，需要先引入外部加成
            criticRate = new BuffProperty(gearData_SO.criticRate, 1);
            criticDamageMultiplier = new BuffProperty(gearData_SO.criticDamageMultiplier);
        }
    }
}
