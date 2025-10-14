using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using onicore.editor;
using System.Linq;
#endif

[Serializable]
[CreateAssetMenu(fileName = "all_formatian", menuName = "OniData/Data/Formatian/FormatianDataCollection", order = 1)]
public class FormatianDataCollection : GameDataCollectionBase
{
    [ReadOnly]
    public List<FormatianData> listFormatianData;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listFormatianData = FileFinder.FindAllAssetsOfAllSubFolders<FormatianData>(path);
    }
#endif
}

public static class AllFormatian
{
    //数据游戏中使用
    public static Dictionary<string, FormatianData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, FormatianData>();
        FormatianDataCollection collection = GameDataControl.Instance.Get("all_formatian") as FormatianDataCollection;
        foreach (FormatianData data in collection.listFormatianData)
        {
            dictData.Add(data.formatianName, data);
        }

    }
}