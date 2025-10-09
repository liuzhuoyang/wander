using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaleHandler : MonoBehaviour
{
    public CanvasScaler canvasScaler;

    void Awake()
    {
        if (canvasScaler == null)
        {
            Debug.LogError("CanvasScaler reference not set in the inspector.");
            return;
        }

        AdjustCanvasScale();
    }

    void AdjustCanvasScale()
    {
        Debug.Log("=== UIScaleHandler : current screen size: " + Screen.width + "//" + Screen.height + " ===");
        canvasScaler.referenceResolution = canvasScaler.referenceResolution * 1.28f;
        canvasScaler.matchWidthOrHeight = 1f;

        if(IsTablet())
        {
            canvasScaler.referenceResolution = canvasScaler.referenceResolution * 0.95f;
            canvasScaler.matchWidthOrHeight = 1f;
            //canvasScaler.matchWidthOrHeight = 0.8f;
        }
    }

    bool IsTablet()
    {
        Debug.Log("=== UIScaleHandler : IsTablet : " + Screen.width + "//" + Screen.height + " ===");
        // Calculate the diagonal screen size in inches
        float screenWidthInches = Screen.width / Screen.dpi;
        float screenHeightInches = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(screenWidthInches * screenWidthInches + screenHeightInches * screenHeightInches);

        // Tablets typically have a screen size of 7 inches or more diagonally
        return diagonalInches >= 7;
    }
}
