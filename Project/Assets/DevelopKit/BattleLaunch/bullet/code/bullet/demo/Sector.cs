using System.Collections.Generic;
using BattleActor;
using UnityEngine;

namespace BattleLaunch.Bullet
{
    public class Sector : BulletBase
    {
        [SerializeField] private float sectorAngle;
        private Vector3 direction;
        private const int RAY_SEG = 8;

        public override void Init(AttackData attackData, TeamMask excludeTeam, BulletData_SO bulletData)
        {
            base.Init(attackData, excludeTeam, bulletData);
            m_rigid.simulated = false;
        }
        public override void Launch(Vector2 startDir, Vector2 targetPos, Transform launchTrans, IBattleActor targetActor)
        {
            this.direction = startDir;

            HashSet<IBattleActor> actors = new HashSet<IBattleActor>(64);
            RaycastHit2D[] hit = new RaycastHit2D[16];
            Ray2D ray = new Ray2D(transform.position, direction);

            IBattleActor front = null;
            for (int i = 0; i < RAY_SEG; i++)
            {
                ray.direction = Quaternion.Euler(0, 0, -0.5f * sectorAngle + sectorAngle * i / (RAY_SEG - 1f)) * direction;
                int iterate = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hit, effectRange, ~(1 << BattleActorService.TeamLayerMasks[excludeTeam]));
                for (int j = 0; j < iterate; j++)
                {
                    front = hit[j].collider.GetComponent<IBattleActor>();
                    if (front != null)
                    {
                        if (actors.Add(front))
                        {
                            OnHit(front, front.GetHitPos(transform.position));
                            if (actors.Count >= 64)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            Destroy(gameObject, 0.1f);
        }

        void OnDrawGizmos()
        {
            Ray2D ray = new Ray2D(transform.position, direction);
            ray.direction = Quaternion.Euler(0, 0, -0.5f * sectorAngle) * direction;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ray.origin, ray.direction * effectRange);
            ray.direction = Quaternion.Euler(0, 0, 0.5f * sectorAngle) * direction;
            Gizmos.DrawRay(ray.origin, ray.direction * effectRange);
        }
    }
}