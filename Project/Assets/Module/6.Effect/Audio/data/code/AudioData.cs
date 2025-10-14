using UnityEngine;

namespace SimpleAudioSystem
{
    //用于存放单个短音频
    [CreateAssetMenu(fileName = "audio_data", menuName = "OniData/Effect/Audio/AudioData")]
    public class AudioData : ScriptableObject
    {
        public string clipName;

        public void InitData()
        {
            clipName = this.name;
        }
        
        [SerializeField] protected AudioClip clip;
        public virtual AudioClip GetClip() => clip;
    }
}
