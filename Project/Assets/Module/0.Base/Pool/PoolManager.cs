using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string, Pool> poolGroup;
    public void Init()
    {
        poolGroup = new Dictionary<string, Pool>();
    }

    public GameObject GetObject(string poolName,GameObject prefabObject)
    {
        if (!poolGroup.ContainsKey(poolName))
        {
            CreatePoolObject(poolName, prefabObject);
        }
        return poolGroup[poolName].Get();
    }

    public GameObject GetObject(string poolName, GameObject prefabObject,  Transform parentGroup = null, bool isUISpace = false, int maxNum = 200, Pool.PoolRelease poolRelease=Pool.PoolRelease.Nomel, Action<GameObject> action=null)
    {
        if (!poolGroup.ContainsKey(poolName))
        {
            CreatePoolObject(poolName, prefabObject, parentGroup, isUISpace, maxNum, poolRelease,action);
        }
        return poolGroup[poolName].Get();
    }

    public int ShowPoolCount(string poolName)
    {
        if (poolGroup.ContainsKey(poolName))
        {
            return poolGroup[poolName].GetCurrentSize();
        }
        return 0;
    }
    public int ShowPoolActiveCount(string poolName)
    {
        if (poolGroup.ContainsKey(poolName))
        {
            return poolGroup[poolName].GetActiveSize();
        }
        return 0;        
    }

    /// <summary>
    /// 标准回收，没有额外组件或子物体。[地块,雾气]
    /// </summary>
    public void Release(string poolName, GameObject go)
    {
        if (poolGroup.ContainsKey(poolName))
            poolGroup[poolName].Release(go);
    }
    /// <summary>
    /// 移除物体池。
    /// </summary>
    public void Dispose(string poolName)
    {
        if(poolGroup.ContainsKey(poolName))
        {
            var pool = poolGroup[poolName];
            pool.Dispose();
            Destroy(pool.gameObject);
            poolGroup.Remove(poolName);
        }
    }

    public void ReleaseAll(string poolName)
    {
        if (poolGroup.ContainsKey(poolName))
            poolGroup[poolName].ReleaseAll();
    }
 
    /// <summary>
    /// UI层创建需要传入parentGroup, 否则在世界坐标与ui坐标对不上
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="prefabObject"></param>
    /// <param name="parentGroup"></param>
    void CreatePoolObject(string poolName, GameObject prefabObject, Transform parentGroup = null, bool isUISpace = false,int maxNum=200,Pool.PoolRelease poolRelease=Pool.PoolRelease.Nomel,Action<GameObject> action=null)
    {
        GameObject poolGroup = new GameObject(poolName);
        if (parentGroup == null)
            parentGroup = this.transform;
        poolGroup.transform.SetParent(parentGroup);
        if (isUISpace)
        {
            poolGroup.AddComponent<RectTransform>();
            poolGroup.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        poolGroup.transform.localScale = Vector3.one;
        Pool poolObject = poolGroup.gameObject.AddComponent<Pool>();
        poolObject.PoolInit(prefabObject,maxNum, poolGroup.transform, poolRelease,action);
        this.poolGroup.Add(poolName, poolObject);
    }

    private void OnDestroy()
    {
        foreach (var item in poolGroup)
        {
            item.Value.Dispose();
        }
    }

}







