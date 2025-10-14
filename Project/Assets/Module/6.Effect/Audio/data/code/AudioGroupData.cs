using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace SimpleAudioSystem
{
    //用于存放成组的音频，播放时随机选中一个播放
    [CreateAssetMenu(fileName = "audio_group_data", menuName = "OniData/Effect/Audio/AudioGroupData")]
    public class AudioGroupData : AudioData
    {
        [InfoBox("若配置设置了多个音频，随机选中播放，否则播放默认的音频")]
        [SerializeField] private AssetReferenceT<AudioClip>[] clips;
        public override AssetReferenceT<AudioClip> GetClipRef()
        {
            if (clips == null || clips.Length == 0)
                return base.GetClipRef();
            else
                return clips[Random.Range(0, clips.Length)];
        }
    }
}
