using UnityEngine;

namespace BattleActor.Building
{
    public class BuildingProductionManager : CommandManager<BuildingBase>
    {
        public BuildingProductionManager(BuildingBase context) : base(context){}
    }

    [System.Serializable]
    public struct ProductionArgs
    {
        public BattleActorData actorData;
        public int produceLevelAdjustment; //等级修改值，生产单位的等级 = 建筑等级+等级修改值
        public int produceCount; //每批次 生产多少单位
        public float produceTime; //生产时长

        public bool infiniteRound; //是否循环生产
        public int roundLimit; //生产批次大于该数量，结束生产，可以是infinit

        #region 动态数据
        public int produceLimit; //已生产且存活的单位数量大于该数量时，暂停生产
        #endregion
    }
    public class ProductionCommand : Command<BuildingBase>
    {
        public string productionID { get; protected set; }
        protected float timer;
        protected ProductionArgs currentOrder;
        public ProductionCommand(in ProductionArgs productionOrderArgs)
        {
            productionID = productionOrderArgs.actorData.m_actorKey;
            currentOrder = productionOrderArgs;
        }
        public int GetLimit() => currentOrder.produceLimit;
        protected override void Init()
        {
            base.Init();
            timer = 0;
        }
        internal override void CommandUpdate(BuildingBase context)
        {
            timer += Time.deltaTime * context.productionSpeed;
            if (timer >= currentOrder.produceTime)
            {
                for (int i = 0; i < currentOrder.produceCount; i++)
                {
                    context.SpawnBattleActor(currentOrder.actorData, context.currentLevel + currentOrder.produceLevelAdjustment, currentOrder.produceLimit);
                }
                SetStatus(CommandStatus.Success);
                return;
            }
        }
    }
}
