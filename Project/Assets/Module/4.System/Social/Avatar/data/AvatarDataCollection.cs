
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_avatar", menuName = "OniData/Social/Avatar/AvatarDataCollection", order = 1)]
public class AvatarDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<AvatarData> listAvatarData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listAvatarData = AssetsFinder.FindAllAssetsOfAllSubFolders<AvatarData>(path);
    }
#endif
}

public static class AllAvatar
{
    //数据游戏中使用
    public static Dictionary<string, AvatarData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, AvatarData>();
        AvatarDataCollection avatarDataAll = GameDataControl.Instance.Get("all_avatar") as AvatarDataCollection;
        foreach (AvatarData avatarData in avatarDataAll.listAvatarData)
        {
            dictData.Add(avatarData.avatarName, avatarData);
        }
    }

    public static List<AvatarData> GetPlayerAvatarList()
    {
        return dictData.Values.Where(avatar => avatar.avatarType == AvatarType.Player).ToList();
    }

    public static List<string> GetPlayerAvatarNameList()
    {
        return dictData.Values.Where(avatar => avatar.avatarType == AvatarType.Player).Select(avatar => avatar.avatarName).ToList();
    }
}