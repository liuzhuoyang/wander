using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "all_privilege", menuName = "OniData/System/Monetization/Privilege/PrivilegeDataCollection", order = 1)]
public class PrivilegeDataCollection : GameDataCollectionBase
{
    //单个资源的路径
    //资源列表
    [ReadOnly]
    public List<PrivilegeData> listPrivilegeData;

#if UNITY_EDITOR
    [Button("Init Data")]
    public override void InitData()
    {
        base.InitData();
        listPrivilegeData = AssetsFinder.FindAllAssetsOfAllSubFolders<PrivilegeData>(path);
    }
#endif
}

public static class AllPrivilege
{
    //数据游戏中使用
    public static Dictionary<string, PrivilegeData> dictData;

    //初始化数据，从资源中加载
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        dictData = new Dictionary<string, PrivilegeData>();
        PrivilegeDataCollection collection = GameDataControl.Instance.Get("all_privilege") as PrivilegeDataCollection;
        foreach (PrivilegeData data in collection.listPrivilegeData)
        {
            dictData.Add(data.privilegeName, data);
        }
    }
}
