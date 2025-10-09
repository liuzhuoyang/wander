using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    int secPassedInGame = 0;
    public long secAFK = 0;
    public long secTempAFK = 0;
    long loginTimespan = 0;
    long tempAFKTimespan = 0;
    public void Init()
    {
        loginTimespan = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        StartCoroutine(SecondCounter());
    }

    private void OnDestroy()
    {
        StopCoroutine(SecondCounter());
    }

    IEnumerator SecondCounter()
    {
        float nextTime = Time.realtimeSinceStartup + 1f;  // 计划下一次更新时间
        while (true)
        {
            yield return new WaitUntil(() => Time.realtimeSinceStartup >= nextTime);  // 等待直到下一次更新时间
            nextTime = Time.realtimeSinceStartup + 1f;  // 更新下次触发时间

            secPassedInGame++;
            GameData.userData.userStats.totalPlayTime++;

            // 检查是否已过10分钟的整数倍
            if (CheckIsPass10Min())
            {
                Debug.Log("=== TimeManager: another 10 minutes have passed ===");
                // AnalyticsControl.Instance.OnLogPlayTime(GameData.userData.userStats.totalPlayTime);
            }

            //注意这里发送的事件记录的是游戏里度过的秒数，并没有记录离线时间
            EventManager.TriggerEvent<TimeArgs>(EventNameTime.EVENT_TIME_TICK, new TimeArgs { secPassedSinceStart = secPassedInGame });
        }
    }

    public void SetTimespan(long timespan)
    {
        this.loginTimespan = timespan;
        secAFK = GetAFKSeconds(timespan);
        GameData.userData.userAccount.SetUserLoginTime(timespan);
        Debug.Log(" === TimeManager: get network time succeed, offline seconds: " + secAFK + " ===");
    }

    // public async UniTask<bool> GetNetTime(Action<long> onSuccess, Action onFailure, Action onTimeout)
    // {
    //     var utcs = new UniTaskCompletionSource<bool>();

    //     var args = new
    //     {
    //         action = "GetTime",
    //         zone = "Global",
    //     };

    //     string jsonData = JsonConvert.SerializeObject(args);

    //     await CloudFunction.PostCloudFunctionAsync(CloudFunctionAPI.GetFunctionUrl(CloudFunctionNames.F_ACCESS), jsonData,
    //         (result) =>
    //         {
    //             utcs.TrySetResult(true);
    //             NetTimeArgs netTimeArgs = JsonConvert.DeserializeObject<NetTimeArgs>(result);
    //             long timespan = netTimeArgs.timespan;
    //             this.loginTimespan = timespan;
    //             onSuccess?.Invoke(timespan);
    //             GameData.userData.userAccount.SetUserLoginTime(timespan);
    //             secAFK = GetAFKSeconds(timespan);

    //             Debug.Log(" === TimeManager: get network time succeed, offline seconds: " + secAFK + " ===");
    //         },
    //         () =>
    //         {
    //             //utcs.TrySetResult(false);
    //             onFailure?.Invoke();
    //             Debug.Log(" === TimeManager: failed to get network time, offline seconds: " + 0 + " ===");
    //         },
    //         () =>
    //         {
    //             onFailure?.Invoke();
    //         },
    //         () =>
    //         {
    //             onTimeout?.Invoke();
    //         });


    //     return await utcs.Task;
    // }

    #region 前后台切换时间
    public void SetTempAFK()
    {
        tempAFKTimespan = GetCurrentTimeSpan();
    }

    public void UpdateTempAFKTime()
    {
        //Debug.Log("=== TimeManager: secTempAFK: " + Utility.ConvertTimespanToDateTime(secTempAFK));
        //Debug.Log("=== TimeManager: afk time: " + (GetCurrentTimeSpan() - secTempAFK));
        secTempAFK = GetCurrentTimeSpan() - tempAFKTimespan;

        //TODO测试提示作用
        if (GameConfig.main.productMode != ProductMode.Release)
        {
            TipManager.Instance.OnTip($"AFK {secTempAFK} seconds");
        }
    }

    #endregion

    /*
    //离线模式获得时间
    public void GetLocalTime()
    {
#if !UNITY_WEBGL
        if (GameData.userData.userAccount.isOfflineTimer)
        {
            //上次登陆是离线模式，本次也是，直接用本地时间给玩
            long localTimespan = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();//用本地时间
            GameData.userData.userAccount.SetUserLoginTime(localTimespan);
            secAFK = GetAFKSeconds(localTimespan);
        }
        else
        {
            //上次登陆不是离线模式，本次才进入的离线模式
            secAFK = 0;//设置离线时间为0，放置刷
            GameData.userData.userAccount.isOfflineTimer = true;
        }
#endif
    }*/

    //上次登陆时间
    public int GetAFKSeconds(long timespan)
    {
        int second = 0;
        if (GameData.userData.userAccount.saveTime == 0) return 0;
        second = (int)(timespan - GameData.userData.userAccount.saveTime); //保存离线时间
        return second;
    }


    /*
    /// <summary>
    /// 检查距离上次登录是否超过24小时
    /// </summary>
    /// <returns></returns>
    public bool CheckIsPast24Hours()
    {
        return true;
    }*/

    public bool CheckIsPass10Min()
    {
        return GameData.userData.userStats.totalPlayTime % 600 == 0;
    }

    /// <summary>
    /// 检查是否过了一天
    /// </summary>
    public bool CheckIsNewDay()
    {
        // 假设loginTime和lastLoginTime是以Unix时间戳表示的, timespan是最新获得网络时间，loginTime是用户上次登陆的时间，还没有进行更新
        DateTime loginDateTime = DateTimeOffset.FromUnixTimeSeconds(loginTimespan).DateTime;
        DateTime lastLoginDateTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime).DateTime;

        // 将DateTime转换为本地时间或UTC时间
        DateTime loginDate = loginDateTime.ToLocalTime().Date;
        DateTime lastLoginDate = lastLoginDateTime.ToLocalTime().Date;

        // 检查是否跨越了日历天
        return loginDate > lastLoginDate;
    }

    /// <summary>
    /// 检查是否过了一周
    /// </summary>
    public bool CheckIsNewWeek()
    {
        DateTime loginDateTime = DateTimeOffset.FromUnixTimeSeconds(loginTimespan).UtcDateTime;
        DateTime lastLoginDateTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime).UtcDateTime;

        // 获取表示年份和周数的字符串
        string loginWeek = $"{loginDateTime.Year}-{CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(loginDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}";
        string lastLoginWeek = $"{lastLoginDateTime.Year}-{CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(lastLoginDateTime, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}";

        // 检查周数是否改变
        return loginWeek != lastLoginWeek;
    }

    /// <summary>
    /// 检查是否过了一月
    /// </summary>
    public bool CheckIsNewMonth()
    {
        DateTime loginDateTime = DateTimeOffset.FromUnixTimeSeconds(loginTimespan).UtcDateTime;
        DateTime lastLoginDateTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime).UtcDateTime;

        // 检查年份或月份是否改变
        return loginDateTime.Year > lastLoginDateTime.Year ||
               (loginDateTime.Year == lastLoginDateTime.Year && loginDateTime.Month > lastLoginDateTime.Month);
    }

    /// <summary>
    /// 获取当前时间
    /// </summary>
    public long GetCurrentTimeSpan()
    {
        long timeSpan = loginTimespan + (long)Time.realtimeSinceStartup;//secPassed;
        // Debug.Log("=== TimeManager: current time: " + Utility.ConvertTimespanToDateTime(timeSpan) + " ===");
        return timeSpan;
    }

    /*
        public long GetCurrentOffsetTimeSpan()
        {
            long timeSpan = loginTimespan + (long)Time.realtimeSinceStartup;
            Debug.Log("=== TimeManager: current offset time: " + Utility.ConvertTimespanToDateTime(timeSpan) + " ===");
            return timeSpan;
        }*/


    /// <summary>
    /// 获得到下一个月份第一天的剩余秒数
    /// </summary>
    public int GetSecondUntilNextMonth()
    {
        // 获取当前的Unix时间戳（UTC时间0点）
        long currentTimeSpan = GetCurrentTimeSpan();

        // 将Unix时间戳转换为DateTimeOffset（UTC时间）
        DateTimeOffset currentTime = DateTimeOffset.FromUnixTimeSeconds(currentTimeSpan);

        // 计算下一个月的第一天，确保使用UTC时间
        DateTimeOffset nextMonthFirstDay = new DateTimeOffset(currentTime.Year, currentTime.Month, 1, 0, 0, 0, TimeSpan.Zero).AddMonths(1);

        // 将下一个月的第一天转换为Unix时间戳
        long nextMonthFirstDayTimeSpan = nextMonthFirstDay.ToUnixTimeSeconds();

        // 计算秒数差值
        int secondsUntilNextMonth = (int)(nextMonthFirstDayTimeSpan - currentTimeSpan);

        return secondsUntilNextMonth;
    }

    //到下个周一剩余的秒数 UTC时间
    public int GetSecondsUntilNextWeek()
    {
        // 获取当前的Unix时间戳（UTC时间0点）
        long currentTimeSpan = GetCurrentTimeSpan();

        // 将Unix时间戳转换为DateTimeOffset（UTC时间）
        DateTimeOffset currentTime = DateTimeOffset.FromUnixTimeSeconds(currentTimeSpan);

        // 计算当前日期是本周的第几天（0表示周日，1表示周一，...，6表示周六）
        int currentDayOfWeek = (int)currentTime.DayOfWeek;

        // 计算距离下一个周一的天数差
        int daysUntilNextMonday = (8 - currentDayOfWeek) % 7;
        if (daysUntilNextMonday == 0)
        {
            daysUntilNextMonday = 7; // 如果当前是周一，则计算到下一个周一的天数
        }

        // 计算下一个周一的日期，并确保它是UTC时间的0点
        DateTimeOffset nextMonday = new DateTimeOffset(currentTime.Year, currentTime.Month, currentTime.Day, 0, 0, 0, TimeSpan.Zero).AddDays(daysUntilNextMonday);

        // 将下一个周一转换为Unix时间戳
        long nextMondayTimeSpan = nextMonday.ToUnixTimeSeconds();

        // 计算秒数差值
        int secondsUntilNextMonday = (int)(nextMondayTimeSpan - currentTimeSpan);

        return secondsUntilNextMonday;
    }

    //获取到明天的秒数 UTC时间
    public int GetSecondUntilNextDay()
    {
        long currentTimeSpan = GetCurrentTimeSpan();

        // Calculate seconds until next UTC 0:00:00
        DateTime currentUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(currentTimeSpan);
        DateTime nextUTCZero = currentUTC.Date.AddDays(1);
        int secondsUntilNextUTCZero = (int)(nextUTCZero - currentUTC).TotalSeconds;

        return secondsUntilNextUTCZero;
    }

    //获取当前时间所在指定间隔的结束秒数
    public int GetSecondUntilNextInterval(int interval)
    {
        long currentTimestamp = GetCurrentTimeSpan(); // 获取当前时间戳（秒）
        long intervalDuration = interval * 24 * 60 * 60; // interval 天的秒数
        return (int)((currentTimestamp / intervalDuration + 1) * intervalDuration);
    }

    public void UpdateLoginTime()
    {
        GameData.userData.userAccount.SetUserLoginTime(loginTimespan);
    }

    //距离目标日期的剩余事件
    public int GetSecondUntilResetDate(DurationType resetType)
    {
        switch (resetType)
        {
            case DurationType.Weekly:
                return GetSecondsUntilNextWeek();
            case DurationType.Monthly:
                return GetSecondUntilNextMonth();
            default:
                return GetSecondUntilNextDay();
        }
    }

    //获取指定时间的当天零点
    public int GetZeroTimeSpanOfDay(long timestamp)
    {
        DateTime dateTime = ConvertFromTimeSpan(timestamp);
        DateTime zeroTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
        return ConvertToTimeSpan(zeroTime);
    }
    // 转换 DateTime 到时间戳（秒）
    public int ConvertToTimeSpan(DateTime time)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (int)(time.ToUniversalTime() - epoch).TotalSeconds;
    }

    // 从时间戳（秒）转换成 DateTime
    public DateTime ConvertFromTimeSpan(long timeS)
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddSeconds(timeS);
    }
}


public enum DurationType
{
    Daily,
    Weekly,
    Monthly,
    Day3,
    Endless
}