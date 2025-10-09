using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TGame;

public class DebuggerTGame : MonoBehaviour
{
    public void OnOpenGM()
    {
        //TGameSDK.GetInstance().UnityOpenGM();
        Debugger.Instance.OnCloseDebug();
    }

    public void OnDebugTGameAdInterstitial()
    {
        AdControl.Instance.OnAdInterstitial(AllAd.dictData[AdType.InterstitalBattleEnd], () =>
        {
            Debug.Log("=== DebuggerTGame: ad interstitial completed ===");
        }, () =>
        {
            Debug.Log("=== DebuggerTGame: ad interstitial failed ===");
        });
    }
}
