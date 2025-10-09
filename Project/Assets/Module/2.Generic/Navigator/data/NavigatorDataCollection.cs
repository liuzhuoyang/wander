using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "all_navigator", menuName = "OniData/Generic/Navigator/NavigatorDataCollection", order = 1)]
public class NavigatorDataCollection : GameDataCollectionBase
{
    //资源列表
    [ReadOnly]
    public List<NavigatorData> listItemData;

#if UNITY_EDITOR

    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listItemData = AssetsFinder.FindAllAssetsOfAllSubFolders<NavigatorData>(path);
    }
#endif
}

public static class AllNavigator
{
    public static Dictionary<string, NavigatorData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, NavigatorData>();
        NavigatorDataCollection collection = GameDataControl.Instance.Get("all_navigator") as NavigatorDataCollection;
        foreach (NavigatorData data in collection.listItemData)
        {
            AllNavigator.dictData.Add(data.navigatorName, data);
        }
    }
}
