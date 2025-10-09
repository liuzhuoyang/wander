
using BattleLaunch;

namespace BattleActor.Building
{
    [System.Serializable]
    public struct BuildingDynamicArgs
    {
        //建筑物配置数据，可通过此获得原始数据
        private readonly BuildingData_SO baseBuildingData;
        public readonly int level;
        public readonly float maxHealth;
        public readonly float maxShield;
        public readonly ProductionArgs[] productionArgs;

        #region 动态数据
        public float currentHealth;
        public float currentShield;
        public float currentProduceSpeed;
        public float currentAttackSpeed;
        public float currentDamage;
        public float currentCriticRate;
        public float currentCriticDamageMultiplier;
        public float currentAttackRange;
        public BattleLaunchCommandData launchData;
        #endregion

        public float autoProduceSpeed => baseBuildingData.autoProduceSpeed;
        public float damageMultiToBuilding => baseBuildingData.damageMultiToBuilding;
        public float damageMultiToShield => baseBuildingData.damageMultiToShield;
        public bool penetrateArmor => baseBuildingData.penetrateArmor;
        public ElementType elementType => baseBuildingData.damageType;

        public BuildingDynamicArgs(BuildingData_SO buildingData, int level)
        {
            this.level = level;
            this.baseBuildingData = buildingData;
            if (buildingData.launchConfigs != null)
                launchData = new BattleLaunchCommandData(buildingData.launchConfigs);
            else
                launchData = default;

            productionArgs = new ProductionArgs[baseBuildingData.productionList.Length];
            for (int i = 0; i < productionArgs.Length; i++)
            {
                productionArgs[i] = baseBuildingData.productionList[i].GetProductionArgs();
            }

            maxHealth = currentHealth = BuildingService.GetBuildingAttributeByLevel(buildingData.healthRange, level, buildingData.maxLevel);
            maxShield = currentShield = BuildingService.GetBuildingAttributeByLevel(buildingData.shieldRange, level, buildingData.maxLevel);
            currentProduceSpeed = buildingData.globalProduceSpeed;
            currentAttackSpeed = buildingData.attackSpeed;
            currentDamage = BuildingService.GetBuildingAttributeByLevel(buildingData.attackDamage, level, buildingData.maxLevel);
            currentAttackRange = buildingData.attackRange;
            currentCriticDamageMultiplier = buildingData.criticDamageMultiplier;
            currentCriticRate = buildingData.criticRate;
        }
        public void RefreshData()
        {
            launchData = new BattleLaunchCommandData(baseBuildingData.launchConfigs);

            currentHealth = BuildingService.GetBuildingAttributeByLevel(baseBuildingData.healthRange, level, baseBuildingData.maxLevel);
            currentShield = BuildingService.GetBuildingAttributeByLevel(baseBuildingData.shieldRange, level, baseBuildingData.maxLevel);
            currentProduceSpeed = baseBuildingData.globalProduceSpeed;
            currentAttackSpeed = baseBuildingData.attackSpeed;
            currentDamage = BuildingService.GetBuildingAttributeByLevel(baseBuildingData.attackDamage, level, baseBuildingData.maxLevel);
            currentAttackRange = baseBuildingData.attackRange;
            currentCriticDamageMultiplier = baseBuildingData.criticDamageMultiplier;
            currentCriticRate = baseBuildingData.criticRate;
        }
    }
}