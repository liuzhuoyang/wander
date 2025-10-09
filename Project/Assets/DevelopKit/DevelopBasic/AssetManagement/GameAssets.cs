using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections;
using Cysharp.Threading.Tasks;

using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class GameAssets
{
    public static string ICON_TAG = "icon_";
    public static T GetAssetsEditor<T>(string prefabName)
    {
        AsyncOperationHandle<T> obj = Addressables.LoadAssetAsync<T>(prefabName);
        obj.WaitForCompletion();
        return obj.Result;
    }

    static Dictionary<string, AsyncOperationHandle> spriteHandleDict = new Dictionary<string, AsyncOperationHandle>();
    static Dictionary<string, AsyncOperationHandle> prefabHandleDict = new Dictionary<string, AsyncOperationHandle>();
    static Dictionary<string, AsyncOperationHandle> audioHandleDict = new Dictionary<string, AsyncOperationHandle>();

    /// <summary>
    /// 检查是否存在
    /// </summary>
    /// <param name="assetAddress"></param>
    /// <returns></returns>
    public static bool CheckAddressableAssetExists(object key)
    {
        Debug.Log("check is container assets: " + key);
        foreach (IResourceLocator locator in Addressables.ResourceLocators)
        {
            IList<IResourceLocation> locations;
            if (locator.Locate(key, typeof(UnityEngine.Object), out locations))
            {
                Debug.Log("asset exist: " + key);
                return true;
            }
        }

        Debug.Log("asset not exist: " + key);
        return false;
    }

    /// <summary>
    /// 只有被加入Handle管理的才会被清除
    /// </summary>
    public static void ReleaseAllAssets()
    {
        Debug.Log("=== start releasing assets ===");

        ReleaseDictionaryAssets(spriteHandleDict);
        ReleaseDictionaryAssets(prefabHandleDict);
        ReleaseDictionaryAssets(audioHandleDict);

        Debug.Log("=== done releasing assets, total : " + (spriteHandleDict.Count + prefabHandleDict.Count) + "===");
    }

    private static void ReleaseDictionaryAssets(Dictionary<string, AsyncOperationHandle> handleDict)
    {
        if (handleDict != null)
        {
            foreach (var handle in handleDict.Values)
            {
                if (handle.IsValid())
                {
                    Debug.Log("release " + handle.DebugName);
                    Addressables.Release(handle);
                }
            }
            Debug.Log("=== done releasing assets: " + handleDict.Count + "===");
            handleDict.Clear();
        }
    }
    //Only Access for Manager Class
    public static async UniTask LoadAssets<T>(IEnumerable<T> items, Func<T, UniTask> loadFunc, int batchSize = 50)
    {
        List<UniTask> batchTasks = new List<UniTask>(batchSize);

        foreach (var item in items)
        {
            batchTasks.Add(loadFunc(item));

            if (batchTasks.Count >= batchSize)
            {
                await UniTask.WhenAll(batchTasks);
                batchTasks.Clear();
            }
        }

        if (batchTasks.Count > 0)
        {
            await UniTask.WhenAll(batchTasks);
        }
    }
    public static async UniTask<GameObject> GetPrefabAsync(string prefabName)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabName);
        await handle.Task;
        if (!prefabHandleDict.ContainsKey(prefabName))
            prefabHandleDict.Add(prefabName, handle);
        return handle.Result;
    }
    public static async UniTask<T> GetScriptablesAsync<T>(string scriptableName) where T : ScriptableObject
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(scriptableName);
        await handle.Task;
        if (!prefabHandleDict.ContainsKey(scriptableName))
            prefabHandleDict.Add(scriptableName, handle);
        return handle.Result;
    }

    public static IEnumerator IEGetAsset<T>(string prefabName, Action<T> onAssetLoaded)
    {
        AsyncOperationHandle<T> obj = Addressables.LoadAssetAsync<T>(prefabName);
        yield return obj;

        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            onAssetLoaded?.Invoke(obj.Result);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + prefabName);
        }
    }
    //用于BGM/Ambience类型的音乐
    public static async UniTask<AudioClip> GetAudioAsync(string assetName)
    {
        AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(assetName);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (!audioHandleDict.ContainsKey(assetName))
                audioHandleDict.Add(assetName, handle);
        }
        else
        {
            Debug.LogError($"=== GameAssets: failed to load bgm: {assetName} ===");
        }

        return handle.Result;
    }
    public static async UniTask<AudioClip> GetAudioAsync(AssetReference assetReference)
    {
        AsyncOperationHandle<AudioClip> handle = assetReference.LoadAssetAsync<AudioClip>();
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (!audioHandleDict.ContainsKey(assetReference.RuntimeKey.ToString()))
                audioHandleDict.Add(assetReference.RuntimeKey.ToString(), handle);
        }
        else
        {
            Debug.LogError($"=== GameAssets: failed to load bgm: {assetReference.RuntimeKey} ===");
        }

        return handle.Result;
    }

    public static async UniTask<Sprite> GetSpriteAsync(string assetName)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(assetName);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (!spriteHandleDict.ContainsKey(assetName))
                spriteHandleDict.Add(assetName, handle);
        }
        else
        {
            Debug.LogError($"Failed to load sprite: {assetName}");
        }

        return handle.Result;
    }

    /// <summary>
    /// 只允许GameAssetsManager使用，读取后存放在全局，不释放
    /// </summary>
    public static async UniTask<T> GetAssetAsync<T>(string assetName)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetName);
        await handle.Task;
        return handle.Result;
    }

    public static async UniTask<T> GetAssetAsync<T>(AssetReference assetReference)
    {
        AsyncOperationHandle<T> handle = assetReference.LoadAssetAsync<T>();
        await handle.Task;
        return handle.Result;
    }
}
