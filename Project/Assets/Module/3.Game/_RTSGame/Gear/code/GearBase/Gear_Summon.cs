using BattleSummon;
using UnityEngine;

namespace BattleGear
{
    public class Gear_Summon : GearBase
    {
        [SerializeField] private BattleSummonArg battleSummonArg;
        public void InitSummonArg(SummonData summonData)
        {
            battleSummonArg = new BattleSummonArg(summonData.actorData,
                                                  summonData.summonLimit,
                                                  Vector2.zero,
                                                  gameObject.name,
                                                  false,
                                                  currentLevel + summonData.summonLevelAdjustment);
        }
        protected override void ReadyState()
        {
            Vector2 summonPos = GeometryUtil.RandomPointInCircle(GetLaunchLayer().launchTrans[0].position,
                                                                gearDynamicArgs.attackRange.cachedValue - 0.1f,
                                                                gearDynamicArgs.attackRange.cachedValue + 0.1f);
            battleSummonArg.summonPosition = summonPos;
            var summonee = GearManager.Instance.TrySummon(battleSummonArg);

            if(summonee != null)
            {
                gearView.OnGearBeginFire();
                CallGearFire();
                CallGearSummon(summonee);
            }


            ChangeState(GearState.Cooling);
        }
        protected override void FiringState(){}
    }
}
