using System;
using Newtonsoft.Json;

public class CloudProgress : Singleton<CloudProgress>
{
    public async void OnUploadProgress(Action success, Action onFailure, Action onTimeout)
    {
        var args = new
        {
            action = "Upload",
            udid = GameData.userData.userAccount.udid,
            data = JsonConvert.SerializeObject(GameData.userData),
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_PROGRESS_SYNC), jsonData,
            (result) =>
            {
                success?.Invoke();
            }, () =>
            {
                onFailure?.Invoke();
            }, () =>
            {
                onFailure?.Invoke();
            }, () =>
            {
                onTimeout?.Invoke();
            });
    }

    public async void OnDownloadProgress(string udid, Action<string> onSuccess, Action onFailure, Action onNoData,Action onTimeout)
    {
        var args = new
        {
            action = "Download",
            udid,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_PROGRESS_SYNC), jsonData,
        (result) =>
        {
            onSuccess?.Invoke(result);
        },
        () =>
        {
            onFailure?.Invoke();
        },
        () =>
        {
            onNoData?.Invoke();
        },
        () =>
        {
            onTimeout?.Invoke();
        }
        );
    }

}
