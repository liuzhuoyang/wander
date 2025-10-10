using BattleActor.Unit;
using SimpleVFXSystem;
using UnityEngine;

//修改目标颜色，并附着在目标身上
public class VFXChangeUnitColor : VFXBuff<UnitStatusBuff>
{
    [SerializeField] private Color colorOverride;
    private UnitViewBasic targetUnit;
    
    protected override void VFXBegin()
    {
        targetUnit = controlObjects[0].GetComponent<UnitViewBasic>();
        targetUnit.ChangeRendererColor(colorOverride);
    }
    protected override void InitBuffData()
    {
        buffData.onBuffRemoved += VFXEnd;
    }
    protected override void VFXEnd()
    {
        buffData.onBuffRemoved -= VFXEnd;
        targetUnit.ResetRendererColor();
        targetUnit = null;
        base.VFXEnd();
    }
}