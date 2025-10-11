using System.Collections.Generic;
using System.Linq;
using BattleActor;
using BattleBuff;
using UnityEngine;

namespace RTSDemo.Zone
{
    [System.Serializable]
    public struct ZoneData
    {
        public readonly float duration;
        public readonly float refreshRate;
        public readonly float zoneRadius;
        public readonly TeamRelation zoneTeam;
        public readonly string[] buffIDs;
        public readonly string vfxKey;

        public ZoneData(float duration, float refreshRate, float zoneRadius, TeamRelation zoneTeam, string[] buffIDs, string vfxKey)
        {
            this.duration = duration;
            this.refreshRate = refreshRate;
            this.zoneRadius = zoneRadius;
            this.zoneTeam = zoneTeam;
            this.buffIDs = buffIDs;
            this.vfxKey = vfxKey;
        }
    }

    public class BuffZone : BattleBehaviour
    {
        private string[] zoneBuffs;
        private bool isPermanent;
        private float refreshRate;
        private float life;
        private float timer;
        private HashSet<BuffHandler> targetHandlers;

        #region 生命周期
        internal void Init(string[] zoneBuffs, bool isPermanent, float duration, float refreshRate)
        {
            this.zoneBuffs = zoneBuffs;
            this.isPermanent = isPermanent;
            this.life = duration;
            this.refreshRate = refreshRate;

            targetHandlers = new HashSet<BuffHandler>();
            timer = 0;
        }
        public override void BattleUpdate()
        {
            if (!isPermanent)
            {
                //刷新范围内的Buff
                timer += Time.deltaTime * refreshRate;
                if (timer >= 1)
                {
                    foreach(var handler in targetHandlers.ToHashSet())
                    {
                        if (handler == null)
                        {
                            targetHandlers.Remove(handler);
                            continue;
                        }
                        else
                        {
                            ApplyAllBuffs(handler);
                        }
                    }
                    timer = 0;
                }
                //更新生命
                life -= Time.deltaTime;
                if (life <= 0)
                {
                    BuffZoneManager.Instance.ReleaseZone(this);
                }
            }
        }
        internal void CleanUp()
        {
            Destroy(gameObject);
        }
        #endregion
        void ApplyAllBuffs(BuffHandler targetHandler)
        {
            foreach (var buffID in zoneBuffs)
            {
                targetHandler.TryAddBuff(buffID);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var handler = other.GetComponent<BuffHandler>();
            if (handler != null)
            {
                targetHandlers.Add(handler);
                ApplyAllBuffs(handler);
            }
        }
        void OnTriggerExit2D(Collider2D other)
        {
            var handler = other.GetComponent<BuffHandler>();
            if (handler != null)
            {
                targetHandlers.Remove(handler);
            }
        }
    }
}
