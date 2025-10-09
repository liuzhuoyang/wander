using Newtonsoft.Json;
using System;
using Cysharp.Threading.Tasks;

public class CloudMail : Singleton<CloudMail>
{
    public async UniTask OnGetMail(int userID, string languageType, Action<string> onSuccess)
    {
        var args = new
        {
            action = "Get",
            userID = userID,
            languageType = languageType,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await OnPostFunction(jsonData, onSuccess);
    }

    public async UniTask OnChangeMailStatus(int userID, int mailID, int status, Action<string> onSuccess)
    {
        var args = new
        {
            action = "Change",
            userID = userID,
            mailID = mailID,
            status = status,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await OnPostFunction(jsonData, onSuccess);
    }

    public async UniTask OnDeleteMail(int userID, Action<string> onSuccess)
    {
        var args = new
        {
            action = "Delete",
            userID = userID,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await OnPostFunction(jsonData, onSuccess);
    }

    public async UniTask OnReceiveMail(int userID, int[] mailIDs, Action<string> onSuccess)
    {
        var args = new
        {
            action = "Receive",
            userID = userID,
            mailIDs = mailIDs,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await OnPostFunction(jsonData, onSuccess);
    }

    async UniTask OnPostFunction(string jsonData, Action<string> onSuccess)
    {
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_MAIL), jsonData,
            (result) =>
            {
                onSuccess?.Invoke(result);
            }, null, null, 
            () => {
                // MessageManager.Instance.OnTimeout();
            },false);
    }

}
