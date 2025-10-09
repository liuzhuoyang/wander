using UnityEngine;
using TMPro;
using System;

public class DebuggerTimer : MonoBehaviour
{
    public TextMeshProUGUI textTickSinceStart;
    public TextMeshProUGUI textTickTotalSec;
    public TextMeshProUGUI textNetTime;
    public TextMeshProUGUI textRegisterTime;
    public TextMeshProUGUI textLoginTime;
    //public TextMeshProUGUI textLastLoginTime;

    void OnEnable()
    {
        if (GameData.userData == null) return;
        UpdateTimeView();
        Debug.Log("StartOnTick");
        //textLastLoginTime.text = Utility.ConvertTimespanToDateTime(GameData.userData.userAccount.lastLoginTime).ToString();
        EventManager.StartListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }

    void UpdateTimeView()
    {
        textRegisterTime.text = TimeUtility.ConvertTimespanToDateTime(GameData.userData.userAccount.registerTime).ToString();
        textLoginTime.text = TimeUtility.ConvertTimespanToDateTime(GameData.userData.userAccount.loginTime).ToString();
    }

    void OnDisable()
    {
        EventManager.StopListening<TimeArgs>(EventNameTime.EVENT_TIME_TICK, OnTick);
    }

    void OnTick(TimeArgs args)
    {
        textTickTotalSec.text = TimeUtility.GetTimeFormat(args.secPassedSinceStart);
    }

    public async void OnSyncNetTime()
    {
        //MessageManager.Instance.OnLoading();
        await CloudAccess.Instance.GetNetTime(
            (timespan) =>
            {
                //MessageManager.Instance.CloseLoading();
                textNetTime.text = TimeUtility.ConvertTimespanToDateTime(timespan).ToString();
            },
            () =>
            {

            },
            () =>
            {
                // MessageManager.Instance.OnTimeout();
            });
    }

    public void OnSetToLastDay()
    {
        // 获取当前的时间，并转换为DateTimeOffset
        DateTimeOffset currentTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime);

        // 减去一天并加上5分钟
        DateTimeOffset newTime = currentTime.AddDays(-1).AddMinutes(5);

        // 更新lastLoginTime
        GameData.userData.userAccount.loginTime = newTime.ToUnixTimeSeconds();
        GameData.userData.userAccount.saveTime = newTime.ToUnixTimeSeconds();

        UpdateTimeView();
    }

    public void OnSetToLastWeek()
    {
        // 获取当前的时间，并转换为DateTimeOffset
        DateTimeOffset currentTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime);

        // 减去一周并加上5分钟
        DateTimeOffset newTime = currentTime.AddDays(-7).AddMinutes(5);

        // 更新lastLoginTime
        GameData.userData.userAccount.loginTime = newTime.ToUnixTimeSeconds();
        GameData.userData.userAccount.saveTime = newTime.ToUnixTimeSeconds();
        UpdateTimeView();
    }

    public void OnSetToLastMonth()
    {
        // 获取当前的时间，并转换为DateTimeOffset
        DateTimeOffset currentTime = DateTimeOffset.FromUnixTimeSeconds(GameData.userData.userAccount.loginTime);

        // 减去一个月并加上5分钟
        DateTimeOffset newTime = currentTime.AddMonths(-1).AddMinutes(5);

        // 更新lastLoginTime
        GameData.userData.userAccount.loginTime = newTime.ToUnixTimeSeconds();
        GameData.userData.userAccount.saveTime = newTime.ToUnixTimeSeconds();

        UpdateTimeView();
    }
}
