using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Cysharp.Threading.Tasks;

//数据编辑器使用，这个scriptableObject包含所有成就数据的List，游戏开始的时候先加载这个资源，后续转换成AllTutorial来使用
[Serializable]
[CreateAssetMenu(fileName = "all_tut", menuName = "OniData/System/Plot/Tutorial/TutDataCollection", order = 1)]
public class TutDataCollection : GameDataCollectionBase
{

    [ReadOnly]
    public List<TutData> listTutData;

    #if UNITY_EDITOR
    [Button("Init Data", ButtonSizes.Gigantic)]
    public override void InitData()
    {
        base.InitData();
        listTutData = AssetsFinder.FindAllAssets<TutData>(path);
    }
    #endif
}

public static class AllTutorial
{
    public static Dictionary<string, TutData> dictData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, TutData>();
        TutDataCollection collection = GameDataControl.Instance.Get("all_tut") as TutDataCollection;
        foreach (TutData data in collection.listTutData)
        {
            dictData.Add(data.name, data);
        }
    }
}