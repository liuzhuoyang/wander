using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEditor;

namespace SimpleAudioSystem
{
    //用于存放成组的音频，播放时随机选中一个播放
    [CreateAssetMenu(fileName = "audio_group_data", menuName = "OniData/Effect/Audio/AudioGroupData")]
    public class AudioGroupData : AudioData
    {
        [InfoBox("若配置设置了多个音频，随机选中播放，否则播放默认的音频")]
        [SerializeField] private AssetReferenceT<AudioClip>[] clips;
        [SerializeField, ReadOnly] private string[] clipKeys;
        //获取音频clip，用于AudioManager执行播放
        public override string GetClipKey()
        {
            if (clips == null || clips.Length == 0)
                return base.GetClipKey();
            else
                return clipKeys[Random.Range(0, clipKeys.Length)];
        }
        //获取所有的音频clips，用于AudioManager预先加载
        public string[] GetClipKeys()
        {
            if (clips == null || clips.Length == 0)
                return new string[] { base.GetClipKey() };
            else
                return clipKeys;
        }
#if UNITY_EDITOR
        [Button("初始化数据")]
        public void InitData()
        {
            clipKeys = new string[clips.Length];
            for (int i = 0; i < clips.Length; i++)
            {
                clipKeys[i] = clips[i].editorAsset.name;
            }
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
