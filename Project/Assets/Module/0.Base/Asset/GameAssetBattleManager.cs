using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

//动态资源管理器
//这里读取的资源需要释放，比如说战斗预制体
public class GameAssetBattleManager : Singleton<GameAssetBattleManager>
{
    bool isLoading = false;
    public bool IsLoading => isLoading;

    public Dictionary<string, GameObject> battlePrefabDict; //战斗通用预制体，需要动态释放
    public Dictionary<string, AsyncOperationHandle<GameObject>> battlePrefabHandles;

    public void Init()
    {
        battlePrefabDict = new Dictionary<string, GameObject>();
        battlePrefabHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
    }

    #region 对外方法
    //读取战斗资源
    public async UniTask OnLoadBattleAsset()
    {
        //如果战斗资源已经加载，则释放
        //一般情况不会进入这里，在战斗开始时候调用读取，在结束时候应该释放掉
        if(battlePrefabDict.Count > 0)
        {
            OnReleaseBattlePrefabAsset();
            Debug.LogWarning("=== GameAssetsBattleManager: 异常，战斗资源已经加载，释放战斗资源 ===");
        }

        if (isLoading)
        {
            Debug.LogWarning("=== GameAssetsBattleManager: 异常，战斗资源正在加载中，可能存在重复加载的问题 ===");
            return;
        }
        
        isLoading = true;
        await OnLoadBattlePrefabAsset();
        isLoading = false;
    }
    
    //获取战斗资源
    public GameObject GetBattlePrefab(string prefabName)
    {
        return battlePrefabDict[prefabName];
    }

    //释放战斗资源
    public void OnReleaseBattlePrefabAsset()
    {
        // 转换为数组，避免修改集合问题
        var handles = battlePrefabHandles.Values.ToArray();
        
        for (int i = 0; i < handles.Length; i++)
        {
            var handle = handles[i];
            if (handle.IsValid())
            {
                Debug.Log("=== GameAssetsBattleManager: 释放战斗资源 ===" + handle.Result.name);
                Addressables.Release(handle);
            }
        }
        
        // 2. 清空字典
        battlePrefabHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
        battlePrefabDict = new Dictionary<string, GameObject>();

        // 3. 强制垃圾回收
        Resources.UnloadUnusedAssets();
    }
    #endregion

    #region 内部方法
    async UniTask OnLoadBattlePrefabAsset()
    {
        Debug.Log("=== GameAssetsBattleManager: 加载战斗资源 ===");

        battlePrefabDict = new Dictionary<string, GameObject>();
        battlePrefabHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

        //预加载武器预制体
        List<string> prefabNameList = new List<string>();
    
        //TODO这里需要获取本关的武器预制体列表
        prefabNameList.Add("ui_acting"); //测试
        prefabNameList.Add("ui_transit"); //测试

        await LoadAssets(prefabNameList, LoadGearPrefab);
    }

    //加载战斗资源
    async UniTask LoadGearPrefab(string gearPrefabName)
    {
        Debug.Log("=== GameAssetsBattleManager: 加载战斗资源 ===" + gearPrefabName);

        // 使用Addressables直接加载，保存Handle
        var handle = Addressables.LoadAssetAsync<GameObject>(gearPrefabName);
        await handle.Task;
        
        battlePrefabHandles[gearPrefabName] = handle;
        battlePrefabDict[gearPrefabName] = handle.Result;
    }

    /// <summary>
    /// 批量异步加载资源
    /// 将大量资源分批处理，避免一次性加载造成内存压力和性能问题
    /// </summary>
    /// <typeparam name="T">资源类型（如string类型的资源名称）</typeparam>
    /// <param name="items">需要加载的资源列表</param>
    /// <param name="loadFunc">单个资源的加载函数</param>
    /// <param name="batchSize">每批处理的资源数量，默认50个</param>
    /// <returns>异步任务</returns>
    async UniTask LoadAssets<T>(IEnumerable<T> items, Func<T, UniTask> loadFunc, int batchSize = 50)
    {
        // 转换为List，避免多次枚举
        var itemList = items.ToList();
        
        // 分批处理：每次跳过已处理的数量，取下一批
        // 例如：batchSize=50时，i=0,50,100,150...
        for (int i = 0; i < itemList.Count; i += batchSize)
        {
            // 获取当前批次的资源：跳过前i个，取batchSize个
            // 如果剩余不足batchSize，则取完所有剩余的
            var batch = itemList.Skip(i).Take(batchSize);
            
            // 为当前批次的每个资源创建加载任务
            var batchTasks = batch.Select(loadFunc);
            
            // 等待当前批次的所有任务完成后再处理下一批
            // 这样可以控制内存使用和避免创建过多并发任务
            await UniTask.WhenAll(batchTasks);
        }
    }
    #endregion
}
