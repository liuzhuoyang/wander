using BattleBuff;

namespace RTSDemo.Zone
{
    public class BuffCreateZone : PositionBasedBuff
    {
        private ZoneData zoneData;

        public BuffCreateZone(string buffID, ZoneData zoneData)
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