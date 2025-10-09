using UnityEngine;

public static class UIUtility
{
    // 判断屏幕比例超过特定的长款比例，比如折叠屏手机的很长，这种情况需要处理一些ui的显示
    // Samsung Galaxy Z Fold2 5G 分辨率为 960 x 2658, 比例为0.36
    public static bool IsScreenOverRatio(float ratio = 0.4f)
    {
        // 计算屏幕宽高比
        float screenRatio = (float)Screen.width / Screen.height;

        // 输出屏幕的宽度和高度
        Debug.Log($"屏幕宽度: {Screen.width} 像素, 屏幕高度: {Screen.height} 像素");
        Debug.Log($"屏幕比例: {screenRatio}");

        if (screenRatio >= ratio)
        {
            return false;
        }
        return true;
    }
}
