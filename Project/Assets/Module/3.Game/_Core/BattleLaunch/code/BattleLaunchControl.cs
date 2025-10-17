using System.Collections.Generic;
using BattleActor;
using BattleLaunch.Bullet;
using UnityEngine;
using UnityEngine.Timeline;

namespace BattleLaunch
{
    [System.Serializable]
    public struct LaunchLayer
    {
        public Transform[] launchTrans;
    }
    [HideInMenu]
    public class BattleLaunchControl : MonoBehaviour
    {
        private List<BattleLaunchCommand_Batch> launchBatches;
        public BattleLaunchControl()
        {
            launchBatches = new List<BattleLaunchCommand_Batch>();
        }
        public void Clear()
        {
            launchBatches.Clear();
        }
        public BattleLaunchCommand_Batch AddLaunch(BattleLaunchCommand_Batch battleLaunchBatch)
        {
            launchBatches.Add(battleLaunchBatch);
            return battleLaunchBatch;
        }
        public void UpdateLaunching()
        {
            BattleLaunchCommand_Batch front = null;
            for (int i = launchBatches.Count - 1; i >= 0; i--)
            {
                front = launchBatches[i];
                if (front.m_countLeft <= 0)
                {
                    front.AllLaunchComplete();
                    launchBatches.Remove(front);
                    continue;
                }
                front.UpdateLaunch(DoLaunch);
            }
        }
        public void AbortLaunch()
        {
            if (launchBatches != null)
                launchBatches.Clear();
        }
        public void DoLaunch(BattleLaunchCommand_Batch launchBatch)
        {
            int burstIndex = launchBatch.m_countLeft;
            for (int i = 0; i < launchBatch.m_spreadCount; i++)
            {
                Vector2 startPos = launchBatch.GetLaunchPoint(burstIndex);
                Vector2 startDir = GetOffsetDirectionBySpreadIndex(i, launchBatch.GetLaunchDir(burstIndex), launchBatch.m_spreadAngle);
                Vector2 targetPos = launchBatch.m_targetPosList[burstIndex % launchBatch.m_targetPosList.Count];
                targetPos = GetOffsetPositionBySpreadIndex(i, startPos, targetPos, launchBatch.m_spreadAngle);

                var launchTrans = launchBatch.GetLaunchTrans(burstIndex);
                IBattleActor targetActor = null;
                if (launchBatch.m_targetActorList != null)
                {
                    targetActor = launchBatch.m_targetActorList[burstIndex % launchBatch.m_targetActorList.Count];
                }

                var bullet = BulletManager.Instance.GetBulletInstance(launchBatch.m_launchableKey, launchBatch.m_excludeTeam, launchBatch.GetAttackData());
                bullet.transform.position = startPos;
                bullet.OnHitTarget(launchBatch.HitTarget);

                bullet.Launch(startDir, targetPos, launchTrans, targetActor);
            }
            launchBatch.PerLaunchSuccess(burstIndex);
        }
        //根据参数执行简单的直接发射
        public static void ExcuteSingleLaunchImmediate(BattleLaunchCommand_Direct battleLaunchSingle)
        {
            var bullet = BulletManager.Instance.GetBulletInstance(battleLaunchSingle.m_launchableKey, battleLaunchSingle.m_excludeTeam, battleLaunchSingle.m_attackData);
            bullet.OnHitTarget(battleLaunchSingle.HitTarget);
            bullet.transform.position = battleLaunchSingle.m_launchPos;
            Vector2 startDir = battleLaunchSingle.m_targetActor.position - battleLaunchSingle.m_launchPos;

            bullet.Launch(startDir, battleLaunchSingle.m_targetActor.position, battleLaunchSingle.m_launchTrans, battleLaunchSingle.m_targetActor);
        }
        static Vector2 GetOffsetPositionBySpreadIndex(int spreadIndex, Vector2 startPos, Vector2 mainHitPos, float angle)
        {
            Vector2 dir = mainHitPos - startPos;
            dir = GetOffsetDirectionBySpreadIndex(spreadIndex, dir, angle);
            return startPos + dir;
        }
        static Vector2 GetOffsetDirectionBySpreadIndex(int spreadIndex, Vector2 mainDirection, float angle)
        {
            if (spreadIndex == 0) return mainDirection;
            // 计算偏移角度：(-1)^(index) * ((index + 1) / 2) * angle
            float offsetAngle = Mathf.Pow(-1, spreadIndex) * Mathf.CeilToInt(spreadIndex / 2f) * angle;
            return Quaternion.Euler(0, 0, offsetAngle) * mainDirection;
        }
    }
}
