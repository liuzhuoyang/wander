using System;

public static class TimeUtility
{
    /// <summary>
    /// 传入总秒数，返回 00h00m00s形式
    /// </summary>
    public static string GetTimeFormat(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        string[] textArray = timeText.Split(new char[] { ':' });

        int d = int.Parse(textArray[0]);
        int h = int.Parse(textArray[1]);
        int m = int.Parse(textArray[2]);
        int s = int.Parse(textArray[3]);

        string text = "";

        if (d != 0)
        {
            text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_d_h"), d.ToString(), h.ToString());
            if (h == 0)
            {
                text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_d"), d.ToString());
            }
        }
        else
        {
            if (h != 0)
            {
                text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_h_m"), h.ToString(), m.ToString());
                if (m == 0)
                {
                    text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_h"), h.ToString());
                }
            }
            else
            {
                if (m != 0)
                {
                    text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_m_s"), m.ToString(), s.ToString());
                    if (s == 0)
                    {
                        text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_m"), m.ToString());
                    }
                }
                else
                {
                    text = string.Format(UtilityLocalization.GetLocalization("dynamic/timer_s"), s.ToString());
                }
            }
        }
        return text;
    }

    /// <summary>
    /// 00:00 的形式
    /// </summary>
    /// <param name="seconds">秒数</param>
    /// <returns>格式化的时间字符串</returns>
    public static string GetMinSecTimeFormat(int seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);

        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        string[] textArray = timeText.Split(new char[] { ':' });
        return timeText;
    }

    /// <summary>
    /// 将时间戳转换为DateTime
    /// </summary>
    /// <param name="timespan">Unix时间戳（秒）</param>
    /// <returns>对应的DateTime对象</returns>
    public static DateTime ConvertTimespanToDateTime(long timespan)
    {
        // 定义Unix纪元起始点
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 将时间戳（秒）添加到纪元
        return epoch.AddSeconds(timespan);
    }

    /// <summary>
    /// 将DateTime转换为时间戳
    /// </summary>
    /// <param name="dateTime">要转换的DateTime对象</param>
    /// <returns>Unix时间戳（秒）</returns>
    public static long ConvertDateTimeToTimespan(DateTime dateTime)
    {
        // 定义Unix纪元起始点
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // 确保输入DateTime为UTC时间
        DateTime utcDateTime = dateTime.ToUniversalTime();

        // 计算给定DateTime与纪元之间的差值
        TimeSpan timeSpan = utcDateTime - epoch;

        // 返回总秒数
        return (long)timeSpan.TotalSeconds;
    }
}