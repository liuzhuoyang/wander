using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SimpleAudioSystem
{
    //用于存放长容量音频，多为背景音乐和环境音
    [CreateAssetMenu(fileName = "audio_ref_data", menuName = "OniData/Effect/Audio/AudioRefData")]
    public class AudioRefData : ScriptableObject
    {
        public AssetReference assetReference;
    }
}
