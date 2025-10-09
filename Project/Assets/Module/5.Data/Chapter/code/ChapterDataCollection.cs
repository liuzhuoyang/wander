using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllChapter来使用
[Serializable]
[CreateAssetMenu(fileName = "all_chapter", menuName = "OniData/Data/Chapter/ChapterDataCollection", order = 1)]
public class ChapterDataCollection : GameDataCollectionBase
{ 
    //资源列表
    [ReadOnly]
    public List<ChapterData> listChapterData;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listChapterData = AssetsFinder.FindAllAssets<ChapterData>(path);
    }
#endif
}

public static class AllChapter
{
    //数据游戏中使用
    public static Dictionary<int, ChapterData> data;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        data = new Dictionary<int, ChapterData>();
        ChapterDataCollection chapterDataCollection = GameDataControl.Instance.Get("all_chapter") as ChapterDataCollection;
        foreach (ChapterData chapterData in chapterDataCollection.listChapterData)
        {
            data.Add(chapterData.chapterID, chapterData);
        }
    }

    public static ChapterData Get(int chapterID)
    {
        return data[chapterID];
    }

    public static bool IsContainChapter(int chapterID)
    {
        return data.ContainsKey(chapterID);
    }

    //获取关卡索引,比如2_1，第二一章有5关，所以关卡index是6
    public static int GetLevelIndex(LevelData levelAssetData, int currentChapterID)
    {
        Dictionary<int, ChapterData> dictChapterData = AllChapter.data;
        int levelIndex = 0;
        foreach (int chapterID in dictChapterData.Keys)
        {
            if (chapterID != currentChapterID)
            {   
                //章节不匹配，直接计算通过
                //当前章节，直接计算
                //第0关没有chapter数据
                levelIndex += dictChapterData[chapterID].totalLevel;
            }
            else
            {
                foreach (LevelData levelAsset in dictChapterData[chapterID].listLevelAssets)
                {
                    //每次轮回添加一次索引
                    levelIndex += 1;
                    if (levelAsset.levelID == levelAssetData.levelID)
                    {
                        //找到当前关卡，直接返回
                        return levelIndex;
                    }
                }
            }
        }
        return levelIndex;
    }
}
