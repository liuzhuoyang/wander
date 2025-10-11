using BattleActor;
using UnityEngine;

namespace BattleLaunch.Bullet
{
    public class Missile : BulletBase
    {
        public enum MissleState
        {
            TakeOff, //起飞阶段
            Dive, //俯冲向目标阶段
        }
        private MissleState missleState; //导弹状态
        [Header("Missile")]
        [SerializeField] private float minSpeedRatio = 0.2f;
        [SerializeField] private float riseTime = 0.3f; //上升时间
        [SerializeField] private float maxAlignStrength = 6; //对齐目标的修正强度
        [SerializeField] private float alignTime = 0.5f; //对齐目标的时间
        [SerializeField] private float speedUpTime = 1.2f; //加速时间

        private float stateTimer = 0; //状态计时器
        private float alignSpeed = 0;
        private Vector3 direction; //飞行方向
        private Vector3 riseDirection; //升空时的方向
        private Vector3 targetPos; //目标点
        private Vector3 offset; //偏移量
        private IBattleActor targetActor; //目标单位

        public override void Init(AttackData attackData, TeamMask excludeTeam, BulletData bulletData_SO)
        {
            base.Init(attackData, excludeTeam, bulletData_SO);
            m_rigid.simulated = false;
        }
        public override void Launch(Vector2 startDir, Vector2 targetPos, Transform launchTrans, IBattleActor targetActor)
        {
            //若存在目标单位，记录目标单位
            if (!IBattleActor.IsInvalid(targetActor))
            {
                this.targetActor = targetActor;
            }
            offset = Random.insideUnitCircle * aimRadius;
            this.targetPos = (Vector3)targetPos + offset;

            stateTimer = 0;
            alignSpeed = 0;
            direction = startDir;
            riseDirection = Quaternion.Euler(0, 0, Random.Range(-10, 10)) * direction;
            riseDirection = riseDirection.normalized;

            //初始化方向
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(direction, Vector2.up));

            missleState = MissleState.TakeOff;
        }

        public override void BattleFixedUpdate()
        {
            switch (missleState)
            {
                case MissleState.TakeOff:
                    if (stateTimer >= riseTime)
                    {
                        missleState = MissleState.Dive;
                        stateTimer = 0;
                        return;
                    }
                    stateTimer += Time.fixedDeltaTime;
                    direction = Vector3.Slerp(direction, riseDirection, EasingFunc.Easing.QuadEaseIn(stateTimer / riseTime));
                    transform.position += Time.fixedDeltaTime * direction.normalized * bulletSpeed * Mathf.Lerp(minSpeedRatio, 1, EasingFunc.Easing.QuadEaseIn(stateTimer / speedUpTime));
                    transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.up));

                    break;
                case MissleState.Dive:
                    //当目标可用时，随时刷新目标位置
                    if (!IBattleActor.IsInvalid(targetActor))
                    {
                        targetPos = targetActor.position + (Vector2)offset;
                    }
                    stateTimer += Time.fixedDeltaTime;
                    Vector3 targetDir = (targetPos - transform.position).normalized;
                    Vector3 diff = targetDir - direction;
                    alignSpeed = Mathf.Lerp(0, maxAlignStrength, Mathf.Clamp01(stateTimer / alignTime));
                    direction += diff * alignSpeed * Time.fixedDeltaTime;
                    transform.position += Time.fixedDeltaTime * direction.normalized * bulletSpeed * Mathf.Lerp(minSpeedRatio, 1, EasingFunc.Easing.QuadEaseIn((stateTimer + riseTime) / speedUpTime));
                    transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.up));

                    break;
            }
        }
        public override void BattleLateUpdate()
        {
            // 检查是否到达目标位置
            if (missleState == MissleState.Dive && Vector3.Distance(transform.position, targetPos) <= 0.5f)
            {
                //触发爆炸
                Destroy(gameObject);

                DamageTargetsInRange(transform.position);
                OnHit(null, targetPos);
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPos, 0.2f);
        }
    }
}