using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuggerSystem : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropStoryboard;

    public DebuggerFeatureUnlocker featureUnlocker;
    public void Init()
    {

    }

    public void OnDebugReward()
    {
        Debugger.Instance.OnCloseDebug();
        RewardSystem.Instance.OnReward(new List<RewardArgs>
        {
            new RewardArgs
            {
                reward = ConstantItem.COIN,
                num = 100
            },
            new RewardArgs
            {
                reward = ConstantItem.GEM,
                num = 50
            }
        });
    }

    public void OnOpenFeatureForceUnlock()
    {
        featureUnlocker.Open();
        //Debugger.Instance.OnCloseDebug();
        //PopupFeatureForceUnlockArgs args = new PopupFeatureForceUnlockArgs();
        //args.popupName = "debug_popup_feature_unlock";
        //PopupManager.Instance.OnPopup(args);
    }

    public void OnDebugTipPower()
    {
        Debugger.Instance.OnCloseDebug();
        PowerSystem.Instance.OnDebugTipPower();
    }
}
