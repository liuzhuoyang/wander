using UnityEngine;
using System;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public class CloudVersion : Singleton<CloudVersion>
{
    //检查版本。版本格式1.1.0
    //这里需要返回两个判断
    //一个是当前版本是否满足强制更新版本 - 如果不是，则弹窗强制更新
    //一个是当前版本是否最新版本 - 如果不是，还可以进入游戏，但再游戏中会出现更新按钮
    public async void OnCheckLatestVersion(Action onPass, Action onUpdateNeeded, Action onNetworkError, Action onTimeout)
    {
        string checkPlatform = "ios";
#if PLATFORM_ANDROID
        checkPlatform = "android";
#endif
        var args = new
        {
            action = "CheckVersion",
            platform = checkPlatform,
            version = VersionManager.Instance.checkVersionArgs.version,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);

        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_VERSION), jsonData,
           (result) =>
           {
               CheckVersionArgs args = JsonConvert.DeserializeObject<CheckVersionArgs>(result);
               VersionManager.Instance.OnCheckServerVersion(args,onUpdateNeeded,onPass);
           },
           () =>
           {
               onNetworkError?.Invoke();
               //联网错误或后台配置key找不到
               Debug.Log("=== ConfigManager: no key found or network error ===");
           },
           () =>
           {
               onTimeout?.Invoke();
           }
           );
    }

    /// <summary>
    /// 来源不明的用户，强制进行版本校验，校验不通过无法进入游戏
    /// 后台有一个True * False的开关，True表示允许非正常来源玩家进入游戏，False表示不允许。
    /// </summary>
    /// <param name="callbackPass"></param>
    /// <param name="callbackUpdateNeeded"></param>
    /// <param name="callbackNetworkError"></param>
    public async UniTask OnHandleUnknownUserVersion(Action onPass, Action onBlock, Action onTimeout)
    {
        Debug.Log("=== ConfigManager:  unkown user version check ===");

        var args = new
        {
            action = "CheckUnknownUser"
        };
        string jsonData = JsonConvert.SerializeObject(args);

        string url = CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_VERSION);
        await CloudFunction.PostCloudFunctionAsync(url, jsonData,
          (result) =>
          {
              try
              {
                  //检查后台是否允许非正常来源玩家进入游戏
                  bool isPass = bool.Parse(result);
                  if (isPass)
                  {
                      //通过验证
                      onPass?.Invoke();
                  }
                  else
                  {
                      //没有通过验证
                      onBlock?.Invoke();
                  }
              }
              catch (Exception ex)
              {
                  //因参数错误解析失败
                  onBlock?.Invoke();
                  Debug.LogError($"=== CloudVersion: OnHandleUnknownUserVersion error: {ex.Message} ===");
              }
          },
          () =>
          {

          },
          () =>
          {
              onTimeout?.Invoke();
          }
          );
    }

    /*
    public struct userAttributes
    {
        
    }

    public struct appAttributes
    {
        // Optionally declare variables for any custom app attributes:
        public string latestAndroidVersion;
    }

    // Declare any Settings variables you’ll want to configure remotely:
    public string latestAndroidVersion;

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.Log("=== VersionCheckManager(RemotConfig): no settings loaded this session; using default values. ===");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("=== VersionCheckManager(RemotConfig): no settings loaded this session; using cached values from a previous session. ===");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("=== VersionCheckManager(RemotConfig): new settings loaded this session; update values accordingly. ===");
                latestAndroidVersion = RemoteConfigService.Instance.appConfig.GetString("LatestAndroidVersion");
                //assignmentId = RemoteConfigService.Instance.appConfig.assignmentId;
                break;
        }
    }*/
}