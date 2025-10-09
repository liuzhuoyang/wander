using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class CloudAccess : Singleton<CloudAccess>
{
    public async UniTask<bool> GetNetTime(Action<long> onSuccess, Action onFailure, Action onTimeout)
    {
        var utcs = new UniTaskCompletionSource<bool>();

        var args = new
        {
            action = "GetTime",
            zone = "Global",
        };

        string jsonData = JsonConvert.SerializeObject(args);

        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCESS), jsonData,
            (result) =>
            {
                utcs.TrySetResult(true);
                NetTimeArgs netTimeArgs = JsonConvert.DeserializeObject<NetTimeArgs>(result);
                long timespan = netTimeArgs.timespan;
                onSuccess?.Invoke(timespan);
                TimeManager.Instance.SetTimespan(timespan);
            },
            () =>
            {
                //utcs.TrySetResult(false);
                onFailure?.Invoke();
                Debug.Log(" === CloudAccess: failed to get network time, offline seconds: " + 0 + " ===");
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onTimeout?.Invoke();
            });
        return await utcs.Task;
    }

    public async UniTask CheckCheatUser()
    {
        var args = new
        {
            action = "CheckCheat",
            serverID = GameData.userData.userServer.serverID,
            userID = GameData.userData.userAccount.userID,
            isDev = GameConfig.main.productMode != ProductMode.Release
        };

        string jsonData = JsonConvert.SerializeObject(args);

        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCESS), jsonData,
            (result) =>
            {
                JObject contentObject = JObject.Parse(result);
                GameData.userData.userTag.isCheatUser = contentObject["isCheat"].Value<bool>();
                if (GameData.userData.userTag.isCheatUser)
                {
                    //RankingSDK.Instance.JoinCheatRanking();
                }
            },
            () =>
            {
            },
            () =>
            {
            },
            () =>
            {
            });
    }
}