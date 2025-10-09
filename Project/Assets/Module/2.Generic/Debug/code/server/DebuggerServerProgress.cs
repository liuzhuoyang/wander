using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerServerProgress : MonoBehaviour
{
    public void OnSaveLocal()
    {
        GameData.userData.userAccount.saveTime = TimeManager.Instance.GetCurrentTimeSpan();
        ReadWrite.WriteUserdata(GameData.userData);
    }

    public void OnSaveLocalNotTime()
    {
        ReadWrite.WriteUserdata(GameData.userData);
    }
}
