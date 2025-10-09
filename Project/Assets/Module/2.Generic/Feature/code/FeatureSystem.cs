using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 功能解锁系统
/// </summary>
public class FeatureSystem : Singleton<FeatureSystem>
{
    public void Init()
    {
        //GlobalFeatureData.Init();
    }

    void OnDestroy()
    {
        
    }

    /// <summary>
    /// 展示解锁功能的页面（纯表现内容）
    /// </summary>
    public async void OnDisplayFeatureUnlock(FeatureType unlockFeatureType, Action onComplete)
    {
        await UIMain.Instance.OpenUI("feature", UIPageType.Overlay);
        EventManager.TriggerEvent<UIFeatureArgs>(EventNameFeature.EVENT_FEATURE_UNLOCK_OPEN_UI, new UIFeatureArgs()
        {
            featureType = unlockFeatureType,
            callbackClose = (()=>
            {
                //解锁预先放置的Lock
                FeatureUnlockControl.OnUnlockStaticLock(unlockFeatureType);
                onComplete?.Invoke();
            }),
        });
    }
}
