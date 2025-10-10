using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SimpleVFXSystem
{
    [CreateAssetMenu(fileName = "VFXData_SO", menuName = "DevelopBasic/VFX_System/VFXData_SO")]
    public class VFXData_SO : ScriptableObject
    {
        public AssetReference vfx_ref;
        [LabelText("VFX的创建方式")]
        public VFXManageMode manageMode = VFXManageMode.Pooled;

        public string vfxKey => this.name;
    }
}