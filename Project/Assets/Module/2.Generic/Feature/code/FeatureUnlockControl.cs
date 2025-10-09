using UnityEngine;
using System.Collections.Generic;

public static class FeatureUnlockControl
{
    public static Dictionary<FeatureType, GameObject> dictStaticLock = new Dictionary<FeatureType, GameObject>();

    // 预先放置的Lock对象要添加进列表，初始化游戏后进行一个统一解锁，因为预先放置时，用户数据还没加载，无法得知解锁情况
    // 一般底部导航栏的按钮都是预先放置Static
    // 创建的页面里的lock都是动态的Dynamic，创建时候自身管理开启关闭即可
    public static void AddStaticLock(FeatureType featureType, GameObject objLock)
    {
        dictStaticLock.TryAdd(featureType, objLock);
    }

    public static void OnUnlockStaticLock(FeatureType featureType)
    {
        //判断一下是否预先解锁
        if (dictStaticLock.ContainsKey(featureType))
        {
            dictStaticLock[featureType].SetActive(false);
        }
    }
}
