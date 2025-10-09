using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllChapter来使用
[Serializable]
[CreateAssetMenu(fileName = "all_game_data", menuName = "OniData/Main/GameDataCollection", order = 1)]
public class GameDataCollection : ScriptableObject
{
    public Dictionary<string, GameDataCollectionBase> dictGameData;

    [BoxGroup("系统数据资源")]
    public List<GameDataCollectionBase> listGameData;
    
    [BoxGroup("通用数据资源")]
    public List<GameDataCollectionBase> listGameDataGeneric;

    [BoxGroup("游戏玩法模块数据资源")]
    public List<GameDataCollectionBase> listGameDataGameplay;

    [BoxGroup("养成模块数据资源")]
    public List<GameDataCollectionBase> listGameDataMeta;

    [BoxGroup("关卡数据资源")]
    public List<GameDataCollectionBase> listGameDataLevel;

    [BoxGroup("目标类型数据资源")]
    public List<GameDataCollectionBase> listGameDataObejctive;

    [BoxGroup("剧情教程数据资源")]
    public List<GameDataCollectionBase> listGameDataPlot;

    [BoxGroup("社交模块数据资源")]
    public List<GameDataCollectionBase> listGameDataSocial;

    [BoxGroup("商业化数据资源")]
    public List<GameDataCollectionBase> listGameDataMonetization;

    [BoxGroup("特效音效数据资源")]
    public List<GameDataCollectionBase> listGameDataFX;

    [BoxGroup("本地化数据资源")]
    public List<GameDataCollectionBase> listGameDataLocalization;

    [BoxGroup("杂项数据资源")]
    public List<GameDataCollectionBase> listGameDataMisc;

    public void Init()
    {
        dictGameData = new Dictionary<string, GameDataCollectionBase>();
        AddGameDataList(listGameData);
        AddGameDataList(listGameDataGeneric);
        AddGameDataList(listGameDataGameplay);
        AddGameDataList(listGameDataMeta);
        AddGameDataList(listGameDataLevel);
        AddGameDataList(listGameDataObejctive);
        AddGameDataList(listGameDataPlot);
        AddGameDataList(listGameDataSocial);
        AddGameDataList(listGameDataMonetization);
        AddGameDataList(listGameDataFX);
        AddGameDataList(listGameDataLocalization);
        AddGameDataList(listGameDataMisc);
    }

    void AddGameDataList(List<GameDataCollectionBase> list)
    {
        if (list != null)
        {
            foreach (GameDataCollectionBase gameData in list)
            {
                dictGameData.TryAdd(gameData.name, gameData);
            }
        }
    }

    #if UNITY_EDITOR
    [BoxGroup("初始化", Order = 0)]
    [InfoBox("可能出现删减资源导致数据组没有更新，开始游戏出现数据错误，这个快捷功能方便意见刷新所有数据组", InfoMessageType.Info)]
    [Button("Init Data 一键刷新所有数据", ButtonSizes.Gigantic) , GUIColor(0.4f, 0.8f, 1f)]
    public void InitData()
    {
        InitGameDataList(listGameData);
        InitGameDataList(listGameDataGeneric);
        InitGameDataList(listGameDataGameplay);
        InitGameDataList(listGameDataMeta);
        InitGameDataList(listGameDataLevel);
        InitGameDataList(listGameDataObejctive);
        InitGameDataList(listGameDataPlot);
        InitGameDataList(listGameDataSocial);
        InitGameDataList(listGameDataMonetization);
        InitGameDataList(listGameDataFX);
        InitGameDataList(listGameDataLocalization);
        InitGameDataList(listGameDataMisc);
    }

    void InitGameDataList(List<GameDataCollectionBase> list)
    {
        foreach (GameDataCollectionBase gameData in list)
        {
            gameData.InitData();
        }
    }
    #endif
}


