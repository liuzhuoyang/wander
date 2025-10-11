using BattleActor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BattleBuff.Ability
{
    public abstract class AbilityData : BuffData
    {
        [SerializeField] private BuffData[] buffs;
        [SerializeField, Tooltip("-1表示可触发无数次")] protected int triggerCount = -1;
        [SerializeField, ShowIf("IsArea"), BoxGroup("范围参数")] protected TeamMask areaTeamMask;
        [SerializeField, ShowIf("IsArea"), BoxGroup("范围参数")] protected float areaRadius;
        protected abstract bool IsArea();
        protected string[] GetAddBuffIDs()
        {
            // 获取所有Buff的ID
            string[] buffIDs = new string[buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                buffIDs[i] = buffs[i].m_buffID;
            }
            return buffIDs;
        }
    }
}
