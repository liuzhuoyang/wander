using BattleBuff;
using SimpleVFXSystem;

public abstract class VFX_Buff<T> : VFXMono where T : Buff
{
    public T buffData;
    public void SetControlBuff(T buff)
    {
        buffData = buff;
        InitBuffData();
    }
    protected abstract void InitBuffData();
}