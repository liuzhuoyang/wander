using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerFeature : MonoBehaviour
{
    public void OnRestart()
    {
        Game.Instance.Restart();
    }

    public void OnResetUserFile()
    {
        ReadWrite.DeleteUserData();
        Game.Instance.Restart();
    }
}
