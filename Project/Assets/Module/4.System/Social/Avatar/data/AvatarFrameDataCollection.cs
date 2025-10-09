
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_avatar_frame", menuName = "OniData/Social/Avatar/AvatarFrameDataCollection", order = 1)]
public class AvatarFrameDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<AvatarFrameData> listAvatarFrameData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listAvatarFrameData = AssetsFinder.FindAllAssetsOfAllSubFolders<AvatarFrameData>(path);
    }
#endif
}

public static class AllAvatarFrame
{
    //数据游戏中使用
    public static Dictionary<string, AvatarFrameData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, AvatarFrameData>();
        AvatarFrameDataCollection collection = GameDataControl.Instance.Get("all_avatar_frame") as AvatarFrameDataCollection;
        foreach (AvatarFrameData data in collection.listAvatarFrameData)
        {
            dictData.Add(data.avatarFrameName, data);
        }
    }
}