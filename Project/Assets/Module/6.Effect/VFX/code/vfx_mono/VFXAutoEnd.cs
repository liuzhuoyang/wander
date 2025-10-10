using SimpleVFXSystem;
using UnityEngine;

public class VFXAutoEnd : VFXMono
{
    [SerializeField] private float life = 1f;
    protected override void VFXBegin()
    {
        base.VFXBegin();
        StartCoroutine(TimerTick.Start(life, VFXEnd));
    }
}
