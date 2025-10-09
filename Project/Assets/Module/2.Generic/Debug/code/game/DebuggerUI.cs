using UnityEngine;

public class DebuggerUI : MonoBehaviour
{
    public void OnDebugLogUIDynamicTarget()
    {
        foreach (var item in UIDynamicControl.Instance.uiTargetDict)
        {
            Debug.Log(item.Key + " " + item.Value.position);
        }
    }
}
