using UnityEngine;

public static class UtilityTextFormat
{
    
    //字体默认颜色（废弃）
    public static Color GetTextColor(TextColorType colorType)
    {
        switch (colorType)
        {
            case TextColorType.Title:
                return new Color(50f / 255f, 60f / 255f, 130f / 255f);
            case TextColorType.TextLight:
                return new Color(120f / 255f, 120f / 255f, 180f / 255f);
            default:
                return Color.white;
        }
    }

    #region 字体颜色
    /// <summary>
    /// 传入两个数字，足够显示绿色，不够显示红色，最终输出带颜色的00/00格式
    /// </summary>
    public static string FormatTextColor(int num1, int num2)
    {
        if (num1 >= num2) //材料足够，绿 
            return "<color=green>" + num1 + "</color>/" + num2;
        else
            return "<color=red>" + num1 + "</color>/" + num2;
    }

    // 传入两个数字，足够显示绿色，不够显示红色
    public static string GetNumColor(int num, int targetNum)
    {
        if (num <= targetNum)
            return "<color=green>" + num + "</color>";
        return "<color=red>" + num + "</color>";
    }
    #endregion

    /// <summary>
    /// 转换价值为BMK
    /// </summary>
    public static string GetNumFormat(int num)
    {
        const int BILLION = 1_000_000_000;
        const int MILLION = 1_000_000;
        const int THOUSAND = 1000;

        if (num >= BILLION) return $"{(num / (double)BILLION):0.0#}b";
        if (num >= MILLION) return $"{(num / (double)MILLION):0.0#}m";
        if (num >= THOUSAND) return $"{(num / (double)THOUSAND):0.0#}k";

        return num.ToString();
    }

}
