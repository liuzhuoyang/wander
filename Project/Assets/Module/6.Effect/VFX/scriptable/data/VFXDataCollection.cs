using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "all_vfx", menuName = "OniData/FX/VFX/VFXDataCollection")]
public class VFXDataCollection : GameDataCollectionBase
{
    [ReadOnly] public VFXData[] vfxAssets;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        //初始化资源列表,查询整个目录，因为特效资源会放在不同的模块里，当前目录只放共享特效资源
        vfxAssets = AssetsFinder.FindAllAssetsOfAllSubFolders<VFXData>("Assets").ToArray();
        EditorUtility.SetDirty(this);
    }
#endif
}

public static class AllVFX
{
    //数据游戏中使用
    public static Dictionary<string, VFXData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        VFXDataCollection dataCollection = GameDataControl.Instance.Get("all_vfx") as VFXDataCollection;

        dictData = new Dictionary<string, VFXData>();
        foreach (VFXData data in dataCollection.vfxAssets)
        {
            dictData.Add(data.name, data);
        }
    }
}