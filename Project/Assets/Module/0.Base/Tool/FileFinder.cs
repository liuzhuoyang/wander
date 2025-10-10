#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

// 编辑器用与查找资源的工具类
public static class FileFinder
{
    public static Sprite FindSprite(string folderPath, string fileName)
    {
        string path = folderPath + fileName + ".png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        return sprite;
    }

    /// <summary>
    /// 查找指定文件夹中所有Sprite类型的资源
    /// </summary>
    /// <param name="folderPath">文件夹路径</param>
    /// <returns>返回所有Sprite类型的资源列表</returns>
    public static List<Sprite> FindAllSprites(string folderPath)
    {
        List<Sprite> sprites = new List<Sprite>();

        string[] fileEntries = Directory.GetFiles(folderPath);

        foreach (string filePath in fileEntries)
        {
            if (filePath.EndsWith(".png"))
            {
                string relativePath = filePath.Replace(Application.dataPath, "").Replace('\\', '/');
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
                if (sprite != null)
                {
                    sprites.Add(sprite);
                }
            }
        }
        return sprites;
    }

    public static List<Object> FindAllObjects(string folderPath)
    {
        List<Object> listObj = new List<Object>();

        string[] fileEntries = Directory.GetFiles(folderPath);

        foreach (string filePath in fileEntries)
        {
            string relativePath = filePath.Replace(Application.dataPath, "").Replace('\\', '/');
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(relativePath);
            if (obj != null)
            {
                listObj.Add(obj);
            }
        }
        return listObj;
    }

    #region 得到目标路径的所有除开.meta后缀的文件
    /// <summary>
    /// 获取目标路径及其所有子文件夹中的文件名称（不含扩展名）
    /// </summary>
    /// <param name="folderPath">要搜索的根目录路径</param>
    /// <returns>返回所有非.meta文件的文件名列表（不带扩展名）</returns>
    public static List<string> FindAllFilesOfAllSubFolders(string folderPath)
    {
        List<string> foundFiles = new List<string>();

        // 检查文件夹是否存在
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("=== AssetsFinder: Folder not found: " + folderPath + " ===");
            return foundFiles;
        }

        // 添加当前目录中的文件
        AddFilesFromFolder(folderPath, foundFiles);

        // 递归处理所有子目录（包括嵌套的子目录）
        string[] subfolders = Directory.GetDirectories(folderPath);
        foreach (string subfolder in subfolders)
        {
            // 递归调用自身处理子目录
            List<string> subFiles = FindAllFilesOfAllSubFolders(subfolder);
            foundFiles.AddRange(subFiles);
        }

        return foundFiles;
    }

    private static void AddFilesFromFolder(string folderPath, List<string> fileList)
    {
        string[] fileEntries = Directory.GetFiles(folderPath);
        foreach (string fileName in fileEntries)
        {
            if (!fileName.EndsWith(".meta"))
            {
                string justFileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                fileList.Add(justFileNameWithoutExtension);
                Debug.Log("=== AssetsFinder: Found file: " + justFileNameWithoutExtension + " ===");
            }
        }
    }
    #endregion

    #region ScriptableObject
    /// <summary>
    /// 获取指定文件夹中所有ScriptableObject类型的资源
    /// </summary>
    /// <typeparam name="T">ScriptableObject类型</typeparam>
    /// <param name="folderPath">文件夹路径</param>
    /// <returns>返回所有ScriptableObject类型的资源列表</returns>

    public static List<T> FindAllAssets<T>(string folderPath) where T : ScriptableObject
    {
        List<T> foundAssets = new List<T>();

        // Check if the folder exists
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Folder not found: " + folderPath);
            return foundAssets;
        }

        string[] fileEntries = Directory.GetFiles(folderPath);
        foreach (string fileName in fileEntries)
        {
            if (fileName.EndsWith(".asset"))
            {
                string assetPath = fileName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (obj != null)
                {
                    foundAssets.Add(obj);
                    Debug.Log("Found asset: " + obj.name);
                }
            }
        }

        return foundAssets;
    }

    #region 获得目录以及目录下所有子文件夹的资源列表
    /// <summary>
    /// 获得目录以及目录下所有子文件夹的资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="folderPath"></param>
    /// <returns></returns>
    public static List<T> FindAllAssetsOfAllSubFolders<T>(string folderPath, List<string> excludedFolders = null) where T : ScriptableObject
    {
        List<T> foundAssets = new List<T>();
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("文件夹未找到: " + folderPath);
            return foundAssets;
        }

        // 包含根目录和子文件夹中的文件
        GetAssetFromTheFolder(folderPath, foundAssets);

        // 使用 SearchOption.AllDirectories 来包含所有子文件夹
        string[] subfolders = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);
        foreach (var subfolder in subfolders)
        {
            // 如果子文件夹在排除列表中，跳过
            if (excludedFolders != null && excludedFolders.Exists(excluded => subfolder.Contains(excluded)))
            {
                continue;
            }

            GetAssetFromTheFolder(subfolder, foundAssets);
        }

        return foundAssets;
    }


    private static void GetAssetFromTheFolder<T>(string folderPath, List<T> foundAssets) where T : ScriptableObject
    {
        foreach (var file in Directory.GetFiles(folderPath, "*.asset"))
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(file.Replace("\\", "/").Replace(Application.dataPath, "Assets"));
            if (asset != null) foundAssets.Add(asset);
        }
    }
    #endregion

    /// <summary>
    /// 根据文件夹路径和资源名称查找资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="folderPath">文件夹路径</param>
    /// <param name="assetName">资源名称</param>
    /// <returns>返回找到的资源，如果未找到则返回null</returns>
    public static T FindAssetByName<T>(string folderPath, string assetName) where T : ScriptableObject
    {
        // Check if the folder exists
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Folder not found: " + folderPath);
            return null;
        }

        return SearchInDirectory<T>(folderPath, assetName);
    }

    /// <summary>
    /// 在指定目录中查找资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="directoryPath">目录路径</param>
    /// <param name="assetName">资源名称</param>
    /// <returns>返回找到的资源，如果未找到则返回null</returns>
    private static T SearchInDirectory<T>(string directoryPath, string assetName) where T : ScriptableObject
    {
        string[] fileEntries = Directory.GetFiles(directoryPath);
        foreach (string fileName in fileEntries)
        {
            if (fileName.EndsWith(".asset"))
            {
                string assetPath = fileName.Replace("\\", "/").Replace(Application.dataPath, "Assets");
                T obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (obj != null && obj.name == assetName)
                {
                    return obj; // Return the asset if it matches the name
                }
            }
        }

        // Recursively search in subdirectories
        string[] subdirectoryEntries = Directory.GetDirectories(directoryPath);
        foreach (string subdirectory in subdirectoryEntries)
        {
            T foundAsset = SearchInDirectory<T>(subdirectory, assetName);
            if (foundAsset != null)
            {
                return foundAsset; // Return the found asset from subdirectories
            }
        }

        //Debug.Log("asset not found: " + directoryPath);
        return null; // No matching asset found
    }

    #endregion

    public static List<string> GetSubFolders(string folderPath)
    {
        List<string> subFolderNames = new List<string>();

        // Check if the folder exists
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Folder not found: " + folderPath);
            return subFolderNames;
        }

        string[] subDirectories = Directory.GetDirectories(folderPath);

        foreach (string subDirectory in subDirectories)
        {
            string folderName = Path.GetFileName(subDirectory);
            subFolderNames.Add(folderName);
        }

        return subFolderNames;
    }

    #region 检查目录是否存在，如果不存在，创建一个
    public static void CreateFolder(string directoryPath)
    {
        // Check if the directory exists
        if (!Directory.Exists(directoryPath))
        {
            // If it doesn't exist, create it
            Directory.CreateDirectory(directoryPath);
        }
    }
    #endregion
}
#endif