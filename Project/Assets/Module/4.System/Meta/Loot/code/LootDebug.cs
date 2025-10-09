public class LootDebug : DebuggerSharedMenu
{
    #region debug
    public void OnGetLoot(int index)
    {
        LootSystem.Instance.OnDebugGetLoot(index);
    }
    #endregion
}
