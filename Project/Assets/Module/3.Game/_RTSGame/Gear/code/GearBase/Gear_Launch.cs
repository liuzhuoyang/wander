using System;
using System.Collections.Generic;
using UnityEngine;

using BattleActor;
using BattleBuff;
using BattleLaunch;

namespace BattleGear
{
    public class Gear_Launch : GearBase
    {
        private BattleLaunchCommandData battleLaunchData;
        private BattleLaunchTargetFinder targetFinder;
        private BattleLaunchControl launchControl;

        private const float TARGET_UPDATE_CYCLE = 0.5f;

        public void InitLaunchData(LaunchConfig launchConfig)
        {
            targetFinder = GetComponent<BattleLaunchTargetFinder>();
            targetFinder.Init(TeamMask.Enemy, launchConfig.scanOrder, BattleActorMotionLayerMask.All);
            launchControl = gameObject.AddComponent<BattleLaunchControl>();
            battleLaunchData = new BattleLaunchCommandData(launchConfig);
        }

        public override void RestartGear()
        {
            launchControl.AbortLaunch();
            targetFinder.CleanCachedTarget();
            base.RestartGear();
        }

        #region Gear State
        public override void BattleLateUpdate()
        {
            //更新Gear表现
            if (state != GearState.Inactive)
                gearView.UpdateOrientation(targetFinder.GetFirstTarget());
        }
        protected override void OnReady() => normalizedTimer = TARGET_UPDATE_CYCLE;
        protected override void FiringState()
        {
            launchControl.UpdateLaunching();
        }
        protected override void ReadyState()
        {
            if (normalizedTimer >= TARGET_UPDATE_CYCLE)
            {
                //更新索敌信息
                if (battleLaunchData.retargetPerLaunch)
                {
                    targetFinder.CleanCachedTarget();
                }
                targetFinder.FlushTarget(searchRange, battleLaunchData.retargetPerCount ? (int)battleLaunchData.burstCount.cachedValue : 1);
                var targets = targetFinder.GetActiveTargetList();
                //如果索敌列表有目标，则发射
                if (!battleLaunchData.requireTargetToLaunch || targets.Count > 0)
                {
                    ChangeState(GearState.Firing);
                    //发送发射信息
                    {
                        //创建新的发射参数
                        var currentLaunchBatch = new BattleLaunchCommand_Batch(battleLaunchData,
                                                                        GetAttackData(),
                                                                        GetLaunchLayer().launchTrans);
                        //目标列表，由于敌人搜索会动态更新敌人列表，此列表必须拷贝，不能引用
                        if (battleLaunchData.trackTargetIfCan)
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
                        currentLaunchBatch.OnLaunchBegin(OnLaunchExcute)
                                          .OnLaunchEnd(OnLaunchComplete)
                                          .OnHitTarget(CallGearHitTarget);
                        launchControl.AddLaunch(currentLaunchBatch);
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
        }
        #endregion

        #region Buff支持
        public override void ApplyAttributeModify(float modifier, AttributeModifyType modifyType, GearModifiableAttributeType attributeType)
        {
            base.ApplyAttributeModify(modifier, modifyType, attributeType);
            switch (attributeType)
            {
                case GearModifiableAttributeType.BurstCount:
                    battleLaunchData.burstCount.ModifiValue(modifier, modifyType);
                    break;
            }
        }
        #endregion

        #region Launch Batch Action
        void OnLaunchExcute()
        {
            gearView.OnGearBeginFire();
            CallGearFire();
        }
        //发射完毕
        void OnLaunchComplete()
        {
            ChangeState(GearState.Cooling);
        }
        #endregion
    }
}