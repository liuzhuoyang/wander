using System.Collections.Generic;
using UnityEngine;

//功能按钮脚本
public class FeatureHandler : MonoBehaviour
{
    public FeatureType feature;

    public void OnFeature()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1)
        {
            Debug.Log("=== OnFeature Return === ");
            return;
        }

        FeatureUtility.OnFeature(feature);
    }
}
