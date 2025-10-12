using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text;

public static class Utility
{

    public static string GetXYStringFormat(int x, int y)
    {
        return x + "_" + y;
    }

    //获取枚举里的随机对象
    public static T GetRandomEnumValue<T>()
    {
        // 获取枚举类型
        Array enumValues = Enum.GetValues(typeof(T));

        // 随机选择一个枚举值
        T randomEnumValue = (T)enumValues.GetValue(Random.Range(0, enumValues.Length));

        return randomEnumValue;
    }

    /// string转换enum
    public static bool TryParseEnum<TEnum>(string value, out TEnum result) where TEnum : struct
    {
        return Enum.TryParse(value, true, out result) && Enum.IsDefined(typeof(TEnum), result);
    }

    /// /// 点击位置转换为世界坐标
    public static Vector2 GetTouchWorldPostion()
    {
        return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
    }

    #region Extension
    public static Color GetOpaqueColor(this Color color)
    {
        Color tempColor = color;
        tempColor.a = 1;
        return tempColor;
    }
    public static Color GetClearColor(this Color color)
    {
        Color tempColor = color;
        tempColor.a = 0;
        return tempColor;
    }
    #endregion

    #region 驼峰命名转换为蛇形命名
    //将驼峰命名转换为蛇形命名
    public static string ConvertCamelToSnake(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        StringBuilder result = new StringBuilder();
        foreach (char c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append('_');
            }
            result.Append(char.ToLower(c));
        }
        return result.ToString();
    }
    #endregion

}
