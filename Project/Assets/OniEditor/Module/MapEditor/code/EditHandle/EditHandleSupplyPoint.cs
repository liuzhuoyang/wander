#if UNITY_EDITOR
using UnityEngine;

public class EditHandleSupplyPoint : EditHandle
{
/*
    public void Init(string targetName, int index = 0)
    {
        base.Init(targetName, index);
    }*/

    public override void OnErase()
    {
        base.OnErase();
        //MapControl.Instance.LevelData.RemoveSupplyPointHandle(this);
    }
}
#endif