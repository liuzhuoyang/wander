using BattleActor.Unit;
using BattleBuff;
using SimpleVFXSystem;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatusBuffData", menuName = "RTS_Demo/Actor/Unit/Buff/UnitStatusBuffData")]
public class UnitStatusBuffData : BuffData
{
    [Header("Status Life Time")]
    public bool isPermanent;
    [HideIf("isPermanent")] public float durtaion;
    [Header("Attribute Modify")]
    public UnitAttributeModifier attributeModifier;
    public VFXData_SO statusVFX;
    protected override Buff GetBuffInstance()
    {
        UnitStatusBuff buff;
        if (isPermanent)
        {
            buff = new UnitStatusBuff(m_buffID, attributeModifier, m_buffTag);
        }
        else
        {
            buff = new UnitStatusBuff(m_buffID, attributeModifier, durtaion, m_buffTag);
        }
        buff.SetVFXData(statusVFX?statusVFX.vfxKey:string.Empty);
        return buff;
    }
}
