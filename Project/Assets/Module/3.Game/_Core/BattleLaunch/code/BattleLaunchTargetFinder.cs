using System.Collections.Generic;
using UnityEngine;
using BattleActor;
using Sirenix.OdinInspector;

namespace BattleLaunch
{
    public class BattleLaunchTargetFinder : MonoBehaviour
    {
        [Header("Target Searching")]
        [SerializeField] private TeamMask teamMask = TeamMask.Enemy;
        [SerializeField] private ActorScanOrder actorScanOrder = ActorScanOrder.Default;
        [SerializeField] private BattleActorMotionLayerMask searchLayer;

        [SerializeField, ReadOnly] protected List<IBattleActor> cachedTarget = new List<IBattleActor>();
        
        public TeamMask m_targetTeam => teamMask;

        public void Init(TeamMask targetTeam, ActorScanOrder scanOrder, BattleActorMotionLayerMask motionLayerMask)
        {
            this.teamMask = targetTeam;
            this.actorScanOrder = scanOrder;
            this.searchLayer = motionLayerMask;
        }
        public IBattleActor GetFirstTarget()
        {
            if (cachedTarget == null || cachedTarget.Count <= 0) return null;
            return cachedTarget[0];
        }
        //获取当前保存目标列表，注意，该列表会实时变化，因此传入拷贝数据
        public List<IBattleActor> GetActiveTargetList() => new List<IBattleActor>(cachedTarget);
        //清理所有当前目标
        public void CleanCachedTarget() => cachedTarget.Clear();
        //保持当前目标，并重新获取不足的目标
        public void FlushTarget(float searchRange, int searchTargetLimit)
        {
            //删除已经不可瞄准的目标
            cachedTarget.RemoveAll(item => IBattleActor.IsInvalid(item));
            for (int i = cachedTarget.Count - 1; i >= 0; i--)
            {
                //目标失效则移除
                if (IBattleActor.IsInvalid(cachedTarget[i]))
                    cachedTarget.RemoveAt(i);
                //目标超出范围，也移除
                if (Vector3.Distance(transform.position, cachedTarget[i].position) > searchRange)
                    cachedTarget.RemoveAt(i);
            }

            //补足缺失的锁定名单
            List<IBattleActor> targetListToAdd = null;
            if (cachedTarget.Count < searchTargetLimit)
            {
                //需要追加索敌的敌人数量
                int targetToAdd = searchTargetLimit - cachedTarget.Count;
                targetListToAdd = BattleActorScanSystem.Instance.FindTargets<IBattleActor>(transform.position, searchRange, actorScanOrder,
                    teamMask, (x) => IBattleActor.IncludeMotionLayer(searchLayer, x.motionLayer), targetToAdd);

                if (targetListToAdd != null && targetListToAdd.Count > 0)
                {
                    cachedTarget.AddRange(targetListToAdd);
                }
            }
        }
    }
}
