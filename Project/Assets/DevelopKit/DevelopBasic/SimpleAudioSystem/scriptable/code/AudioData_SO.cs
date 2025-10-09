using UnityEngine;

namespace SimpleAudioSystem
{
    //用于存放单个短音频
    [CreateAssetMenu(fileName = "AudioData_SO", menuName = "DevelopBasic/AudioSystem/AudioData_SO")]
    public class AudioData_SO : ScriptableObject
    {
        [SerializeField] protected AudioClip clip;
        public virtual AudioClip GetClip() => clip;
    }
}
