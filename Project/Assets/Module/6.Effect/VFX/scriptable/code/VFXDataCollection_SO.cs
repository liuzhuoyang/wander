using UnityEngine;

namespace SimpleVFXSystem
{
    [CreateAssetMenu(fileName = "VFXDataCollection_SO", menuName = "DevelopBasic/VFX_System/VFXDataCollection_SO")]
    public class VFXDataCollection_SO : DataCollection<VFXData_SO>
    {
        public override VFXData_SO GetDataByKey(string key) => DataList.Find(data => data.vfxKey == key);
    }
}