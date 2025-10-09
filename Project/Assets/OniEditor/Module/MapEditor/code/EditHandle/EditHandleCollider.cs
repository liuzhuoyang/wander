#if UNITY_EDITOR
public class EditHandleCollider : EditHandle
{
    public override void OnErase()
    {
        MapControl.Instance.LevelData.RemoveColliderHandle(this);
        base.OnErase();
    }
}
#endif
