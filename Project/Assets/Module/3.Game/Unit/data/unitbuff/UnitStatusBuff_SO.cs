using BattleActor.Unit;
using BattleBuff;
using SimpleVFXSystem;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStatusBuff_SO", menuName = "RTS_Demo/Actor/Unit/Buff/UnitStatusBuff")]
public class UnitStatusBuff_SO : BuffData
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
