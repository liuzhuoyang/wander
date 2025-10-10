using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "all_character", menuName = "OniData/System/Meta/Character/CharacterDataList", order = 1)]
public class CharacterCollection : GameDataCollectionBase
{
    [BoxGroup("info", LabelText = "基础信息")]
    //资源列表
    [ReadOnly]
    public List<CharacterData> listCharacterData;

#if UNITY_EDITOR
    [BoxGroup("Action", LabelText = "初始化")]
    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {
        base.InitData();
        listCharacterData = AssetsFinder.FindAllAssets<CharacterData>(path);
    }
#endif
}

public static class AllCharacter
{
    //数据游戏中使用
    public static Dictionary<string, CharacterData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, CharacterData>();
        CharacterCollection collection = GameDataControl.Instance.Get("all_character") as CharacterCollection;
        foreach (CharacterData data in collection.listCharacterData)
        {
            dictData.Add(data.characterName, data);
        }
    }
}