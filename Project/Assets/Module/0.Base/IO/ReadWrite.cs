using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.AddressableAssets;
using System.Linq;
using System.Collections.Concurrent;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITE_WEIXIN || UNITY_WEBGL
using WeChatWASM;
#endif

public static class ReadWrite
{
    static string userdataPath = GetPersistentDataPath("user");
    static string userMapDataPath = GetPersistentDataPath("user");

    static string GetPersistentDataPath(string folderName)
    {
#if UNITY_EDITOR             
        return Application.dataPath + "/AddressableLocal/data/" + folderName;
#elif UNITE_WEIXIN || UNITY_WEBGL
        return WX.env.USER_DATA_PATH;
#else
        return Application.persistentDataPath + "/" + folderName;
#endif
    }

    #region 读取模块

    /// <summary>
    /// 读取地图
    /// </summary>
    public static async UniTask<string> ReadDataAsync(string mapName)
    {
        try
        {
            var handle = await GameAsset.GetAssetAsync<TextAsset>(mapName);
            if (handle != null)
            {
                string tmp = handle.text;
                Addressables.Release(handle);
                return tmp;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("=== ReadWrite: Error reading data: " + ex.Message + " ===");
            return null;
        }
    }
    
    /// <summary>
    /// 读取用户游戏数据
    /// </summary>
    public static string ReadUserData()
    {
        return Read(userdataPath + "/userdata.json");
    }

    public static string ReadUserMapData(string mapName)
    {
        return Read(userMapDataPath + "/" + mapName + ".json");
    }

    public static string ReadUserBattleData()
    {
        return Read(userdataPath + "/battledata.json");
    }

    static string Read(string dataPath)
    {

#if UNITY_EDITOR
        if (File.Exists(dataPath))
        {
            string stream = File.ReadAllText(dataPath, System.Text.Encoding.UTF8);
            return stream;
        }
        Debug.Log("=== ReadWrite: not file found in path: " + dataPath + "===");
        return null;
#elif UNITE_WEIXIN || UNITY_WEBGL
        string a = WX.GetFileSystemManager().AccessSync(dataPath);
        if (a != "access:ok")
        {
            return null;
        }

        string stream = WX.GetFileSystemManager().ReadFileSync(dataPath, "utf8");
        return stream;
#else
       if (File.Exists(dataPath))
        {
            string stream = File.ReadAllText(dataPath, System.Text.Encoding.UTF8);
            return stream;
        }
        Debug.Log("=== ReadWrite: not file found in path: " + dataPath + "===");
        return null;
#endif
    }

    /*
    public static string ReadSeasonData(string seasonID)
    {
        string dataPath = Application.dataPath + "/Data/user/season/" + seasonID + ".json";
        return Read(dataPath);
    }

    public static string ReadUserSeasonData()
    {
        string dataPath = Application.dataPath + "/Data" + "/userseason.json";
        return Read(dataPath);
    }
    */
    #endregion

    #region 写入模块
    /// <summary>
    /// 用户存档数据 - 直接写入，不处理队列逻辑
    /// </summary>
    public static void WriteUserdata(UserData data)
    {
        CreateUserFolder(userdataPath);
        string stream = JsonConvert.SerializeObject(data, Formatting.Indented);
        string dataPath = userdataPath + "/userdata.json";
        Write(dataPath, stream);
        Debug.Log("=== ReadWrite: 用户数据保存成功（高优先级写入） ===");
    }
    
    /// <summary>
    /// 异步写入用户数据
    /// </summary>
    public static async UniTask WriteUserdataAsync(UserData data)
    {
        CreateUserFolder(userdataPath);
        string stream = JsonConvert.SerializeObject(data, Formatting.Indented);
        string dataPath = userdataPath + "/userdata.json";
        
        await UniTask.RunOnThreadPool(() => Write(dataPath, stream));
        Debug.Log("=== ReadWrite: 用户数据保存成功（防抖）===");
    }

    /// <summary>
    /// 用户存档数据
    /// </summary>
    public static void WriteUserBattleData(UserBattleData data)
    {
        CreateUserFolder(userdataPath);
        string dataPath = userdataPath + "/battledata.json";
        string stream = JsonConvert.SerializeObject(data, Formatting.Indented);//TODO
        Write(dataPath, stream);
        Debug.Log("=== ReadWrite: write userbattledata IO ===");
    }

    //创建用户数据文件夹
    static void CreateUserFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    static void Write(string dataPath, string stream)
    {
#if UNITY_WEBGL
            WX.GetFileSystemManager().WriteFileSync(dataPath, stream, "utf8");
            return;
#else
        File.WriteAllText(dataPath, stream, System.Text.Encoding.UTF8);
#endif
    }

    #endregion

    #region 删除玩家所有数据
    public static void DeleteUserData()
    {
        //PlayerPrefs.DeleteAll();

#if UNITY_EDITOR
        string folderPath = userdataPath;

        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
        else
        {
            Debug.LogWarning("=== ReadWrite: folder does not exist. ===");
        }
#else
        string path = Application.persistentDataPath + "/user";
        DeleteDirectoryContents(path);
#endif
    }

    static void DeleteDirectoryContents(string targetDirectory)
    {
        string[] files = Directory.GetFiles(targetDirectory);
        string[] dirs = Directory.GetDirectories(targetDirectory);

        foreach (string file in files)
        {
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectoryContents(dir);
            Directory.Delete(dir);
        }
    }
    #endregion
    //清除DLC文件
    static public void ClearAllAddressablesCache()
    {
        string cacheFolderPath = Path.Combine(Application.persistentDataPath, "com.Unity3D.Addressables");

        if (Directory.Exists(cacheFolderPath))
        {
            try
            {
                Directory.Delete(cacheFolderPath, true);
                Debug.Log("=== ReadWrite: addressables cache cleared successfully. ===");
            }
            catch (IOException ex)
            {
                Debug.LogError("=== ReadWrite: failed to clear Addressables cache: " + ex.Message + " ===");
            }
        }
        else
        {
            Debug.Log("=== ReadWrite: Addressables cache folder does not exist. ===");
        }
    }

#if UNITY_EDITOR

    // ************ 编辑器用 *****************

    /// <summary>
    /// 创建地图，并写入模版，同时检查路径及下面文件夹内是否有同名文件
    /// </summary>
    // public static void CreateLevelJsonFile(string fileName, LevelRawData templateData)
    // {
    //     string directoryPath = EditorPathUtility.levelFilePath;
    //     string filePath = Path.Combine(directoryPath, fileName + ".json");

    //     // Ensure the directory exists
    //     if (!Directory.Exists(directoryPath))
    //     {
    //         Directory.CreateDirectory(directoryPath);
    //     }

    //     // Check if the file exists in the directory or any of its subdirectories
    //     if (Directory.GetFiles(directoryPath, fileName + ".json", SearchOption.AllDirectories).Any())
    //     {
    //         Debug.LogError("=== ReadWrite: File already exists at: " + filePath + " ===");
    //         return;
    //     }

    //     // Serialize the template data to JSON
    //     string jsonContent = JsonConvert.SerializeObject(templateData, Formatting.Indented);

    //     // Create the file
    //     File.WriteAllText(filePath, jsonContent);
    //     Debug.Log("=== ReadWrite: File created at: " + filePath + " ===");

    //     // Refresh the AssetDatabase if you're in the Unity Editor

    //     AssetDatabase.Refresh();
    // }

    /// <summary>
    /// 保存当前地图数据
    /// </summary>
    public static void OnWriteMapJsonFile(LevelData mapArgs)
    {
        // MapControl.Instance.LevelData.InitEditorMapData();
        // LevelRawData levelData = MapControl.Instance.LevelData;

        // if (!levelData.ValidateMapData()) return;

        // string directoryPath = EditorPathUtility.levelFilePath + levelData.levelType.ToLower() + "/" + levelData.chapterID.ToString("D3") + "/";
        // string filePath = Path.Combine(directoryPath, mapArgs.levelName + ".json");

        // try
        // {
        //     if (File.Exists(filePath))
        //     {
        //         string jsonContent = JsonConvert.SerializeObject(levelData, Formatting.Indented);
        //         File.WriteAllText(filePath, jsonContent);
        //         Debug.Log("=== ReadWrite: Map file overwritten: " + filePath + " ===");
        //     }
        //     else
        //     {
        //         Debug.LogError("=== ReadWrite: No file found to overwrite: " + filePath + " ===");
        //     }
        // }
        // catch (Exception ex)
        // {
        //     Debug.LogError("=== ReadWrite: Error writing map file: " + ex.Message + " ===");
        // }

        // EditorUtility.DisplayDialog("成功", "地图保存完成", "ok");
    }
#endif
}
