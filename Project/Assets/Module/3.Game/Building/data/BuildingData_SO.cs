using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;
using BattleLaunch;
using BattleSummon;

namespace BattleActor.Building
{
    [System.Serializable]
    public struct ProductionData
    {
        public SummonData_SO summonData_SO;
        public float produceTime;
        public int produceCount;
        public bool infiniteRound;
        public int roundLimit;
        public ProductionArgs GetProductionArgs()
        {
            return new ProductionArgs()
            {
                actorData = summonData_SO.actorData,
                produceLevelAdjustment = summonData_SO.summonLevelAdjustment,
                produceLimit = summonData_SO.summonLimit,
                produceCount = produceCount,
                produceTime = produceTime,
                infiniteRound = infiniteRound,
                roundLimit = roundLimit,
            };
        }
    }
    [CreateAssetMenu(fileName = "BuildingData_SO", menuName = "RTS_Demo/Actor/Building/BuildingData_SO")]
    public class BuildingData_SO : BattleActorData_SO
    {
        public override BattleActorType actorType => BattleActorType.Building;
        
        [TabGroup("建筑素材")] public AssetReference bodyRef;

        [TabGroup("建筑生产"), PropertyTooltip("建筑自动添加生产队列的速度")] public float autoProduceSpeed = 1;
        [TabGroup("建筑生产")] public float globalProduceSpeed = 1;
        [TabGroup("建筑生产")] public ProductionData[] productionList;

        [TabGroup("战斗参数")] public LaunchConfig_SO launchConfigs;
    }
}