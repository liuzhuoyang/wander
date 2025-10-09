using Sirenix.OdinInspector;
using UnityEngine;

namespace SimpleAudioSystem
{
    //用于存放成组的音频，播放时随机选中一个播放
    [CreateAssetMenu(fileName = "AudioGroupData_SO", menuName = "DevelopBasic/AudioSystem/AudioGroupData_SO")]
    public class AudioGroupData_SO : AudioData_SO
    {
        [InfoBox("若配置设置了多个音频，随机选中播放，否则播放默认的音频")]
        [SerializeField] private AudioClip[] clips;
        public override AudioClip GetClip()
        {
            if (clips == null || clips.Length == 0)
                return base.GetClip();
            else
                return clips[Random.Range(0, clips.Length)];
        }
    }
}
