using UnityEngine;

[DefaultExecutionOrder(1000)]
public class UIDynamicTargetHandler : MonoBehaviour
{
   public UIDynamicTargetData data;

   public void Start()
   {
        if(data == null)
        {
            Debug.LogError($"=== UIDynamicTargetAttacher: data 为空，路径: {UtilityDebug.GetFullTransformPath(transform)} ===");
            return;
        }

        UIDynamicControl.Instance.AddUIDynamicTarget(data.targetName, transform);
   }
}
