using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CloudGroupAB : Singleton<CloudGroupAB>
{
    //获取AB测试是否开启
    public async void GetGroupABOpen()
    {
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_GROUP_AB), "",
            (result) =>
            {
                JObject contentObject = JObject.Parse(result);
                GroupABSystem.Instance.SetGroupABOpen(contentObject["isOpen"].Value<bool>());
            }, null, null, null, false);
    }

    //获取AB测试关卡数据
    public async void GetGroupABLevel(Action callBack)
    {
        var args = new
        {
            action = "GetAllLevel"
        };
        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_LEVEL), jsonData,
            (result) =>
            {
                GroupABSystem.Instance.SetGroupABLevel(JsonConvert.DeserializeObject<List<GroupABBattleLevelArgs>>(result));
                callBack?.Invoke();
            }, () =>
            {
                // TipManager.Instance.OnTip(LocalizationUtility.GetLocalization("tip/tip_battle_network_anomaly"));
            }, () =>
            {
                // TipManager.Instance.OnTip(LocalizationUtility.GetLocalization("tip/tip_battle_network_anomaly"));
            },
            () =>
            {
                // TipManager.Instance.OnTip(LocalizationUtility.GetLocalization("tip/tip_battle_network_anomaly"));
            }, false);
    }
}