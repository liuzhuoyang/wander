using Sirenix.OdinInspector;
using UnityEngine;
using SimpleVFXSystem;
using SimpleAudioSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BattleLaunch
{
    public abstract class LaunchableData : ScriptableObject
    {
        [BoxGroup("子弹基本参数")] public float aimRadius;
        [BoxGroup("子弹基本参数")] public float effectRange;
        [TabGroup("特效参数")] public VFXData vfx_impact;
        [TabGroup("音效参数")] public AudioData sfx_impact;
        public LaunchableType launchableType;
        public string m_launchableKey => this.name;
    }
}
