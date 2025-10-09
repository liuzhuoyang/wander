using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class DebuggerServerAccount : MonoBehaviour
{
    public TextMeshProUGUI textUDID;
    public TextMeshProUGUI textUserID;
    public TextMeshProUGUI textRegisterTime;

    public void OnRegisterNewAccount()
    {
        CloudAccount.Instance.OnRegisterNewAccount(false, (isNewRegisterUser) => { }, () => { }, () => { });
    }

    public async void OnFetchAccount()
    {
        var args = new
        {
            action = "Fetch",
            platform = "Global",
            openID = "",
            udid = "66ed12c8dac15807c26cd895",
            characterDict = new Dictionary<int, UserCharacter>()
        };

        string jsonData = JsonConvert.SerializeObject(args);
        await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCOUNT), jsonData,
            (result) =>
            {
                Debug.Log("=== DebuggerAccount: sucessfully fetch a user. result: " + result + "===");
            });
    }
}

public class UserAccountArgs
{
    public string action;
    public string udid;
    public string userID;
    public string openID;
    public string registerTime;
    public Dictionary<int, UserCharacter> characterDict;
}
