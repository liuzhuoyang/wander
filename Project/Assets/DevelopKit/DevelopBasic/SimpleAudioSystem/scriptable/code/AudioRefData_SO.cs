using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SimpleAudioSystem
{
    //用于存放长容量音频，多为背景音乐和环境音
    [CreateAssetMenu(fileName = "AudioRefData_SO", menuName = "DevelopBasic/AudioSystem/AudioRefData_SO")]
    public class AudioRefData_SO : ScriptableObject
    {
        public AssetReference assetReference;
    }
}
