using UnityEngine;
using TMPro;
using System.Linq;

public class UIModeBattlePrepareDebug : DebuggerSharedMenu
{
    public TMP_InputField inputFieldWave;

    public GameObject objDebugMenu;

    public void Init()
    {
       
    }

    public void OnDebugPrepareEnd()
    {
        BattleSystem.Instance.OnChangeBattleState(BattleStates.PrepareEnd);
    }

    public void OnDebugOpen()
    {
        objDebugMenu.SetActive(true);
    }

    public void OnDebugClose()
    {
        objDebugMenu.SetActive(false);
    }
}


