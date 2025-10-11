using UnityEngine;

//单例基类，适用于大多数manager类
//注意：单例不应该频繁被非manager类引用，从而加剧模块之间的耦合
//尽量采用manager与manager之间互相通信，被管理对象通过event或自身所属manager去调用其他模块功能
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance
    {
        get
        {
            //Lazy Initialize
            if (!IsInitialized)
            {
                instance = FindAnyObjectByType<T>();
            }
            return instance;
        }
    }
    public static bool IsInitialized => instance != null;
    private static T instance;
    protected virtual void Awake()
    {
        if (IsInitialized)
        {
            if (instance != ((T)this)) Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}