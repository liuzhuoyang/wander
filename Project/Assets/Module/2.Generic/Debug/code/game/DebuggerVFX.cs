using UnityEngine;
using System.Collections.Generic;

public class DebuggerVFX : MonoBehaviour
{
    public void OnDebugUIVFXFlyerCoin()
    {
        Debugger.Instance.OnCloseDebug();
        UIItemFlyerManager.Instance.OnVFXFlayerBatchUI(new List<RewardArgs>(){
            new RewardArgs()
            {
                reward = ConstantItem.COIN,
                num = 1
            }
        });
    }
}
