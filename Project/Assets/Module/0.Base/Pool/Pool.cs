using System;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private ObjectPool<GameObject> mPool;

    GameObject poolObject;
    public GameObject PoolObject => poolObject;

    int poolMaxSize;
    public int PoolMaxSize => poolMaxSize;

    Transform poolParent;
    public Transform PoolParent => poolParent;

    public PoolRelease poolRelease;
    Action<GameObject> action;

    public const string POOL_KEYWORD = "n_pool";

    public enum PoolRelease
    {
        Test,
        Nomel,
        AllComp,
        AllChild,
        AllChildAndComp,
        SetParent,
        Other,
    }

    public void PoolInit(GameObject poolObject, int poolMaxSize, Transform poolParent, PoolRelease poolRelease, Action<GameObject> action)
    {
        if (mPool != null)
        {
            mPool.DestroyPool();
        }
        this.poolParent = poolParent;
        this.poolObject = poolObject;
        this.poolMaxSize = poolMaxSize;
        this.poolRelease = poolRelease;
        this.action = action;
        mPool = new ObjectPool<GameObject>(poolMaxSize, OnCreatePoolItem, OnGetPoolItem, OnRelesePoolItem, OnDestroyPoolItem);
    }

    public GameObject Get()
    {
        if (mPool == null)
        {
            mPool = new ObjectPool<GameObject>(poolMaxSize, OnCreatePoolItem, OnGetPoolItem, OnRelesePoolItem, OnDestroyPoolItem);
        }
        GameObject are = mPool.GetObject();
        return are;
    }

    public void Release(GameObject go)
    {
        mPool.RecycleObject(go);
    }

    public void Release(GameObject go, Action<GameObject> action)
    {
        action(go);
        mPool.RecycleObject(go);
    }
    public void ReleaseAll()
    {
        mPool.RecycleAllObjects();
    }


    public void Dispose()
    {
        mPool.DestroyPool();
    }

    void OnDestroyPoolItem(GameObject obj)
    {
        Destroy(obj.gameObject);
    }

    void OnRelesePoolItem(GameObject go)
    {
        switch (poolRelease)
        {

            case PoolRelease.Nomel:
                break;

            case PoolRelease.AllChild:
                foreach (Transform t in go.transform)
                    Destroy(t.gameObject);

                break;

            case PoolRelease.AllComp:
                foreach (Component comp in go.GetComponents<Component>())
                {
                    if (comp is Transform || comp is SpriteRenderer) continue;
                    Destroy(comp);
                }
                break;

            case PoolRelease.AllChildAndComp:
                foreach (Component comp in go.GetComponents<Component>())
                {
                    if (comp is Transform || comp is SpriteRenderer) continue;
                    Destroy(comp);
                }
                foreach (Transform t in go.transform)
                    Destroy(t.gameObject);
                break;

            case PoolRelease.Other:
                if (action != null)
                {
                    action(go);
                }
                return;
            case PoolRelease.SetParent:
                go.transform.SetParent(poolParent);
                break;
        }
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
        go.gameObject.SetActive(false);
        go.name = POOL_KEYWORD;
    }

    void OnGetPoolItem(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    GameObject OnCreatePoolItem()
    {
        var obj = Instantiate(poolObject, Vector2.zero, Quaternion.identity, poolParent);
        return obj;
    }

    public int GetCurrentSize() => mPool.m_currentSize;
    public int GetActiveSize() => mPool.m_activeSize;

    #region 废弃代码
    /*
    /// <summary>
    /// 标准回收，没有额外组件或子物体。[地块,雾气]
    /// </summary>
public void ReleaseAll()
{
    if (mainList == null) return;
    GameObject gameObject;
    for (int i = mainList.Count-1; i >= 0; i--)
    {
        gameObject = mainList[i];
        if (gameObject.activeSelf)
        {
            mPool.Release(gameObject);
        }
    }
    if (mainList.Count == 0) return;
    foreach (GameObject item in mainList)
    {
        if (item.activeSelf)
        {
            mPool.Release(item);
            //Release(item, action);
        }
    }
}
    */

    /// <summary>
    /// 标准回收，带回调
    /// </summary>
    /*
public void ReleaseAll(Action<GameObject> action)
{
    GameObject gameObject;
    for (int i = mainList.Count - 1; i >= 0; i--)
    {
        gameObject = mainList[i];
        if (gameObject.activeSelf)
        {
            action(gameObject);
            mPool.Release(gameObject);
        }
    }
    if (mainList.Count == 0) return;
    foreach (GameObject item in mainList)
    {
        if (item.activeSelf)
        {
            mPool.Release(item);
            action(item);
            //Release(item, action);
        }
    }
}
    */

    /// <summary>
    /// 清理组件回收
    /// </summary>
    /*
public void ReleaseAllContainCom()
{
    GameObject gameObject;
    for (int i = mainList.Count - 1; i >= 0; i--)
    {
        gameObject = mainList[i];
        if (gameObject.activeSelf)
        {
            foreach (Component comp in gameObject.GetComponents<Component>())
            {
                if (comp is Transform || comp is SpriteRenderer) continue;
                Destroy(comp);
            }
            mPool.Release(gameObject);
        }
    }
    if (mainList.Count == 0) return;
    foreach (GameObject item in mainList)
    {
        if (item.activeSelf)
        {
            foreach (Component comp in item.GetComponents<Component>())
            {
                if (comp is Transform || comp is SpriteRenderer) continue;
                Destroy(comp);
            }
            mPool.Release(item);
        }
    }
}
    */

    /// <summary>
    /// 清理子物件回收 [Terrain]
    /// </summary>
    /*
public void ReleaseAllContainChild()
{
    GameObject gameObject;
    for (int i = mainList.Count - 1; i >= 0; i--)
    {
        gameObject = mainList[i];
        if (gameObject.activeSelf)
        {
            foreach (Transform t in gameObject.transform)
                Destroy(t.gameObject);//TODO 也要做成对象池
            mPool.Release(gameObject);
        }
    }
    foreach (GameObject item in mainList)
    {
        if (item.activeSelf)
        {
            foreach (Transform t in item.transform)
                Destroy(t.gameObject);//TODO 也要做成对象池
            mPool.Release(item);
        }
}
    }*/

    /// <summary>
    /// 清理子物件与组件回收 [unit]
    /// </summary>
    /*
public void ReleaseAllContainChildAndComp()
{
    GameObject gameObject;
    for (int i = mainList.Count - 1; i >= 0; i--)
    {
        gameObject = mainList[i];
        if (gameObject.activeSelf)
        {
            foreach (Component comp in gameObject.GetComponents<Component>())
            {
                if (comp is Transform || comp is SpriteRenderer) continue;
                Destroy(comp);
            }
            foreach (Transform t in gameObject.transform)
                Destroy(t.gameObject);//TODO 也要做成对象池
            mPool.Release(gameObject);
        }
    }
    if (mainList.Count == 0) return;
    foreach (GameObject item in mainList)
    {
        if (item.activeSelf)
        {
            foreach (Component comp in item.GetComponents<Component>())
            {
                if (comp is Transform || comp is SpriteRenderer) continue;
                Destroy(comp);
            }
            foreach (Transform t in item.transform)
                Destroy(t.gameObject);//TODO 也要做成对象池
            mPool.Release(item);
        }
    }
}
    */
    #endregion
}

