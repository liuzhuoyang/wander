using UnityEngine;
using BattleBuff;
using BattleActor;
using SimpleVFXSystem;

namespace RTSDemo.Zone
{
    [CreateAssetMenu(fileName = "CreateZoneBuff_SO", menuName = "Assets/Buff/CreateZoneBuff_SO")]
    public class CreateZoneBuff_SO : BuffData_SO
    {
        [SerializeField] private float zoneRadius;
        [SerializeField] private float refreshRate;
        [SerializeField] private float duration;
        [SerializeField] private bool isPermanent;
        [SerializeField] private TeamRelation zoneTeam;
        [SerializeField] private BuffData_SO[] zoneBuffs;

        [Header("区域表现")]
        [SerializeField] private VFXData_SO zoneVFX;

        public override bool m_positionbasedBuff => true;
        protected override Buff GetBuffInstance()
        {
            string[] buffIDs = new string[zoneBuffs.Length];
            for (int i = 0; i < zoneBuffs.Length; i++)
            {
                buffIDs[i] = zoneBuffs[i].m_buffID;
            }
            return new Buff_CreateZone(m_buffID, new ZoneData(isPermanent ? -1 : duration, refreshRate, zoneRadius, zoneTeam, buffIDs, zoneVFX ? zoneVFX.vfxKey : string.Empty));
        }
    }
}