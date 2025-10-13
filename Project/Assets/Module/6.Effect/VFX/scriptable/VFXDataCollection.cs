using UnityEngine;

namespace SimpleVFXSystem
{
    [CreateAssetMenu(fileName = "all_vfx", menuName = "OniData/Effect/VFX/VFXDataCollection")]
    public class VFXDataCollection : DataCollection<VFXData>
    {
        public override VFXData GetDataByKey(string key) => DataList.Find(data => data.vfxKey == key);
    }
}