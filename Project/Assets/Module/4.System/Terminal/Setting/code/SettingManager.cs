using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System;
//using TGame;

public class SettingManager : Singleton<SettingManager>
{
    public int isSoundOn;
    public int isMusicOn;
    public int isNotiOn;
    public int isHapticOn;

    public LinkStatusArgs linkStatusArgs;

    #region 初始化
    public void Init()
    {
        linkStatusArgs = new LinkStatusArgs();
        linkStatusArgs.facebookID = "";
        linkStatusArgs.appleID = "";
        linkStatusArgs.googleID = "";

        isSoundOn = PlayerPrefs.GetInt("sound", 1);
        isMusicOn = PlayerPrefs.GetInt("music", 1);
        isNotiOn = PlayerPrefs.GetInt("noti", 1);
        isHapticOn = PlayerPrefs.GetInt("haptic", 1);
    }
    #endregion

    #region 功能开关
    //音效
    public void OnSound()
    {
        isSoundOn *= -1;
        Debug.Log("=== SettingSystem: sound " + isSoundOn);
        PlayerPrefs.SetInt("sound", isSoundOn);
        Refresh();

        if (isSoundOn == 1)
            AudioControl.Instance.UnMuteSound();
        else
            AudioControl.Instance.MuteSound();
    }

    //音乐
    public void OnMusic()
    {
        isMusicOn *= -1;
        Debug.Log("=== SettingSystem: music " + isMusicOn);
        PlayerPrefs.SetInt("music", isMusicOn);
        Refresh();

        if (isMusicOn == 1)
            AudioControl.Instance.UnMuteMusic();
        else
            AudioControl.Instance.MuteMusic();
    }

    //推送同志
    public void OnNoti()
    {
        isNotiOn *= -1;
        Debug.Log("=== SettingSystem: noti " + isNotiOn);
        PlayerPrefs.SetInt("noti", isNotiOn);
        Refresh();
    }

    //震动
    public void OnHaptic()
    {
        isHapticOn *= -1;
        Debug.Log("=== SettingSystem: haptic " + isHapticOn);
        PlayerPrefs.SetInt("haptic", isHapticOn);
        Refresh();
    }
    #endregion

    #region 交互
    // public async void Open(GameObject parent)
    // {
    //     // 打开设置界面
    //     GameObject prefab = await GameAssets.GetPrefabAsync("ui_setting");
    //     if (prefab == null)
    //     {
    //         Debug.LogError($"=== SettingManager: Open Prefab not found: ui_setting ===");
    //         return;
    //     }

    //     GameObject page = Instantiate(prefab, parent.transform, false);
    //     page.name = "ui_setting";
    //     Refresh();
    // }

    public async void Open()
    {
        await UIMain.Instance.OpenUI("setting", UIPageType.Overlay);
        Refresh();
    }

    public void Refresh()
    {
        EventManager.TriggerEvent<UISettingArgs>(EventNameSetting.EVENT_SETTING_REFRESH_UI, new UISettingArgs
        {
            uid = GameData.userData.userAccount.userID.ToString(),
            isSoundOn = isSoundOn == 1,
            isMusicOn = isMusicOn == 1,
            isNotiOn = isNotiOn == 1,
            isHapticOn = isHapticOn == 1
        }); ;
    }

    public void Close()
    {
        EventManager.TriggerEvent<UISettingArgs>(EventNameSetting.EVENT_SETTING_CLOSE_UI, new UISettingArgs
        {
        });
    }

    public void OnLanguage()
    {

    }

    public void OnCopyUserID()
    {
        string userID = GameData.userData.userAccount.userID.ToString();
        GUIUtility.systemCopyBuffer = userID;
        TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("tip/tip_setting_id_copy"));
    }
    #endregion


    public void OnSupport()
    {
        // int userId = GameData.userData.userAccount.userID;
        // Mail.Send("support@onicore.games", "GameID:" + userId, "");
        //TGameSDK.AIHelpLanguage(TGameSDK.UTGetLanguage());
        //TGameSDK.AIHelpOpenRPA("E001");
        //TGameSDK.AIHelpAddTags("game id", GameData.userData.userAccount.userID.ToString());
    }

    public void OnPrivacy()
    {
        //TGameSDK.OPOpenPrivacy();
    }

    public void OnTerms()
    {
        //TGameSDK.OPOpenTerms();
    }

    #region 辅助方法
    // 用于确保URL编码正确处理特殊字符
    string EscapeURL(string url)
    {
        return UnityEngine.Networking.UnityWebRequest.EscapeURL(url);
    }
    #endregion

    #region 用户账户绑定
    public void OnDeleteAccount()
    {
#if !UNITY_WEBGL
        MessageManager.Instance.OnConfrim(ConstantLocKey.MSG_DELETE_ACCOUNT, () =>
        {
            // AnalyticsControl.Instance.OnLogAccountDelete(GameData.userData.userAccount.userID);//打点
            ReadWrite.DeleteUserData();
            Game.Instance.Restart();
            // ZPlayerPrefs.DeleteAll();
        });
        //Popup
#endif
    }

    //上传用户数据
    //callbackSucceed 成功回调，关闭上传按钮
    public void OnUploadData(Action callbackSucceed = null)
    {
#if !UNITY_WEBGL
        MessageManager.Instance.OnLoading();
        CloudProgress.Instance.OnUploadProgress(() =>
        {
            callbackSucceed?.Invoke();
        }, () =>
        {
            // MessageManager.Instance.OnTimeout();
        }, () =>
        {
            // MessageManager.Instance.OnTimeout();
        });
#endif
    }

    //下载用户数据
    UserData cloudUserData;
    public void OnDownloadData(string udid)
    {
#if !UNITY_WEBGL
        MessageManager.Instance.OnLoading();
        CloudProgress.Instance.OnDownloadProgress(
            udid,
            (string stream) =>
            {
                Close();
                cloudUserData = JsonConvert.DeserializeObject<UserData>(stream);
                //打点下载成功
                // AnalyticsControl.Instance.OnLogAccountSyncDownload();//打点
                Debug.Log("=== 下载成功: OnDownloadData ===");
                OnConfirmSyncCloudData();
                // PopupAccountArgs args = new PopupAccountArgs()
                // {
                //     target = "popup_account",

                //     localID = GameData.userData.userAccount.userID.ToString(),
                //     localChapter = GameData.userData.userProgress.dictProgress[ChapterMode.Normal].chapter,
                //     localSaveTime = Utility.ConvertTimespanToDateTime(GameData.userData.userAccount.saveTime).ToString(),

                //     cloudID = cloudUserData.userAccount.userID.ToString(),
                //     cloudChapter = cloudUserData.userProgress.dictProgress[ChapterMode.Normal].chapter,
                //     cloudSaveTime = Utility.ConvertTimespanToDateTime(cloudUserData.userAccount.saveTime).ToString()
                // };
                // PopupManager.Instance.OnPopup(args);
            },
        () =>
        {
            //下载错误
            Debug.Log("=== 下载失败: OnDownloadData ===");
            // AnalyticsControl.Instance.OnLogAccountSyncError("download fail");//打点
        },
        () =>
        {
            //没有找到数据
            Debug.Log("=== 没有找到数据: OnDownloadData ===");
            // AnalyticsControl.Instance.OnLogAccountSyncError("file not found");//打点
        }, () =>
        {
            // MessageManager.Instance.OnTimeout();
        });
#endif
    }

    //如果从Facebook登陆后，没有找到数据，会显示另外的提示
    //     public void OnDownloadData(string entityID, string entityType)
    //     {
    // #if !UNITY_WEBGL
    //         MessageManager.Instance.OnLoading();

    //         /*
    //         PlayfabPlayerData.Instance.OnDownloadUserData(
    //             entityID,
    //             entityType,
    //             (string stream) =>
    //             {
    //                 Close();
    //                 cloudUserData = JsonConvert.DeserializeObject<UserData>(stream);
    //                 //关闭读取遮挡
    //                 MessageManager.Instance.CloseLoading();
    //                 //打点下载成功
    //                 AnalyticsControl.Instance.OnLogAccountSyncDownload();//打点
    //                 PopupManager.Instance.OnPopupAccount(new PopupAccountArgs()
    //                 {
    //                     localID = GameData.userData.userAccount.userID,
    //                     localChapter = GameData.userData.userProgress.chapter,
    //                     localSaveTime = Utility.ConvertTimespanToDateTime(GameData.userData.userAccount.saveTime).ToString(),

    //                     cloudID = cloudUserData.userAccount.userID,
    //                     cloudChapter = cloudUserData.userProgress.chapter,
    //                     cloudSaveTime = Utility.ConvertTimespanToDateTime(cloudUserData.userAccount.saveTime).ToString()
    //                 }); ;
    //             },
    //         () =>
    //         {
    //             //下载错误
    //             TipManager.Instance.OnTip(GameData.AllLocalization[ConstantLocKey.TIP_LOGIN_ERROR]);
    //             AnalyticsControl.Instance.OnLogAccountSyncError("download fail");//打点
    //             MessageManager.Instance.CloseLoading();
    //         },
    //         () =>
    //         {
    //             //没有找到数据
    //             TipManager.Instance.OnTip(GameData.AllLocalization[ConstantLocKey.TIP_NO_DATA]);
    //             AnalyticsControl.Instance.OnLogAccountSyncError("file not found");//打点
    //             MessageManager.Instance.CloseLoading();
    //         });*/
    // #endif
    //     }

    //确认应用云端数据，覆盖本地，保存，重启
    public void OnConfirmSyncCloudData()
    {
#if !UNITY_WEBGL
        GameData.userData = cloudUserData;
        cloudUserData = null;
        Game.Instance.Restart();
#endif
    }

    //确认应用当前数据，切换Faceebok账号绑定
    public void OnConfirmSyncLocalData()
    {
#if !UNITY_WEBGL
        /*
        MessageManager.Instance.OnLoading();
        PlayfabAuth.Instance.OnLinkFacebook(
            true,
            () =>
            {
                //切换成功
                MessageManager.Instance.CloseLoading();
                GameData.userData.userAccount.isLinkFacebook = true;
                AnalyticsControl.Instance.OnLogAccountSyncApplyLocal();//打点
                GameSaver.Instance.OnSave();
            },
            () =>
            {
                //切换失败
                MessageManager.Instance.CloseLoading();
                GameData.userData.userAccount.isLinkFacebook = false;
                AnalyticsControl.Instance.OnLogAccountSyncError("failed to sync local data");//打点
                GameSaver.Instance.OnSave();
            },
            () =>
            {

            }
            );*/
#endif

    }

    #region 苹果登录
    public void OnLinkApple(Action callbackSucceed)
    {
        /*
#if UNITY_IOS
        MessageManager.Instance.OnLoading();
        Debug.Log("=== 开始登录: OnLinkApple ===");
        TGOAppleLoginNew.Instance.AppleLogin((isSuccess) =>
            {
                MessageManager.Instance.CloseLoading();
                if (isSuccess)
                {
                    // 登录成功
                    string token = TGOAppleLoginNew.Instance.GetToken();
                    Debug.Log("=== 登录成功: OnLinkApple ===" + token);
                    CloudAccount.Instance.OnLinkAccount("Apple", token,
                        (udid) =>
                        {
                            Debug.Log("=== 登录成功: OnLinkApple ===" + udid);
                            linkStatusArgs.appleID = token;
                            OnDownloadData(udid, token);
                        },
                        () =>
                        {
                            Debug.Log("=== 登录成功: OnLinkApple ===");
                            TipManager.Instance.OnTip("Link Apple Success");
                            linkStatusArgs.appleID = token;
                            OnUploadData();
                            callbackSucceed?.Invoke();
                        },
                        () =>
                        {
                            Debug.Log("=== 登录失败: OnLinkApple ===");
                            TipManager.Instance.OnTip("Link Apple Failed");
                        });
                }
            });
#endif
        */
    }

    public void OnUnlinkApple(Action callbackSucceed)
    {
#if UNITY_IOS
/*
        if (linkStatusArgs.appleID != "")
        {
            TGOAppleLoginNew.Instance.AppleLogout();
            Debug.Log("=== 登出成功: OnUnlinkApple ===");
            MessageManager.Instance.OnLoading();
            CloudAccount.Instance.OnUnlinkAccount("Apple", linkStatusArgs.appleID,
                () =>
                {
                    MessageManager.Instance.CloseLoading();
                    Debug.Log("=== 解绑成功: OnUnlinkApple ===");
                },
                () =>
                {
                    MessageManager.Instance.CloseLoading();
                    Debug.Log("=== 解绑失败: OnUnlinkApple ===");
                });
        }
        linkStatusArgs.appleID = "";
        callbackSucceed?.Invoke();
*/
#endif
    }
    #endregion

    #region Facebook登录
    public void OnLinkFacebook(Action callbackSucceed)
    {
        /*
#if UNITY_ANDROID

        MessageManager.Instance.OnLoading();
        Debug.Log("=== 开始登陆: OnLinkFacebook ===");
        TGOFBLoginNew.Instance.FBLogin((isSuccess) =>
        {
            MessageManager.Instance.CloseLoading();
            if (isSuccess)
            {
                Debug.Log("=== 登陆成功: OnLinkFacebook ===");
                string token = TGOFBLoginNew.Instance.GetToken();
                CloudAccount.Instance.OnLinkAccount("Facebook", token,
                (udid) =>
                {
                    Debug.Log("=== 登录成功: OnLinkFacebook ===" + udid);
                    linkStatusArgs.facebookID = token;
                    OnDownloadData(udid);
                },
                () =>
                {
                    Debug.Log("=== 登录成功: OnLinkFacebook ===");
                    TipManager.Instance.OnTip("Link Facebook Success");
                    linkStatusArgs.facebookID = token;
                    OnUploadData();
                    callbackSucceed?.Invoke();
                },
                () =>
                {
                    Debug.Log("=== 登录失败: OnLinkFacebook ===");
                    TipManager.Instance.OnTip("Link Facebook Failed");
                },
                () =>
                {
                    // MessageManager.Instance.OnTimeout();
                });
            }
        });
#endif*/
    }
    //解除Facebook绑定
    public void OnUnlinkFacebook(Action callbackSucceed)
    {
        /*
#if UNITY_ANDROID
        if (linkStatusArgs.facebookID != "")
        {
            TGOFBLoginNew.Instance.FBLogout();
            Debug.Log("=== 登出成功: OnUnlinkFacebook ===");
            MessageManager.Instance.OnLoading();
            CloudAccount.Instance.OnUnlinkAccount("Facebook", linkStatusArgs.facebookID,
                () =>
                {
                    MessageManager.Instance.CloseLoading();
                    Debug.Log("=== 解绑成功: OnUnlinkFacebook ===");
                },
                () =>
                {
                    MessageManager.Instance.CloseLoading();
                    Debug.Log("=== 解绑失败: OnUnlinkFacebook ===");
                },
                () =>
                {
                    // MessageManager.Instance.OnTimeout();
                });
        }
        linkStatusArgs.facebookID = "";
        callbackSucceed?.Invoke();
#endif*/
    }
    #endregion
    /*
    //Facebook端登出游戏
    public void LogoutFromFacebook(Action callbackSucceed)
    {
        PlayfabAuth.Instance.UnlinkAndLogoutFacebook(
           () =>
           {

           },
           () =>
           {
               MessageManager.Instance.CloseLoading();
               TipManager.Instance.OnTip("Failed to unlink Facebook"); //TODO
               Debug.Log("=== SettingSystem: failed to unlink to facebook account  ===");
           },
           () =>
           {
               TipManager.Instance.OnTip("Cancel unlinking Facebook"); //TODO
           },
           () =>
           {
               TipManager.Instance.OnTip("No Game Account linked to this Facebook Account"); //TODO
           });
    }*/
    #endregion

    #region 获取链接状态
    public void OnGetLinkStatus(Action callback)
    {
        if (GameConfig.main.productMode == ProductMode.DevOffline)
        {
            callback?.Invoke();
            return;
        }
        MessageManager.Instance.OnLoading();
        CloudAccount.Instance.OnGetLinkStatus(
            (result) =>
            {
                Debug.Log("=== 获取链接状态: OnGetLinkStatus ===" + result.facebookID + " " + result.appleID + " " + result.googleID);
                linkStatusArgs.facebookID = result.facebookID;
                linkStatusArgs.appleID = result.appleID;
                linkStatusArgs.googleID = result.googleID;
                callback?.Invoke();
                MessageManager.Instance.CloseLoading();
            },
            () =>
            {
                Debug.Log("=== 获取链接状态失败: OnGetLinkStatus ===");
                callback?.Invoke();
                MessageManager.Instance.CloseLoading();
            },
            () =>
            {
                // MessageManager.Instance.OnTimeout();
            }
        );
    }
    #endregion

}
