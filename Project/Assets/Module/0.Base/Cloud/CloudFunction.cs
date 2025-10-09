using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public static class CloudFunction
{
    static int timeout = 15;
    public static async UniTask GetCloudFunctionAsync(string url, Action<string> onSuccess, Action onFailure = null, Action onTimeout = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.timeout = timeout; // 设置超时

            try
            {
                MessageManager.Instance.OnLoading();

                webRequest.SetRequestHeader("Authentication", "Bearer " + ZPlayerPrefs.GetString("token"));

                await webRequest.SendWebRequest();

                ResponseWrapper response = JsonConvert.DeserializeObject<ResponseWrapper>(webRequest.downloadHandler.text);

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    //逻辑代号
                    switch (response.logicCode)
                    {
                        case 200:
                            HandleSuccess(onSuccess, response.content);
                            break;
                        default:
                            HandleFailure(onFailure, response.logicCode, response.message);
                            break;
                    }
                }
                else
                {
                    // 处理其他网络错误或协议错误
                    HandleFailure(onFailure, webRequest.responseCode, webRequest.error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout"))
                {
                    HandleTimeout(onTimeout);
                }
                else if (ex.Message.Contains("expired jwt"))
                {
                    HandleFailure(onFailure, 0, ex.ToString());
                    //Jtw验证令牌超时，重新申请令牌，但本次申请会返回错误，需要让玩家重新执行操作
                    //不要重启，玩家离线再回来，令牌过期了，但可以继续玩。这里访问了令牌后重启可能会导致这段进度丢失
                    //TODO优化项，可以回到前台时候先去检查令牌过期没，过期了重新申请，等到海内外服务器合并时再做这个优化
                    TokenManager.Instance.OnTokenExpired();
                }
                else
                {
                    Debug.LogError("=== CloudFunction: an error occurred during UnityWebRequest: " + ex.Message);
                    HandleFailure(onFailure, 0, ex.ToString());
                }
            }
        }
    }

    public static async UniTask PostCloudFunctionAsync(string url, string jsonData, 
        Action<string> onSuccess, Action onConflict = null, Action onFailure = null, Action onTimeout = null,bool needLoading = true)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            try
            {
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json"); // 确保使用正确的内容类型
                webRequest.SetRequestHeader("Authentication", "Bearer " + ZPlayerPrefs.GetString("token"));
                webRequest.timeout = timeout; // 设置超时

                if(needLoading)
                {
                    MessageManager.Instance.OnLoading();
                }

                await webRequest.SendWebRequest();

                ResponseWrapper response = JsonConvert.DeserializeObject<ResponseWrapper>(webRequest.downloadHandler.text);

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    //逻辑代号
                    switch (response.logicCode)
                    {
                        case 200://成功
                            HandleSuccess(onSuccess, response.content);
                            return;
                        case 304://无修改
                            HandleSuccess(onSuccess, response.content);
                            break;
                        case 409://冲突，字典相同key等
                            HandleConflict(onConflict, response.logicCode, response.message);
                            return;
                        default://默认失败
                            HandleFailure(onFailure, response.logicCode, response.message);
                            return;
                    }
                }
                else
                {
                    // 处理其他网络错误或协议错误
                    HandleFailure(onFailure, webRequest.responseCode, webRequest.error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("timeout") || ex.Message.Contains("Timeout"))
                {
                    //访问超时
                    HandleTimeout(onTimeout);
                }
                else if(ex.Message.Contains("expired jwt"))
                {
                    //Jtw验证令牌超时
                    HandleFailure(onFailure, 0, ex.ToString());
                    TokenManager.Instance.OnTokenExpired();
                }
                else
                {
                    Debug.LogError("=== CloudFunction: an error occurred during UnityWebRequest: " + ex.Message);
                    HandleFailure(onFailure, 0, ex.ToString());
                }
            }
        }
    }

    /*
    //发送请求，不需要回调
    public static void PostCloudFunction(string url, string jsonData)
    {
        Debug.Log("=== CloudFunctionHandler: PostCloudFunction ===");
        using (UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(url, jsonData))
        {
            try
            {
                webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json"); // 确保使用正确的内容类型
                webRequest.SetRequestHeader("Authentication", "Bearer " + ZPlayerPrefs.GetString("token"));
                webRequest.timeout = timeout; // 设置超时
                webRequest.SendWebRequest();

            }
            catch (Exception ex)
            {
                Debug.LogError($"=== CloudFunctionHandler: Error: {ex.Message}");
            }
        }
    }*/

    static void HandleSuccess(Action<string> onSuccess, string jsonData)
    {
        onSuccess?.Invoke(jsonData);
        MessageManager.Instance.CloseLoading();
    }

    static void HandleConflict(Action onConflict, long logicCode, string error)
    {
        onConflict?.Invoke();
        Debug.LogWarning($"=== CloudFunctionHandler: Error: {error}, Logic Code: {logicCode}");
        MessageManager.Instance.CloseLoading();
    }

    static void HandleTimeout(Action onTimeout)
    {
        onTimeout?.Invoke();
        Debug.LogWarning("=== CloudFunctionHandler: Request timeout ===");

        MessageManager.Instance.CloseLoading();
    }

    static void HandleFailure(Action onFailure, long logicCode, string error)
    {
        onFailure?.Invoke();
        Debug.LogError($"=== CloudFunctionHandler: Error: {error}, Logic Code: {logicCode}");

        MessageManager.Instance.CloseLoading();
    }

    static void HandleException(Action onFailure, string exMessage)
    {
        onFailure?.Invoke();
        Debug.LogError($"=== CloudFunctionHandler: Exception: {exMessage}");

        MessageManager.Instance.CloseLoading();
    }
}

[Serializable]
public class ResponseWrapper
{
    //public int statusCode;
    //public BodyWarpper body;
    public int logicCode;
    public string content;
    public string message;
}