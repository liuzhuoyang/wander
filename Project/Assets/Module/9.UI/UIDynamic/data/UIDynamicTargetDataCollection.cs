using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu(fileName = "all_ui_dynamic_target", menuName = "OniData/UI/UIDynamicTargetDataCollection", order = 1)]
public class UIDynamicTargetDataCollection : GameDataCollectionBase
{
    public List<UIDynamicTargetData> listUIDynamicTargetData;

    //资源列表
    [ReadOnly]
    public List<UIDynamicTargetData> listData;
    
#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listData = new List<UIDynamicTargetData>();
        List<UIDynamicTargetData> list = FileFinder.FindAllAssetsOfAllSubFolders<UIDynamicTargetData>(path);
        foreach (UIDynamicTargetData data in list)
        {
            listData.Add(data);
        }
    }
#endif
}

public static class AllUIDynamicTarget
{
    public static Dictionary<string, UIDynamicTargetData> dictData;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, UIDynamicTargetData>();
        UIDynamicTargetDataCollection collectionData = GameDataControl.Instance.Get("all_ui_dynamic_target") as UIDynamicTargetDataCollection;
        foreach (UIDynamicTargetData data in collectionData.listData)
        {
            dictData.Add(data.targetName, data);
        }
    }
}
