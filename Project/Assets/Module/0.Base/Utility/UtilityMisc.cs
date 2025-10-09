using UnityEngine;

public static class UtilityMisc
{
    public static string GetStoreName()
    {
        string storeName = "";
        string installerName = Application.installerName;
        switch (installerName)
        {
            case "com.android.vending":
                storeName = "Google Play";
                break;
            case "com.amazon.venezia":
                storeName = "Amazon Appstore";
                break;
            case "com.apple.appstore":
                storeName = "Apple App Store";
                break;
            default:
                storeName = "Unknown";
                break;
        }

#if UNITY_EDITOR
        return "Editor";
#endif

        return storeName;
    }
}
