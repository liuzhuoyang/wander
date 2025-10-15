using BattleBuff;

namespace RTSDemo.Basement
{
    [System.Serializable]
    public struct BasementDynamicArgs
    {
        private readonly BasementData baseBasementData;

        #region 可增益数据
        public BuffProperty maxHealth;
        public BuffProperty maxShield;
        #endregion

        #region 动态数据
        public float currentHealth;
        public float currentShield;
        #endregion

        public BasementDynamicArgs(BasementData basementData_SO)
        {
            baseBasementData = basementData_SO;
            maxHealth = new BuffProperty(basementData_SO.maxHealth);
            maxShield = new BuffProperty(basementData_SO.maxShield);
            currentHealth = maxHealth.cachedValue;
            currentShield = maxShield.cachedValue;
        }
    }
}