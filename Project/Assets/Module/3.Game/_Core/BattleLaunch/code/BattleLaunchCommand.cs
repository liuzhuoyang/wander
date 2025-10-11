using System;
using System.Collections.Generic;
using UnityEngine;
using BattleActor;
using BattleBuff;

namespace BattleLaunch
{
    //发射物类型
    public enum LaunchableType
    {
        Projectile = 0, //发射后，实体朝方向直线运动
        Missile = 1, //发射后，实体逐渐瞄准目标方向
        Parabola = 2, //发射后，实体以抛物线投向目标
        Direct = 3, //发射后，无实体，直接产生作用
        Laser = 4, //发射后，程一条直线
        Sector = 5, //发射后，呈扇形范围作用
        CircularSpread = 6 //发射后，呈圆形扩散作用，无需目标
    }
    //发射参数，用于转译为发射指令
    //可以在发射器初始化阶段创建后储存，避免重复创建数据
    public class BattleLaunchCommandData
    {
        public readonly LaunchableData launchableData;
        public readonly bool trackTargetIfCan;
        public readonly bool retargetPerCount;
        public readonly bool retargetPerLaunch;
        public readonly float spreadAngle;
        public readonly float burstInterval;

        #region 动态数据
        public BuffProperty burstCount;
        public int spreadCount { get; private set; }
        #endregion

        public bool requireTargetToLaunch => launchableData.launchableType != LaunchableType.CircularSpread;

        public BattleLaunchCommandData(LaunchConfig launchConfig)
        {
            burstCount = new BuffProperty(launchConfig.burstCount,-1,true);
            burstInterval = launchConfig.burstInterval;
            spreadCount = launchConfig.spreadCount;
            spreadAngle = launchConfig.spreadAngle;
            launchableData = launchConfig.launchableData_SO;
            retargetPerLaunch = launchConfig.retargetPerLaunch;
            retargetPerCount = launchConfig.retargetPerCount;
            trackTargetIfCan = launchConfig.trackTargetIfCan;
        }
    }

    //发射指令
    public abstract class BattleLaunchCommand { }
    //直接发射指令，发送后直接处理，并抛弃
    public class BattleLaunchCommand_Direct : BattleLaunchCommand
    {
        private readonly LaunchableData launchable_SO;
        private readonly AttackData attackData;
        private readonly TeamMask excludeTeam;
        private IBattleActor targetActor;
        private Transform launchTrans;

        private Action<BattleHitData> onPerHitTarget; //命中目标回执

        public LaunchableType m_launchType => launchable_SO.launchableType;
        public string m_launchableKey => launchable_SO.m_launchableKey;
        public TeamMask m_excludeTeam => excludeTeam;
        public AttackData m_attackData => attackData;
        public IBattleActor m_targetActor => targetActor;
        public Transform m_launchTrans => launchTrans;
        public Vector2 m_launchPos => launchTrans.position;
        public Vector2 m_launchDir => launchTrans.up;

        public BattleLaunchCommand_Direct(LaunchableData launchable_SO, AttackData attackData, Transform launchTrans, IBattleActor target)
        {
            this.launchable_SO = launchable_SO;
            this.attackData = attackData;
            this.targetActor = target;
            this.excludeTeam = BattleActorService.GetOppositeTeam(targetActor.teamType);
            this.launchTrans = launchTrans;
        }
        public void OnHitTarget(Action<BattleHitData> OnHit)
        {
            onPerHitTarget = OnHit;
        }
        public void HitTarget(BattleHitData hitData)
        {
            onPerHitTarget?.Invoke(hitData);
        }
    }
    //完整发射指令，包含连射等信息
    public class BattleLaunchCommand_Batch : BattleLaunchCommand
    {
        private readonly BattleLaunchCommandData dynamicLaunchData; //发射数据
        private readonly AttackData attackData;
        private TeamMask excludeTeam = TeamMask.None;
        private float launchTimer;
        private int burstCountLeft;

        //当前目标单位列表
        private List<IBattleActor> targetActorList;
        //退化的目标位置列表
        private List<Vector2> targetPosList; //手动，或目标获取失败时采用

        private bool isExcuted = false; //是否执行过，用于判断是否是初次执行
        private Transform[] launchTranses; //所有的发射点位
        private Vector2 backupLaunchPos; //退化的发射位置，发射位置获取失败时采用
        private Vector2 backupLaunchDir; //退化的发射方向，发射方向获取失败时采用

        private Action<int> onBurstComplete; //发射成功的回执，可接受int，作为当前发射指令下的发射计数索引，vector2作为发射指令获取的目标位置（可选）
        private Action onLaunchExcute; //初次成功发射的回执，例如连射武器的第一发子弹
        private Action onLaunchComplete;//该次数所有子弹发射完毕的回执
        private Action<Vector2> onPerHit;//当命中地点时执行回执
        private Action<BattleHitData> onPerHitTarget; //当命中目标时执行回执

        #region 索敌数据
        public List<IBattleActor> m_targetActorList => targetActorList;
        public List<Vector2> m_targetPosList => targetPosList;
        public TeamMask m_excludeTeam => excludeTeam;
        #endregion

        #region 发射数据
        public int m_spreadCount => dynamicLaunchData.spreadCount;
        public float m_spreadAngle => dynamicLaunchData.spreadAngle;
        public int m_countLeft => burstCountLeft;
        #endregion

        #region 子弹数据
        public LaunchableType m_launchType => dynamicLaunchData.launchableData.launchableType;
        public float m_aimRadius => dynamicLaunchData.launchableData.aimRadius;
        public string m_launchableKey => dynamicLaunchData.launchableData.m_launchableKey;
        #endregion

        #region 构造函数
        //多发射点发射指令
        public BattleLaunchCommand_Batch(in BattleLaunchCommandData dynamicLaunchData, AttackData attackData, Transform[] launchTranses)
        {
            this.dynamicLaunchData = dynamicLaunchData;
            this.launchTranses = launchTranses;
            this.launchTimer = 0;
            this.burstCountLeft = (int)dynamicLaunchData.burstCount.cachedValue;
            this.attackData = attackData;
        }
        //退化的无发射点位的发射指令，只用于基于地点的relaunch
        public BattleLaunchCommand_Batch(in BattleLaunchCommandData dynamicLaunchData, AttackData attackData, Vector2 launchPos, Vector2 launchDir)
        {
            this.dynamicLaunchData = dynamicLaunchData;
            this.attackData = attackData;
            this.launchTimer = 0;
            this.burstCountLeft = (int)dynamicLaunchData.burstCount.cachedValue;
            backupLaunchPos = launchPos;
            backupLaunchDir = launchDir;
        }
        //单独发射点发射指令，用于建筑、主动技能、基于单位的relaunch、moving launcher
        public BattleLaunchCommand_Batch(in BattleLaunchCommandData dynamicLaunchData, AttackData attackData, Transform launchTrans)
        {
            this.attackData = attackData;
            this.dynamicLaunchData = dynamicLaunchData;
            this.launchTimer = 0;
            this.burstCountLeft = (int)dynamicLaunchData.burstCount.cachedValue;
            this.launchTranses = new Transform[] { launchTrans };
        }
        #endregion
        public void AssignTargets(List<IBattleActor> targets)
        {
            targetActorList = targets;
            targetPosList = new List<Vector2>();
            for (int i = 0; i < targets.Count; i++)
            {
                targetPosList.Add(targets[i].position);
            }
        }
        public void AssignTargets(List<Vector2> targetPoses)
        {
            targetPosList = targetPoses;
        }
        public void ExcludeTeam(TeamMask teamType)
        {
            excludeTeam = teamType;
        }
        public void UpdateLaunch(Action<BattleLaunchCommand_Batch> launchCallback)
        {
            launchTimer -= Time.deltaTime;
            if (launchTimer <= 0)
            {
                burstCountLeft--;
                launchTimer = dynamicLaunchData.burstInterval;
                launchCallback?.Invoke(this);
            }
        }

        #region 事件注册
        //赋予发射指令完成回执，会在发射指令中的每一次发射调用
        public BattleLaunchCommand_Batch OnPerLaunchComplete(Action<int> launchAction)
        {
            this.onBurstComplete = launchAction;
            return this;
        }
        //赋予发射指令执行回执，只在发射指令中的第一次发射时调用
        public BattleLaunchCommand_Batch OnLaunchBegin(Action excuteAction)
        {
            this.onLaunchExcute = excuteAction;
            return this;
        }
        //全部发射执行完毕后调用
        public BattleLaunchCommand_Batch OnLaunchEnd(Action action)
        {
            this.onLaunchComplete = action;
            return this;
        }
        public BattleLaunchCommand_Batch OnHitTarget(Action<BattleHitData> action)
        {
            this.onPerHitTarget = action;
            return this;
        }
        //发射结束后执行
        public void AllLaunchComplete()
        {
            onLaunchComplete?.Invoke();
        }
        //发射成功时，调用该函数以告诉发射指令执行发射成功回执
        public void PerLaunchSuccess(int launchIndex)
        {
            //初次执行时，执行callback
            if (!isExcuted)
            {
                isExcuted = true;
                onLaunchExcute?.Invoke();
            }
            //每次成功时，执行完成发射命令
            onBurstComplete?.Invoke(launchIndex);
        }
        public void HitTarget(BattleHitData hitData)
        {
            onPerHitTarget?.Invoke(hitData);
        }

        public void HitPos(Vector2 targetPos)
        {
            onPerHit?.Invoke(targetPos);
        }
        #endregion

        #region 发射点获取
        public Transform GetLaunchTrans(int index)
        {
            if (launchTranses == null)
                return null;
            else
                return launchTranses[index % launchTranses.Length];
        }
        public Vector2 GetLaunchDir(int index)
        {
            var trans = GetLaunchTrans(index);
            if (trans == null)
                return backupLaunchDir;
            else
                return trans.up;
        }
        public Vector2 GetLaunchPoint(int index)
        {
            if (launchTranses == null)
                return backupLaunchPos;
            else
                return GetLaunchTrans(index).position;
        }
        #endregion

        public AttackData GetAttackData() => attackData;
    }
}