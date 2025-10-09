using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIModeBattleFightDebug : DebuggerSharedMenu
{
    public TMP_Dropdown dropGear;
    public void Init()
    {

    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnDebugWaveEnd()
    {
        BattleSystem.Instance.OnChangeBattleState(BattleStates.WaveFightEnd);
    }
}
