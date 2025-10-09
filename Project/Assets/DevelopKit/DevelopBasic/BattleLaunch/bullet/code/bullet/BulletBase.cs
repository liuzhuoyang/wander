using System;
using UnityEngine;
using BattleActor;
using Sirenix.OdinInspector;

namespace BattleLaunch.Bullet
{
    public abstract class BulletBase : BattleBehaviour
    {
        [SerializeField] protected float life = 2;
        [SerializeField] protected float bulletSpeed;
        [SerializeField, ReadOnly] protected float effectRange;
        [SerializeField, ReadOnly] protected float aimRadius;

        protected bool isHit = false;
        protected TeamMask excludeTeam; //忽略命中检测的队伍
        protected AttackData attackData;

        protected Rigidbody2D m_rigid;
        protected float lifeTimer;

        protected string vfx_impact;
        protected string sfx_impact;

        protected Action<BattleHitData> onHitTarget;

        public virtual void Init(AttackData attackData, TeamMask excludeTeam, BulletData_SO bulletData)
        {
            this.m_rigid = GetComponent<Rigidbody2D>();
            this.excludeTeam = excludeTeam;
            this.attackData = attackData;
            this.effectRange = bulletData.effectRange;
            this.aimRadius = bulletData.aimRadius;
            this.vfx_impact = bulletData.vfx_impact != null ? bulletData.vfx_impact.vfxKey : string.Empty;
            this.sfx_impact = bulletData.sfx_impact != null ? bulletData.sfx_impact.name : string.Empty;

            var team = excludeTeam;
            if (team == TeamMask.Enemy)
                gameObject.layer = BattleActorService.EnemyLayer;
            else if (team == TeamMask.Player)
                gameObject.layer = BattleActorService.FriendlyLayer;
            else
                gameObject.layer = Physics2D.IgnoreRaycastLayer;
                
            m_rigid.excludeLayers = BattleActorService.TeamLayerMasks[excludeTeam];
            lifeTimer = 0;
        }
        public abstract void Launch(Vector2 startDir, Vector2 targetPos, Transform launchTrans, IBattleActor targetActor);
        public override void BattleUpdate()
        {
            lifeTimer += Time.deltaTime;
            if (lifeTimer >= life)
            {
                OnHit(null, transform.position);
            }
        }
        public BulletBase OnHitTarget(Action<BattleHitData> hitCallback)
        {
            onHitTarget = hitCallback;
            return this;
        }
        protected void DamageTargetsInRange(Vector2 pos)
        {
            var effectTarget = BulletManager.Instance.FindTargetsInRange(pos, effectRange, BattleActorService.GetOppositeTeam(excludeTeam));
            if (effectTarget != null)
            {
                foreach (var target in effectTarget)
                {
                    if (!IBattleActor.IsInvalid(target))
                    {
                        target.TakeDamage(attackData, target.GetHitPos(pos));
                        OnHitActor(target, true);
                    }
                }
            }
        }
        protected void OnHit(IBattleActor other, Vector2 impactPoint)
        {
            if (isHit)
                return;
            isHit = true;

            if (IBattleActor.IsInvalid(other))
                OnHitPos(impactPoint);
            else
                OnHitActor(other, false);

            BulletManager.PlayBulletImpactEffect(sfx_impact, vfx_impact, impactPoint, effectRange);
            Destroy(gameObject);
        }
        protected void OnHitPos(Vector2 hitPos)
        {
            var hitData = new BattleHitData(gameObject, null, hitPos, false);
            onHitTarget?.Invoke(hitData);
        }
        protected void OnHitActor(IBattleActor hitActor, bool hitByRange)
        {
            var hitData = new BattleHitData(gameObject, hitActor, hitActor.position, hitByRange);
            onHitTarget?.Invoke(hitData);
        }
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var battleActor = other.GetComponent<IBattleActor>();
            if (!IBattleActor.IsInvalid(battleActor))
            {
                Vector2 closePoint = other.ClosestPoint(transform.position);
                battleActor?.TakeDamage(attackData, closePoint);
                OnHit(battleActor, closePoint);
            }
        }
        public void OverrideEffectRange(float newRange) => effectRange = newRange;
    }
}