using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SimpleVFXSystem
{
    [CreateAssetMenu(fileName = "vfx_data", menuName = "OniData/Effect/VFX/VFXData")]
    public class VFXData : ScriptableObject
    {
        public AssetReference vfx_ref;
        [LabelText("VFX的创建方式")]
        public VFXManageMode manageMode = VFXManageMode.Pooled;

        public string vfxKey => this.name;
    }
}