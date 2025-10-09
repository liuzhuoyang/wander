using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//多平台使用抽象类来管理，初始化时候调用平台管理工厂分配相应的类
public static class PlatformControlFactory
{
        /*
        public static TGAnalyticsControl CreateTGAnalyticsControl(GameObject target)
        {
                TGAnalyticsControl analyticsControl = null;
                if (GameConfig.main.productMode == ProductMode.DevOffline)
                {
                        return target.AddComponent<DevTGAnalyticsControl>();
                }
#if UNITY_IOS || UNITY_ANDROID
                analyticsControl = target.AddComponent<ProdTGAnalyticsControl>();
#else
                Debug.LogError("=== PlatformControlFactory: CreateTGAnalyticsControl: no platform ===");        
#endif
                return analyticsControl;
        }*/

        public static IAPControl CreateIAPControl(GameObject target)
        {
                IAPControl iapControl = null;
                if (GameConfig.main.productMode == ProductMode.DevOffline)
                {
                        return target.AddComponent<DevIAPControl>();
                }
#if UNITY_ANDROID && !TGAME_AMAZON
                iapControl = target.AddComponent<AndroidIAPControl>();
#elif UNITY_ANDROID && TGAME_AMAZON
                iapControl = target.AddComponent<AmazonIAPControl>();
#elif UNITY_IOS
                iapControl = target.AddComponent<IOSIAPControl>();
#elif UNITY_WEBGL
                iapControl = target.AddComponent<WebGLIAPControl>();
#else
                Debug.LogError("=== PlatformControlFactory: CreateIAPControl: no platform ===");
#endif
                return iapControl;
        }

        public static AdControl CreateAdsManager(GameObject target)
        {
                AdControl adsManager = null;
                if (GameConfig.main.productMode == ProductMode.DevOffline)
                {
                        return target.AddComponent<AdControlDev>();
                }
#if UNITY_ANDROID && !TGAME_AMAZON
                adsManager = target.AddComponent<AdControlAndroid>();
#elif UNITY_ANDROID && TGAME_AMAZON
                adsManager = target.AddComponent<AmazonAdsManager>();
#elif UNITY_IOS
                adsManager = target.AddComponent<IOSAdsManager>();
#elif UNITY_WEBGL
                adsManager = target.AddComponent<WebGLAdsManager>();
#else
                Debug.LogError("=== PlatformControlFactory: CreateAdsManager: no platform ==="); 
#endif
                return adsManager;
        }
}