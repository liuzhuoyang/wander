using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

//用于创建配置资源的总体索引，需要与配置资源在同一目录下或其父路径下
public abstract class DataCollection<T> : ScriptableObject where T : ScriptableObject
{

    [SerializeField] protected List<T> DataList;
    public abstract T GetDataByKey(string key);
    public List<T> GetDataCollection() => DataList;

#if UNITY_EDITOR
    [Button("Find All Data")]
    public void FindAllData()
    {
        string path = AssetDatabase.GetAssetPath(this);
        path = Path.GetDirectoryName(path);
        DataList = FileFinder.FindAllAssetsOfAllSubFolders<T>(path);
        EditorUtility.SetDirty(this);
    }
#endif
}