using UnityEngine;
using System;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public class TokenManager:Singleton<TokenManager>
{
    public async UniTask OnRefreshToken(Action onSuccess, Action onFailure, Action onTimeout)
    {
        Debug.Log("=== TokenManager:  requesting a new token ===");
        await CloudFunction.GetCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_TOKEN),
           (result) =>
           {
               Debug.Log("=== TokenManager: new token obtained ===");
               TokenArgs tokenArgs = JsonConvert.DeserializeObject<TokenArgs>(result);
               ZPlayerPrefs.SetString("token", tokenArgs.token);
               onSuccess?.Invoke();
           },
           () =>
           {
               Debug.Log("=== TokenManager: fail to get new token ===");
               onFailure?.Invoke();
           },
           () =>
           {
               onTimeout?.Invoke();
           });
    }

    public async void OnTokenExpired()
    {
        Debug.Log("=== TokenManager: token expired, requesting a new one ===");
        //TODO 提示码过期，需要重新获取码
        await OnRefreshToken(
            () =>
            {
                
            },
            () =>
            {
                // MessageManager.Instance.OnTimeout();
            },
            () =>
            {
                // MessageManager.Instance.OnTimeout();
            }
        );
    }

    public void Init()
    {
        ZPlayerPrefs.Initialize("Aa123456@", "salt12issalt");
    }
}

public class TokenArgs
{
    public string token;
}
