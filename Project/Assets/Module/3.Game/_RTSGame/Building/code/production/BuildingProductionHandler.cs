using UnityEngine;

namespace RTSDemo.Building
{
    //一个production handler负责一组自动生产，productionArgs会维持不变
    public class BuildingProductionHandler : MonoBehaviour
    {
        [SerializeField] private ProductionArgs productionArgs;

        private float timer = 0;
        private int roundCount = 0;
        private BuildingBase self;

        public ProductionArgs m_productionArgs => productionArgs;

        public void Init(BuildingBase buildingBase, ProductionArgs _productionArgs)
        {
            productionArgs = _productionArgs;
            self = buildingBase;

            timer = 0;
        }
        public void UpdateAutoProduction()
        {
            timer += Time.deltaTime * self.autoProduceSpeed;
            if (timer >= 1)
            {
                self.AddProductionOrder(in productionArgs);
                timer = 0;
                roundCount++;

                if (productionArgs.infiniteRound)
                {
                    roundCount = -1;
                }
                //当生产批次抵达上限后，移除该生产线
                if (!productionArgs.infiniteRound && roundCount >= productionArgs.roundLimit)
                {
                    self.EndProduction(this);
                    Destroy(this);
                }
            }
        }
    }
}
