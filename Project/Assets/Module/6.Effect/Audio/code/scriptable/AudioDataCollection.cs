using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "all_audio", menuName = "OniData/FX/Audio/AudioDataCollection")]
public class AudioDataCollection : GameDataCollectionBase
{
    [ReadOnly] public List<AudioData> ListAudioSFXData;
    [ReadOnly] public List<AudioData> ListAudioBGMData;
#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        //初始化资源列表,查询整个目录，音效不能放入当前模块
        ListAudioSFXData = AssetsFinder.FindAllAssetsOfAllSubFolders<AudioData>("Assets");
        //背景音乐都放在当前路径下
        ListAudioBGMData = AssetsFinder.FindAllAssetsOfAllSubFolders<AudioData>(path);
        EditorUtility.SetDirty(this);
    }
#endif
}

public static class AllAudio
{
    //数据游戏中使用
    public static Dictionary<string, string> dictSFX;
    public static Dictionary<string, string> dictBGM;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        AudioDataCollection dataCollection = GameDataControl.Instance.Get("all_audio") as AudioDataCollection;

        dictSFX = new Dictionary<string, string>();
        foreach (AudioData data in dataCollection.ListAudioSFXData)
        {
            dictSFX.Add(data.name, data.clipName);
        }

        dictBGM = new Dictionary<string, string>();
        foreach (AudioData data in dataCollection.ListAudioBGMData)
        {
            dictBGM.Add(data.name, data.clipName);
        }
    }
}
