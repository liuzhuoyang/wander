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
    [CreateAssetMenu(fileName = "AudioDataCollection_SO", menuName = "DevelopBasic/AudioSystem/AudioDataCollection_SO")]
    public class AudioDataCollection_SO : ScriptableObject
    {
        public List<AudioRefData_SO> bgm_list;
        public List<AudioRefData_SO> amb_list;
        public List<AudioData_SO> sfx_list;
        public List<AudioGroupData_SO> sfx_group_list;

        private Dictionary<string, AudioRefData_SO> bgm_dict;
        private Dictionary<string, AudioRefData_SO> amb_dict;
        private Dictionary<string, AudioData_SO> sfx_dict;

        private const string BGM_DIRECTOR_KEY = "/bgm";
        private const string AMB_DIRECTOR_KEY = "/amb";
        private const string SFX_DIRECTOR_KEY = "/sfx";
        private const string SFX_GROUP_DIRECTOR_KEY = "/sfx_group";

        void OnEnable()
        {
            Debug.Log("-------Initializing Audio Data Collection-------");
            bgm_dict = new Dictionary<string, AudioRefData_SO>();
            amb_dict = new Dictionary<string, AudioRefData_SO>();
            sfx_dict = new Dictionary<string, AudioData_SO>();

            foreach (var item in bgm_list)
            {
                bgm_dict.Add(item.name, item);
            }
            foreach (var item in amb_list)
            {
                amb_dict.Add(item.name, item);
            }
            foreach (var item in sfx_list)
            {
                sfx_dict.Add(item.name, item);
            }
            foreach (var item in sfx_group_list)
            {
                sfx_dict.Add(item.name, item);
            }

            Debug.Log("-------Finish Initializing Audio Data Collection-------");
        }
        public AudioClip GetSFXClipByKey(string key)
        {
            return sfx_dict[key].GetClip();
        }
        public AssetReference GetBGMRefByKey(string key)
        {
            return bgm_dict[key].assetReference;
        }
        public AssetReference GetAMBRefByKey(string key)
        {
            return amb_dict[key].assetReference;
        }

#if UNITY_EDITOR
        [Button("Find All Data")]
        public void FindAllAudioData()
        {
            string path = AssetDatabase.GetAssetPath(this);
            path = Path.GetDirectoryName(path);

            bgm_list = GetDataFromPath<AudioRefData_SO>(path + BGM_DIRECTOR_KEY);
            amb_list = GetDataFromPath<AudioRefData_SO>(path + AMB_DIRECTOR_KEY);
            sfx_list = GetDataFromPath<AudioData_SO>(path + SFX_DIRECTOR_KEY);
            sfx_group_list = GetDataFromPath<AudioGroupData_SO>(path + SFX_GROUP_DIRECTOR_KEY);

            EditorUtility.SetDirty(this);
        }
        List<T> GetDataFromPath<T>(string _path) where T : ScriptableObject
        {
            var asset = AssetsFinder.FindAllAssetsOfAllSubFolders<T>(_path);
            if(asset != null && asset.Count > 0)
                return asset;
            else
                return new List<T>();
        }
#endif
    }
}
