using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebuggerGeneric : MonoBehaviour
{
    public TextMeshProUGUI textToken;

    public async void OnGetJWEToken()
    {
        await TokenManager.Instance.OnRefreshToken(() =>
        {
            textToken.text = ZPlayerPrefs.GetString("token");
        },
        () =>
        {

        },
        () =>
        {

        });
    }
}
