using System.Collections.Generic;
using BattleActor;
using SimpleVFXSystem;
using UnityEngine;

namespace RTSDemo.Zone
{
    public class BuffZoneManager : Singleton<BuffZoneManager>
    {
        private HashSet<BuffZone> buffZones;
        private Transform zoneRoot;

        public BuffZone CreateZone(string zoneID, GameObject caster, Vector2 position, ZoneData zoneData)
        {
            GameObject zoneObj = new GameObject("zone_" + zoneID);
            zoneObj.transform.position = position;
            zoneObj.transform.SetParent(zoneRoot);

            //添加zone rigid组件
            zoneObj.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

            //添加Collider组件，并设置剔除层级
            var zoneCollider = zoneObj.AddComponent<CircleCollider2D>();
            zoneCollider.isTrigger = true;
            zoneCollider.radius = zoneData.zoneRadius;
            var casterTeam = BattleActorService.GetTeam(caster.layer);
            switch (zoneData.zoneTeam)
            {
                case TeamRelation.None:
                    zoneCollider.excludeLayers = BattleActorService.TeamLayerMasks[TeamMask.Both];
                    break;
                case TeamRelation.SameSide:
                    zoneCollider.excludeLayers = BattleActorService.TeamLayerMasks[BattleActorService.GetOppositeTeam(casterTeam)];
                    break;
                case TeamRelation.OppositeSide:
                    zoneCollider.excludeLayers = BattleActorService.TeamLayerMasks[casterTeam];
                    break;
                default:
                    break;
            }
            
            BuffZone buffZone = zoneObj.AddComponent<BuffZone>();
            buffZone.Init(zoneData.buffIDs, zoneData.duration<0, zoneData.duration, zoneData.refreshRate);

            //注册zone
            if (buffZones == null)
                buffZones = new HashSet<BuffZone>();
            buffZones.Add(buffZone);

            //创建Zone特效
            VFXManager.Instance.PlayVFX(zoneData.vfxKey, position, 0, zoneData.zoneRadius, null, new GameObject[]{ zoneObj });
            return buffZone;
        }
        public void ReleaseZone(BuffZone zone)
        {
            if (buffZones.Contains(zone))
            {
                buffZones.Remove(zone);
                zone.CleanUp();
            }
        }
        #region 战斗节点处理
        public void StartBattle()
        {
            buffZones = new HashSet<BuffZone>();
            zoneRoot = new GameObject("[BuffZone]").transform;
        }
        public void CleanUpBattle()
        {
            if (buffZones != null)
            {
                foreach (var zone in buffZones)
                {
                    Destroy(zone.gameObject);
                }
                buffZones.Clear();
            }
            Destroy(zoneRoot.gameObject);
        }
        #endregion
    }
}
