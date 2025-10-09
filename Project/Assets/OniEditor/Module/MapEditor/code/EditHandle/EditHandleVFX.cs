#if UNITY_EDITOR
using UnityEngine;

public class EditHandleVFX : EditHandle
{
    public override void OnErase()
    {
        base.OnErase();
        MapControl.Instance.LevelData.RemoveVFXHandle(this);
    }

}

#endif