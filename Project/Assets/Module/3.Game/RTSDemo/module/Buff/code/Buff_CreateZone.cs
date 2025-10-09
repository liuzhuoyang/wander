using BattleBuff;

namespace RTSDemo.Zone
{
    public class Buff_CreateZone : PositionBasedBuff
    {
        private ZoneData zoneData;

        public Buff_CreateZone(string buffID, ZoneData zoneData)
        {
            this.buffTypeID = buffID;
            this.zoneData = zoneData;
        }
        public override void ExcuteBuff()
        {
            BuffZoneManager.Instance.CreateZone(buffTypeID, caster, controlPosition, zoneData);
        }
    }
}