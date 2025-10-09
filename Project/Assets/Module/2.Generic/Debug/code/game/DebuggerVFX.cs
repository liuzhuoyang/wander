using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DebuggerVFX : MonoBehaviour
{
    void OnEnable()
    {
        //unitToggle.onValueChanged.AddListener(OnToggleUnitRender);
    }
    void OnDisable()
    {
        //unitToggle.onValueChanged.RemoveListener(OnToggleUnitRender);
    }

    public void OnToggleVFX()
    {
        //VFXControl.Instance.ForceNoVFX = !vfxToggle.isOn;
    }

    public void OnDebugUIVFXFlyerCoin()
    {
        Debugger.Instance.OnCloseDebug();
        VFXControl.Instance.OnVFXFlayerBatchUI(new List<RewardArgs>()
        {
            new RewardArgs()
            {
                reward = ConstantItem.COIN,
                num = 100
            }
        });
    }
}
