using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T> where T : class
{
    public int MaxSize { get; private set; }        // 对象池的最大大小

    private readonly Queue<T> pool;                 // 对象池
    private readonly List<T> activeObjects;         // 启用标记
    private readonly Func<T> createAction;          // 创建对象的方法
    private readonly Action<T> getObjectAction;     // 提取对象的方法
    private readonly Action<T> recycleAction;       // 回收对象的方法
    private readonly Action<T> destroyAction;       // 销毁对象的方法
    private int currentSize;                        // 当前对象池中对象的数量
    public int m_currentSize => currentSize;
    public int m_activeSize => activeObjects.Count;

    // 构造函数，设置最大大小、创建对象的方法、提取对象的方法、回收对象的方法、销毁对象的方法
    public ObjectPool(int maxSize, Func<T> createAction, Action<T> getObjectAction, Action<T> recycleAction, Action<T> destroyAction)
    {
        MaxSize = maxSize;
        this.createAction = createAction;
        this.recycleAction = recycleAction;
        this.destroyAction = destroyAction;
        this.getObjectAction = getObjectAction;
        pool = new Queue<T>();
        activeObjects = new List<T>();
    }

    // 从对象池中获取对象
    public T GetObject()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = CreateObject();
        }
        activeObjects.Add(obj);
        getObjectAction?.Invoke(obj);
        return obj;
    }

    // 回收对象到对象池中，如果对象池已经达到最大容量，则直接销毁对象
    public void RecycleObject(T obj)
    {
        if (activeObjects.Contains(obj))
        {
            activeObjects.Remove(obj);
            recycleAction?.Invoke(obj);
            if (pool.Count < MaxSize)
            {
                pool.Enqueue(obj);
            }
            else
            {
                DestroyObject(obj);
            }
        }
      
    }

    // 回收所有已经启用的对象到对象池中
    public void RecycleAllObjects()
    {
        foreach (T obj in activeObjects)
        {
            if (pool.Count < MaxSize)
            {
                recycleAction?.Invoke(obj);
                pool.Enqueue(obj);
            }
            else
            {
                destroyAction?.Invoke(obj);
            }
        }

        activeObjects.Clear();
    }

    // 创建对象
    private T CreateObject()
    {
        T obj = createAction();
        currentSize++;

        return obj;
    }

    // 销毁对象
    private void DestroyObject(T obj)
    {
        destroyAction?.Invoke(obj);
        currentSize--;
    }

    //销毁池
    public void DestroyPool()
    {
        while (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            destroyAction?.Invoke(obj);
        }
        pool.Clear();
        foreach (T obj in activeObjects)
        {
            destroyAction?.Invoke(obj);
        }
        activeObjects.Clear();
    }

}

