using UnityEngine;
using System;

public class VersionManager : Singleton<VersionManager>
{
    public CheckVersionArgs checkVersionArgs;
    public void Init()
    {
        checkVersionArgs = new CheckVersionArgs();
        checkVersionArgs.isUpdateNeeded = false;
        checkVersionArgs.isUnknownUser = false;
        checkVersionArgs.version = Application.version;
        checkVersionArgs.latestVersion = "0.0.0";
        checkVersionArgs.forcedVersion = "0.0.0";
        //判断当前版本和登陆版本是否一致
        OnCheckLoginVersion();
    }

    void OnCheckLoginVersion()
    {
        if (string.Equals(GameData.userData.userMisc.loginVersion, Application.version))
        {
            return;
        }
        //如果不一致说明更新版本，需要处理相关操作
        GameData.userData.userMisc.loginVersion = Application.version;
    }

    #region 检查建议版本
    public bool OnCheckLatestVersion()
    {
        if (checkVersionArgs.isUpdateNeeded)
        {
            return false;
        }
        if (!IsVersionBehind(checkVersionArgs.version, checkVersionArgs.latestVersion))
        {
            return false;
        }
        return true;
    }

    bool IsVersionBehind(string v1, string v2)
    {
        string[] a1 = v1.Split('.');
        string[] a2 = v2.Split('.');
        for (int i = 0; i < Math.Max(a1.Length, a2.Length); i++)
        {
            int n1 = int.Parse(a1[i]), n2 = int.Parse(a2[i]);
            if (n1 < n2) return true;
            if (n1 > n2) return false;
        }
        return false;
    }
    #endregion

    #region 检查服务器最低版本
    public void OnCheckServerVersion(CheckVersionArgs args, Action onUpdateNeeded, Action onPass)
    {
        checkVersionArgs.isUpdateNeeded = args.isUpdateNeeded;
        checkVersionArgs.latestVersion = args.latestVersion;
        checkVersionArgs.forcedVersion = args.forcedVersion;

        if (checkVersionArgs.isUpdateNeeded)
        {
            OnUpdateMessage();
            onUpdateNeeded?.Invoke();
        }
        else
        {
            onPass?.Invoke();
        }
    }
    #endregion

    void OnUpdateMessage()
    {
        MsgUpdateArgs args = new MsgUpdateArgs() { 
            target = "msg_update", 
            currentVersion = checkVersionArgs.version, 
            targetVersion = checkVersionArgs.latestVersion };
        MessageControl.OnShowMessage<MsgUpdateArgs>(args);
    }
}
