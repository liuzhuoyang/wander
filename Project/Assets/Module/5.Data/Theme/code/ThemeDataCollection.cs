using UnityEngine;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;


//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllTheme来使用
[Serializable]
[CreateAssetMenu(fileName = "all_theme", menuName = "OniData/Data/Theme/ThemeDataCollection", order = 1)]
public class ThemeDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<ThemeData> listThemeData;

    #if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listThemeData = AssetsFinder.FindAllAssets<ThemeData>(path);
    }
    
    #endif
}

public static class AllTheme
{
     //数据游戏中使用
    public static Dictionary<string, ThemeData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, ThemeData>();
        ThemeDataCollection collection = GameDataControl.Instance.Get("all_theme") as ThemeDataCollection;
        foreach (ThemeData data in collection.listThemeData)
        {
            dictData.Add(data.themeName, data);
        }
    }
}