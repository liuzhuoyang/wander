using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
//using TGame;
using UnityEngine.SceneManagement;

public class SDKControl : Singleton<SDKControl>
{
    
    [SerializeField] GameObject objTGameSDK;
    [SerializeField] GameObject objRankingSDK;

    public void Start()
    {
        //保留单例，且只在preload场景会被创建
        DontDestroyOnLoad(gameObject);

        InitSDK();

        //内购
        PlatformControlFactory.CreateIAPControl(gameObject).Init();
        //广告
        PlatformControlFactory.CreateAdsManager(gameObject).Init();
        //运营打点
        //PlatformControlFactory.CreateTGAnalyticsControl(gameObject).Init();
        //离线开发模式不初始化SDK
        if (GameConfig.main.productMode != ProductMode.DevOffline)
        {
            //初始化分析平台SDK
            GetComponent<AnalyticsControl>().InitSDK();
        }

        SceneManager.LoadScene("main");
    }

    //分平台加载sdk
    public void InitSDK()
    {
        if (GameConfig.main.productMode == ProductMode.DevOffline)
        {
            Destroy(objTGameSDK);
            Destroy(objRankingSDK);
            return;
        }
        /*
#if UNITY_IOS || UNITY_ANDROID
        TGameSDK.Init();
        objRankingSDK.GetComponent<RankingSDK>().Init();
#else

#endif
*/
    }

    /*
    void InitFB()
    {
        if (!FB.IsInitialized)
        {
            // 初始化 Facebook SDK
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    // 在初始化成功后执行的操作
                    Debug.Log(" === SDKControl: Facebook SDK initialized === ");

                    // 关闭自动采集
                    //FB.Mobile.SetAutoLogAppEventsEnabled(false);
                }
                else
                {
                    // 初始化失败时执行的操作
                    Debug.LogError("=== SDKControl: failed to initialize Facebook SDK ===");
                }
            });
        }
        else
        {
            // 如果已经初始化，则直接激活游戏对象
            FB.ActivateApp();
        }
    }

    //登陆Facebook，成功的话，回调带上access token
    public void GetFacebookLoginToken(Action<string> callbackSucceed, Action callbackFailed, Action callbackCancel)
    {
    #if !UNITY_WEBGL
        if (!FB.IsInitialized)
        {
            Debug.LogError("=== SDKControl: Facebook SDK is not initialized ===");
            callbackFailed?.Invoke();
            return;
        }

        if (FB.IsLoggedIn)
            FB.LogOut();

        var permissions = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(permissions, (ILoginResult result) =>
        {
            if (result == null)
            {
                Debug.LogError("=== SDKControl: Facebook login result is null ===");
                callbackFailed?.Invoke();
                return;
            }

            if (result.Error != null)
            {
                Debug.LogError("=== SDKControl: Facebook login error: " + result.Error + " ===");
                callbackFailed?.Invoke();
                return;
            }

            if (result.Cancelled)
            {
                Debug.Log("=== SDKControl: Facebook login cancelled ===");
                callbackCancel?.Invoke();
                return;
            }
            Debug.Log("=== SDKControl: Facebook login result: " + result.RawResult + " ===");

            Debug.Log("=== SDKControl: is fb logged in: " + FB.IsLoggedIn + " ===");
            if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken;
                Debug.Log("=== SDKControl: Facebook login successful, Access Token: " + accessToken.TokenString + " ===");

                callbackSucceed?.Invoke(accessToken.TokenString);
            }
            else
            {
                Debug.Log("=== SDKControl: user cancelled Facebook login ===");
                callbackCancel?.Invoke();
            }
        });
    #endif
    }

    public void LogoutFromFacebook(Action callbackSucceed, Action callbackFailed, Action callbackCancel, Action callbackNotLoggedIn)
    {
    #if !UNITY_WEBGL
        if (FB.IsLoggedIn)
        {
            string userId = AccessToken.CurrentAccessToken.UserId;
            string appAccessToken = AccessToken.CurrentAccessToken.TokenString;
            string unlinkUrl = $"/{userId}/permissions";

            FB.API(unlinkUrl, HttpMethod.DELETE, (result) =>
            {
                if (result == null || string.IsNullOrEmpty(result.RawResult))
                {
                    Debug.LogError("=== PlayfabAuth: Failed to unlink Facebook account. No response from the Graph API.");
                    callbackFailed?.Invoke();
                    return;
                }

                if (result.Error != null)
                {
                    callbackFailed?.Invoke();
                    Debug.LogError("=== PlayfabAuth: Failed to unlink Facebook account. Error: " + result.Error);
                }
                else if (result.Cancelled)
                {
                    callbackCancel?.Invoke();
                    Debug.LogWarning("=== PlayfabAuth: Facebook account unlink cancelled.");
                }
                else
                {
                    Debug.Log("=== PlayfabAuth: Successfully unlinked Facebook account. Response: " + result.RawResult);
                    callbackSucceed?.Invoke();
                    FB.LogOut();
                }
            },
            new Dictionary<string, string>()
            {
                { "access_token", appAccessToken }
            });
        }
        else
        {
            callbackNotLoggedIn?.Invoke();
            Debug.LogWarning("Cannot unlink Facebook account because the user is not logged in.");
        }
        
    #endif
    }*/
}
