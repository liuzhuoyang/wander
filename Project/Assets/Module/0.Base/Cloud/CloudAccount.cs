using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CloudAccount : Singleton<CloudAccount>
{
    //onSuccess方法要传出是否新注册的用户，因为AnalyticsControl.cs中AMP的打点Register需要判断是否是新注册用户
    public void Init(bool isUnknownUser, Action<bool> onSuccess, Action onFailure, Action onTimeout)
    {
        if (string.IsNullOrEmpty(GameData.userData.userAccount.udid))
        {
            Debug.Log("=== CloudAuth: udid not exists, this is a new user, start registing new account ===");
            OnRegisterNewAccount(isUnknownUser, onSuccess, onFailure, onTimeout);

            // 注册成功后，触发注册事件
            AnalyticsControl.Instance.OnRegister();
        }
        else
        {
            Debug.Log($"=== CloudAuth: udid exists, udid {GameData.userData.userAccount.udid} ===");
            //连接成功，用户数据里已经有UDID，所以不是新注册用户，回调传入false
            onSuccess?.Invoke(false);
        }
    }

    // 注册新账号
    public async void OnRegisterNewAccount(bool isUnknownUser, Action<bool> onSuccess, Action onFailure, Action onTimeout)
    {
        var args = new
        {
            action = isUnknownUser ? "RegisterUnknownUser" : "Register",
            openID = "",
            isDev = GameConfig.main.productMode != ProductMode.Release
        };

        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCOUNT), jsonData,
            (result) =>
            {
                JObject contentObject = JObject.Parse(result);
                GameData.userData.userAccount.udid = contentObject["udid"].Value<string>();
                GameData.userData.userAccount.userID = contentObject["userID"].Value<int>();
                GameData.userData.userServer.serverRegisterTime = contentObject["serverRegisterTime"].Value<long>();
                GameData.userData.userServer.serverID = contentObject["serverID"].Value<int>();
                GameData.userData.userServer.serverRandomSeed = contentObject["serverRandomSeed"].Value<int>();
                GameData.userData.userServer.initialUserID = contentObject["initialUserID"].Value<int>();
                //RankingSDK.Instance.SetUserID(GameData.userData.userAccount.userID);
                GameData.userData.userAnalytics.groupABType = UnityEngine.Random.Range(0, 100) < 50 ? GroupABType.A : GroupABType.B;
                //TGameSDK.GetInstance().UCustomAttr("lvABtest", GameData.userData.userAnalytics.groupABType == GroupABType.A ? "A" : "B");

                Debug.Log($"=== CloudAuth: successfully set user account udid {GameData.userData.userAccount.udid}");

                // 将时间戳转换为可读的日期格式
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.registerTime);
                string registerTimeDisplay = dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss");

                Debug.Log($"=== CloudAuth: sucessfully register a user. result: {result} ===");
                EventManager.TriggerEvent<ActionArgs>(EventNameAction.EVENT_ON_ACTION, new ActionArgs()
                {
                    action = ActionType.Register
                });

                // 注册成功后，标记为新注册用户
                GameData.userData.userProgress.isRegister = true;

                onSuccess?.Invoke(true);
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onTimeout?.Invoke();
            });

    }

    //账号绑定平台
    public async void OnLinkAccount(string linkType, string linkID, Action<string> onSuccessLoginIn, Action onSuccessLink, Action onFailure, Action onTimeout)
    {

        string udid = GameData.userData.userAccount.udid;
        if (string.IsNullOrEmpty(udid))
        {
            Debug.LogError("=== CloudAccount: udid not exists, please register a new account first ===");
            return;
        }

        var args = new
        {
            action = "Link",
            isDev = GameConfig.main.productMode != ProductMode.Release,
            udid,
            linkType,
            linkID
        };

        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCOUNT), jsonData,
            (result) =>
            {
                JObject contentObject = JObject.Parse(result);
                string udid = contentObject["udid"].Value<string>();

                //判空 空表示绑定成功 非空表示该平台有绑定账号
                if (string.IsNullOrEmpty(udid))
                {
                    onSuccessLink?.Invoke();
                }
                else
                {
                    onSuccessLoginIn?.Invoke(udid);
                }
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onTimeout?.Invoke();
            });
    }

    //获取链接状态
    public async void OnGetLinkStatus(Action<LinkStatusArgs> onSuccess, Action onFailure, Action onTimeout)
    {
        var args = new
        {
            action = "GetStatus",
            isDev = GameConfig.main.productMode != ProductMode.Release,
            udid = GameData.userData.userAccount.udid
        };

        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCOUNT), jsonData,
            (result) =>
            {
                LinkStatusArgs linkStatusArgs = JsonConvert.DeserializeObject<LinkStatusArgs>(result);
                onSuccess?.Invoke(linkStatusArgs);
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onTimeout?.Invoke();
            });
    }

    //账号解绑平台
    public async void OnUnlinkAccount(string linkType, string linkID, Action onSuccess, Action onFailure, Action onTimeout)
    {
        string udid = GameData.userData.userAccount.udid;
        if (string.IsNullOrEmpty(udid))
        {
            Debug.LogError("=== CloudAccount: udid not exists, please register a new account first ===");
            return;
        }
        var args = new
        {
            action = "Unlink",
            isDev = GameConfig.main.productMode != ProductMode.Release,
            udid,
            linkType,
            linkID
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCOUNT), jsonData,
            (result) =>
            {
                JObject contentObject = JObject.Parse(result);
                bool success = contentObject["success"].Value<bool>();
                if (success)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    onFailure?.Invoke();
                }
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onFailure?.Invoke();
            },
            () =>
            {
                onTimeout?.Invoke();
            });
    }

}
