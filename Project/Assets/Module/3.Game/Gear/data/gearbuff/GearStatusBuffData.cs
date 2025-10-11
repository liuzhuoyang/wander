using UnityEngine;

using BattleBuff;
using BattleGear;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GearStatusBuffData", menuName = "RTS_Demo/Gear/Buff/GearStatusBuffData")]
public class GearStatusBuffData : BuffData
{
    [Header("Status Life Time")]
    public bool isPermanent;
    [HideIf("isPermanent")] public float durtaion;
    [Header("Attribute Modify")]
    public GearAttributeModifier attributeModifier;
    protected override Buff GetBuffInstance()
    {
        if (isPermanent)
        {
            return new GearStatusBuff(m_buffID, attributeModifier, m_buffTag);
        }
        else
        {
            return new GearStatusBuff(m_buffID, attributeModifier, durtaion, m_buffTag);
        }
    }
}
