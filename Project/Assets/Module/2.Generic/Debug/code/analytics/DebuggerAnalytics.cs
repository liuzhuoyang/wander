using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerAnalytics : MonoBehaviour
{
    public void OnDebugMixLogin()
    {
        Debug.Log("=== OnDebugAnalytics: login ===");
        AnalyticsControl.Instance.OnLogin();
    }

    public void OnDebugMixIAPSelect()
    {
        Debug.Log("=== OnDebugAnalytics: iap select ===");
    }

    public void OnDebugAmpIAPSelect()
    {
        Debug.Log("=== OnDebugAnalytics: iap select ===");
    }

    public void OnDebugAmpLogin()
    {
        Debug.Log("=== OnDebugAnalytics: login ===");
        AnalyticsControl.Instance.OnLogin();
    }
}
