using UnityEngine;
using System.Collections.Generic;
public class UICache : MonoBehaviour
{
    [SerializeField]
    private int maxSize = 10;
    [SerializeField]
    private Dictionary<string, GameObject> cache;
    [SerializeField]
    private List<string> usageOrder;

    public void Init(int size)
    {
        maxSize = size;
        cache = new Dictionary<string, GameObject>();
        usageOrder = new List<string>();
    }

    public GameObject Get(string key)
    {
        if (cache.TryGetValue(key, out GameObject value))
        {
            // 更新使用顺序
            usageOrder.Remove(key);
            usageOrder.Insert(0, key);
            return value;
        }
        return null;
    }

    public void Add(string key, GameObject value)
    {
        if (cache.ContainsKey(key))
        {
            // 如果已经存在，更新使用顺序
            usageOrder.Remove(key);
        }
        else if (cache.Count >= maxSize)
        {
            // 移除最久未使用的项目
            string oldestKey = usageOrder[usageOrder.Count - 1];
            GameObject oldestObject = cache[oldestKey];
            cache.Remove(oldestKey);
            usageOrder.RemoveAt(usageOrder.Count - 1);
            GameObject.Destroy(oldestObject); // 销毁 GameObject
            Debug.Log($"=== UICache full. Removed and destroyed least recently used item: {oldestKey} ===");
        }

        // 添加新项目并更新使用顺序
        cache[key] = value;
        usageOrder.Insert(0, key);
    }
}