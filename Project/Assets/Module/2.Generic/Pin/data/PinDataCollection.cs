using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "all_pin", menuName = "OniData/Generic/Pin/PinDataCollection", order = 1)]
public class PinDataCollection : GameDataCollectionBase
{
    [ReadOnly] public PinData[] pinAssets;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        pinAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<PinData>(path).ToArray();
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}

public static class AllPin
{
    public static Dictionary<string, PinData> dictData { get; private set; }
    //初始化数据，从资源中加载
    public static void Init()
    {
        PinDataCollection collectionData = GameDataControl.Instance.Get("all_pin") as PinDataCollection;

        dictData = new Dictionary<string, PinData>();
        foreach (PinData pinAsset in collectionData.pinAssets)
        {
            dictData.Add(pinAsset.name, pinAsset);
        }
    }
}
