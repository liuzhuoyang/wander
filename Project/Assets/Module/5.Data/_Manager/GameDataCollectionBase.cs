using UnityEngine;
using Sirenix.OdinInspector;

public class GameDataCollectionBase : ScriptableObject
{
    
    [ReadOnly]
    public string path;

#if UNITY_EDITOR

    public virtual void InitData()
    {
        InitAssetPath();
    }

    //初始化资源路径
    public void InitAssetPath()
    {
        string objectPath = GetObjectPath();
        string assetsPath = GetParentDirectory(GetParentDirectory(objectPath));
        path = assetsPath + "/asset/";
    }

    // 获取当前对象的路径
    public string GetObjectPath()
    {
        return UnityEditor.AssetDatabase.GetAssetPath(this);
    }

    public string GetParentDirectory(string path)
    {
        return System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
    }

    //获取当前脚本所在文件夹的loc路径
    public string GetLocPath()
    {
        return path.Replace("asset", "loc");
    }
    #endif
    
}


/*

    .... 模版参考，方便复制用

   public class AdDataCollection : GameDataCollectionBase
    {
        //资源列表
        [ReadOnly]
        public List<AdData> listAdData;

    #if UNITY_EDITOR

        [Button("Init Data")]
        public override void InitData()
        {
            base.InitData();
            listAdData = AssetsFinder.FindAllAssets<AdData>(path);
        }
    #endif
    }

    public static class AllAd
    {
        public static Dictionary<AdType, AdData> dictData;

        //初始化数据，从资源中加载
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Init()
        {
            dictData = new Dictionary<AdType, AdData>();
            AdDataCollection collection = GameDataControl.Instance.Get("all_ad") as AdDataCollection;
            foreach (AdData adData in collection.listAdData)
            {
                dictData.Add(adData.adType, adData);
            }
        }
    }

*/