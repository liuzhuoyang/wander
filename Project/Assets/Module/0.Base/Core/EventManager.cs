using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    Dictionary<string, Delegate> eventDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            eventDictionary = new Dictionary<string, Delegate>();
        }
        else
        {
            Destroy(gameObject);
        }

        if (eventDictionary != null)
        {
            // 日志输出即将清除的事件名及其关联的委托
            foreach (var entry in eventDictionary)
            {
                if (entry.Value != null)
                {
                    // 日志输出，显示事件名和委托信息
                    Debug.LogWarning($"=== EventManager: Clearing event: {entry.Key}, with delegates: {entry.Value.GetInvocationList().Length} ===");
                    foreach (Delegate del in entry.Value.GetInvocationList())
                    {
                        Debug.LogWarning($"=== EventManager: Removing delegate: {del.Method.Name} from event: {entry.Key} ===");
                    }
                }
            }

            // 清除所有委托
            foreach (var eventName in new List<string>(eventDictionary.Keys))
            {
                eventDictionary[eventName] = null;
            }
            eventDictionary.Clear();  // 清除字典中所有的项
        }
    }

    public static void StartListening<T>(string eventName, Action<T> listener) where T : EventArgs
    {
        if (Instance == null) return;

        if (!Instance.eventDictionary.ContainsKey(eventName))
        {
            Instance.eventDictionary.Add(eventName, null);
        }

        Instance.eventDictionary[eventName] = Delegate.Combine(Instance.eventDictionary[eventName], listener);
    }

    public static void StopListening<T>(string eventName, Action<T> listener) where T : EventArgs
    {
        if (Instance == null) return;

        if (Instance.eventDictionary.ContainsKey(eventName))
        {
            Instance.eventDictionary[eventName] = Delegate.Remove(Instance.eventDictionary[eventName], listener);
        }
    }

    public static void TriggerEvent<T>(string eventName, T eventParameter) where T : EventArgs
    {
        if (Instance == null || !Instance.eventDictionary.ContainsKey(eventName)) return;

        Delegate eventToTrigger = Instance.eventDictionary[eventName];
        eventToTrigger?.DynamicInvoke(eventParameter);
    }

    internal static void TriggerEvent<T>(string eVENT_ENHANCE_REFRESH_UI, object refreshUI)
    {
        throw new NotImplementedException();
    }

    //注销所有监听器
    public static void StopAllListening()
    {
        if (Instance == null || Instance.eventDictionary == null) return;

        foreach (var eventName in new List<string>(Instance.eventDictionary.Keys))
        {
            if (Instance.eventDictionary[eventName] != null)
            {
                // 日志输出显示每个事件以及移除的监听器
                Debug.LogWarning($"=== EventManager: Removing all listeners for event: {eventName} ===");
                Delegate[] listeners = Instance.eventDictionary[eventName].GetInvocationList();
                foreach (var listener in listeners)
                {
                    Debug.LogWarning($"=== EventManager: Removing listener: {listener.Method.Name} from event: {eventName} ===");
                }
            }

            // 将所有事件的委托置为 null
            Instance.eventDictionary[eventName] = null;
        }

        // 清空字典
        Instance.eventDictionary.Clear();
        Debug.Log("=== EventManager: All listeners have been removed and event dictionary is cleared. ===");
    }

    // 检查是否所有监听器都被注销
    public static bool AreAllListenersCleared()
    {
        if (Instance == null || Instance.eventDictionary.Count == 0)
        {
            Debug.Log("=== EventManager: no listeners or instance is null. ===");
            return true; // Instance is null or no events at all
        }

        foreach (var entry in Instance.eventDictionary)
        {
            if (entry.Value != null)
            {
                Debug.LogWarning($"=== EventManager: Listeners still present for event: {entry.Key} ===");
                return false; // Found at least one listener
            }
        }

        Debug.Log("=== EventManager: all listeners are cleared. ===");
        return true; // No listeners found
    }
}

public abstract class EventArgs
{
    
}
