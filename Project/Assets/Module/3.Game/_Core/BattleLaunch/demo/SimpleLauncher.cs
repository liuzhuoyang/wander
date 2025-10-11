using System.Collections.Generic;
using BattleActor;
using UnityEngine;

namespace BattleLaunch.Demo
{
    public class SimpleLauncher : BattleBehaviour
    {
        public enum GearState
        {
            Inactive = 0,
            Cooling = 1,
            Ready = 2,
            Shooting = 3,
        }
        [SerializeField] private LaunchConfig launchConfig;
        [SerializeField] private BattleLaunchTargetFinder targetFinder;
        [SerializeField] private LaunchLayer mainLaunchLayer;
        [Header("Search Target")]
        [SerializeField] private float launchRate = 3;
        [SerializeField] private float searchRange = 5;
        [Header("Attack Param")]
        [SerializeField] private ElementType damageElement;
        [SerializeField] private bool penetrateArmor = false;
        [SerializeField] private float baseDamage = 3;
        [SerializeField] private float damageMultiToShield = 0;
        [SerializeField] private float damageMultiToBuilding = 0;
        [SerializeField] private float criticRate = 0;
        [SerializeField] private float criticDamageMulti = 0f;

        [SerializeField, ShowOnly] private GearState state;
        private BattleLaunchControl launchControl;

        private const float TARGET_UPDATE_CYCLE = 0.5f;
        private float normalizedTimer = 0;

        void Awake()
        {
            launchControl = gameObject.AddComponent<BattleLaunchControl>();
        }
        public void ResetGear()
        {
            launchControl.AbortLaunch();
            normalizedTimer = 1;
            ChangeState(GearState.Cooling);
        }
        public override void BattleUpdate()
        {
            //发射以及延时发射逻辑
            switch (state)
            {
                case GearState.Inactive:
                    return;
                case GearState.Cooling:
                    normalizedTimer -= Time.deltaTime * launchRate;
                    if (normalizedTimer <= 0)
                    {
                        ChangeState(GearState.Ready);
                    }
                    break;
                case GearState.Ready:
                    if (normalizedTimer >= TARGET_UPDATE_CYCLE)
                    {
                        //更新索敌信息
                        if (launchConfig.retargetPerLaunch)
                        {
                            targetFinder.CleanCachedTarget();
                        }
                        targetFinder.FlushTarget(searchRange, launchConfig.retargetPerCount ? launchConfig.burstCount : 1);
                        var targets = targetFinder.GetActiveTargetList();
                        //如果索敌列表有目标，则发射
                        if (!launchConfig.requireTargetToLaunch || targets.Count > 0)
                        {
                            ChangeState(GearState.Shooting);
                            //发送发射信息
                            {
                                //创建新的发射参数
                                var currentLaunchBatch = new BattleLaunchCommand_Batch(new BattleLaunchCommandData(launchConfig),
                                                                                GetAttackData(),
                                                                                mainLaunchLayer.launchTrans);
                                if (launchConfig.trackTargetIfCan)
                                    currentLaunchBatch.AssignTargets(targets);
                                //目标地点列表
                                else
                                {
                                    List<Vector2> posList = new List<Vector2>();
                                    foreach (var target in targets)
                                    {
                                        posList.Add(target.position);
                                    }
                                    currentLaunchBatch.AssignTargets(posList);
                                }
                                currentLaunchBatch.ExcludeTeam(BattleActorService.GetOppositeTeam(targetFinder.m_targetTeam));
                                //添加发射命令，并在成功时执行callback
                                launchControl.AddLaunch(currentLaunchBatch)
                                .OnLaunchEnd(OnLaunchComplete);
                            }
                        }
                        else
                        {
                            //没有目标，等待下次索敌
                            normalizedTimer = 0;
                        }
                    }
                    else
                    {
                        normalizedTimer += Time.deltaTime;
                    }
                    break;
                case GearState.Shooting:
                    launchControl.UpdateLaunching();
                    break;
                default:
                    break;
            }
        }
        //改变武器的状态
        void ChangeState(GearState nextState)
        {
            if (state == nextState)
            {
                return;
            }

            state = nextState;
            switch (nextState)
            {
                case GearState.Inactive:
                    break;
                case GearState.Cooling:
                    normalizedTimer = 1;
                    break;
                case GearState.Ready:
                    normalizedTimer = TARGET_UPDATE_CYCLE;
                    break;
            }
        }
        #region Launch Batch Action
        //BattleLaunchManager成功执行一个发射指令的第一次发射时回执处理
        void OnFirstExcute() { }
        //BattleLaunchManager成功发射后的回执处理，用于在正确点位播放发射特效，实际位置取决于弹道和连射的轮换方式
        void OnPerLaunchsucceed(int launchIndex) { }
        //发射完毕
        void OnLaunchComplete()
        {
            ChangeState(GearState.Cooling);
        }
        #endregion

        #region Attack power
        protected AttackData GetAttackData()
        {
            var attackData = new AttackData(baseDamage,
                                            baseDamage * damageMultiToBuilding,
                                            baseDamage * damageMultiToShield,
                                            penetrateArmor,
                                            criticRate,
                                            criticDamageMulti,
                                            damageElement);

            return attackData;
        }
        #endregion
    }
}