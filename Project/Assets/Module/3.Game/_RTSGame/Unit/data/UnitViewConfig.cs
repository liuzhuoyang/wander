using UnityEngine;

namespace RTSDemo.Unit
{
    [CreateAssetMenu(fileName = "UnitViewConfig", menuName = "RTS_Demo/Actor/Unit/UnitViewConfig")]
    public class UnitViewConfig : ScriptableObject
    {
        public Material default_Hit;
        public Material boss_Hit;
    }
}
