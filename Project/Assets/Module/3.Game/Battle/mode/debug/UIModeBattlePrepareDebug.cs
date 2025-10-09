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

    public void OnDebugGetTileBrick()
    {
        BattleSystem.Instance.OnRefreshBrick();
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


