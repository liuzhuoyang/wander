using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SimpleAudioSystem
{
    /// <summary>
    /// 用于收集AudioData_SO的集合
    /// </summary>
    [CreateAssetMenu(fileName = "all_audio", menuName = "OniData/Effect/Audio/AudioDataCollection")]
    public class AudioDataCollection : GameDataCollectionBase
    {
        public List<AudioData> bgm_list;
        public List<AudioData> amb_list;
        public List<AudioData> sfx_list;
        public List<AudioGroupData> sfx_group_list;

#if UNITY_EDITOR
        [Button("Find All Data", ButtonSizes.Gigantic)]
        public void FindAllAudioData()
        {
            string path = AssetDatabase.GetAssetPath(this);
            path = Path.GetDirectoryName(path);

            bgm_list = GetDataFromPath<AudioData>("Assets").FindAll(a => a.name.Contains("bgm"));
            amb_list = GetDataFromPath<AudioData>("Assets").FindAll(a => a.name.Contains("amb"));
            sfx_list = GetDataFromPath<AudioData>("Assets").FindAll(a=>a is not AudioGroupData);

            sfx_group_list = GetDataFromPath<AudioGroupData>("Assets");

            EditorUtility.SetDirty(this);
        }
        List<T> GetDataFromPath<T>(string _path) where T : ScriptableObject
        {
            var asset = FileFinder.FindAllAssetsOfAllSubFolders<T>(_path);
            if(asset != null && asset.Count > 0)
                return asset;
            else
                return new List<T>();
        }
#endif
    }
    public static class AllBGM
    {
        private static Dictionary<string, AudioData> dictBGMData;

        //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictBGMData = new Dictionary<string, AudioData>();
            AudioDataCollection collection = GameDataControl.Instance.Get("all_audio") as AudioDataCollection;
            foreach (AudioData data in collection.bgm_list)
            {
                dictBGMData.Add(data.name, data);
            }
        }
        public static AudioData GetBGMData(string key)
        {
            if (dictBGMData.TryGetValue(key, out var data))
                return data;
            Debug.LogError("No BGM Data Found By Key: " + key);
            return null;
        }
    }
    public static class AllAMB
    {
        private static Dictionary<string, AudioData> dictAMBData;

        //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictAMBData = new Dictionary<string, AudioData>();
            AudioDataCollection collection = GameDataControl.Instance.Get("all_audio") as AudioDataCollection;
            foreach (AudioData data in collection.amb_list)
            {
                dictAMBData.Add(data.name, data);
            }
        }
        public static AudioData GetAMBData(string key)
        {
            if (dictAMBData.TryGetValue(key, out var data))
                return data;
            Debug.LogError("No AMB Data Found By Key: " + key);
            return null;
        }
    }
    public static class AllSFX
    {
        private static Dictionary<string, AudioData> dictSFXData;

        //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictSFXData = new Dictionary<string, AudioData>();
            AudioDataCollection collection = GameDataControl.Instance.Get("all_audio") as AudioDataCollection;
            foreach (AudioData data in collection.sfx_list)
            {
                dictSFXData.Add(data.name, data);
            }
            foreach(AudioData data in collection.sfx_group_list)
            {
                dictSFXData.Add(data.name, data);
            }
        }
        public static AudioData GetSFXData(string key)
        {
            if (dictSFXData.TryGetValue(key, out var data))
                return data;
            Debug.LogError("No SFX Data Found By Key: " + key);
            return null;
        }
        public static Dictionary<string, AudioData> GetDictSFXData() => dictSFXData;
    }
}