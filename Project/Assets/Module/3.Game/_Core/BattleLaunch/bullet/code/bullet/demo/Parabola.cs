using BattleActor;
using UnityEngine;

namespace BattleLaunch.Bullet
{
    public class Parabola : BulletBase
    {
        [Header("Parabola")]
        [SerializeField] protected float rotateRound = 1.5f;
        [SerializeField] protected float maxHeight = 4f;
        protected Vector2 startPosition;
        protected Vector2 targetPosition;
        protected float dir = 1;

        public override void Init(AttackData attackData, TeamMask excludeTeam, BulletData bulletData)
        {
            base.Init(attackData, excludeTeam, bulletData);
            m_rigid.simulated = false;
        }
        public override void Launch(Vector2 startDir, Vector2 targetPos, Transform launchTrans,  IBattleActor targetActor)
        {
            this.startPosition = transform.position;
            this.targetPosition = targetPos + Random.insideUnitCircle * aimRadius;
            this.dir = Mathf.Sign(targetPos.x - transform.position.x);
        }
        public override void BattleUpdate()
        {
            lifeTimer += Time.deltaTime;

            // 计算当前时间的插值比例
            float t = lifeTimer / life;

            // 计算抛物线位置
            Vector3 currentPosition = CalculateParabolaPosition(startPosition, targetPosition, t);

            // 更新位置
            transform.position = currentPosition;

            // 调整缩放效果：小->大->小
            float scale = Mathf.Sin(t*t * Mathf.PI) * 0.25f + 1.25f; // 0.7到1.0之间变化
            transform.localScale = new Vector3(scale, scale, scale);

            // 优化旋转计算：保持匀速旋转
            transform.rotation = Quaternion.Euler(0, 0, -dir * lifeTimer * rotateRound * 360); // 移除速度系数，保持匀速
            // 检查是否到达目标位置
            if (lifeTimer >= life)
            {
                DamageTargetsInRange(transform.position);
                OnHit(null, targetPosition);
            }
        }

        private Vector3 CalculateParabolaPosition(Vector3 start, Vector3 end, float t)
        {
            // 计算水平方向的位置
            Vector3 horizontalPosition = Vector3.Lerp(start, end, t);

            // 计算垂直方向的高度（抛物线公式）
            float y = 4 * maxHeight * (t-t*t);

            // 组合最终位置，确保终点高度与目标一致
            float targetY = Mathf.Lerp(start.y, end.y, t);
            return new Vector3(horizontalPosition.x, targetY + y, horizontalPosition.z);
        }
    }
}