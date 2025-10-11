using BattleActor;
using UnityEngine;

namespace BattleLaunch.Bullet
{
    public class Projectile : BulletBase
    {
        protected Vector2 direction;
        protected float distanceTraveled = 0f;

        public override void Launch(Vector2 startDir, Vector2 targetPos, Transform launchTrans, IBattleActor targetActor)
        {
            float length = life * bulletSpeed;
            this.direction = startDir.normalized;
            this.direction = (direction * length + aimRadius * Random.insideUnitCircle).normalized;

            this.m_rigid = GetComponent<Rigidbody2D>();

            //初始化方向
            float angleInRadians = Mathf.Atan2(direction.y, direction.x);
            float angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            m_rigid.rotation = angleInDegrees - 90f;
        }

        public override void BattleFixedUpdate()
        {
            distanceTraveled += bulletSpeed * Time.fixedDeltaTime;
            m_rigid.MovePosition(m_rigid.position + direction * bulletSpeed * Time.fixedDeltaTime);
        }
    }
}