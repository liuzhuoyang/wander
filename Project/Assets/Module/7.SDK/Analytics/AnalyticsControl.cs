using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
//using GameAnalyticsSDK;
//using TGame;
using System.Collections;

public static class AnalyticsKey
{
    #region Amplitude/Game Analytics 数据分析点位
    public const string REGISTER = "REGISTER";      //注册（首次登陆）
    public const string LOGIN = "LOGIN";            //登陆游戏
    public const string LOGIN_DAY = "LOGIN DAY";    //累计登陆天数
    public const string IAP_SUCCEED = "IAP SUCCEED";
    public const string IAP_FAILED  = "IAP FAILED";
    public const string IAP_SELECT  = "IAP SELECT";
    public const string AD_SELECT   = "AD SELECT";   //选择广告
    public const string AD_REQUEST  = "AD REQUEST"; //请求广告
    public const string AD_SHOW     = "AD SHOW";    //播放广告
    public const string AD_COMPLETE = "AD COMPLETE";//完成播放
    public const string AD_FAILED   = "AD FAILED";  //播放失败
    public const string AD_NOT_LOADED = "AD NOT LOADED"; //广告没装填
    public const string AD_INTER_SHOW = "AD INTER SHOW"; //插屏广告开始
    public const string AD_INTER_COMPLETE = "AD INTER COMPLETE"; //插屏广告 
    public const string USER_ATTRIBUTION = "USER ATTRIBUTION"; //用户属性识别，用户来源
    public const string USER_ATTRIBUTION_FAIL = "USER ATTRIBUTION FAIL"; //用户属性识别失败
    public const string USER_ATTRIBUTION_STEP = "USER ATTRIBUTION STEP"; //用户属性标签抓取步骤，Debug用

    //账户数据
    public const string ACCOUNT_LOGIN = "ACCOUNT LOGIN";
    public const string ACCOUNT_LOGOUT = "ACCOUNT LOGOUT";
    public const string ACCOUNT_LINK = "ACCOUNT LINK";
    public const string ACCOUNT_UNLINK = "ACCOUNT UNLINK";
    public const string ACCOUNT_CONNECT_ERROR = "ACCOUNT CONNECT ERROR"; //连接错误
    public const string ACCOUNT_DELETE = "ACCOUNT DELETE";
    public const string ACCOUNT_SYNC_ERROR = "ACCOUNT SYNC ERROR";//数据错误
    public const string ACCOUNT_SYNC_UPLOAD = "ACCOUNT SYNC UPLOAD";
    public const string ACCOUNT_SYNC_DOWNLOAD = "ACCOUNT SYNC DOWNLOAD";
    public const string ACCOUNT_SYNC_APPLY_LOCAL = "ACCOUNT SYNC APPLY LOCAL";
    public const string ACCOUNT_SYNC_APPLY_CLOUD = "ACCOUNT SYNC APPLY CLOUD";

    //自定义
    public const string PROGRESS_CHAPTER_UNLOCK = "CHAPTER UNLOCK";
    public const string PROGRESS_CHAPTER_COMPLETE = "CHAPTER COMPLETE";
    public const string LEVEL_START = "LEVEL START";
    public const string LEVEL_FAIL = "LEVEL FAIL";
    public const string LEVEL_WIN = "LEVEL WIN";
    public const string LEVEL_REVIVE = "LEVEL REVIVE";
    public const string LEVEL_QUIT = "LEVEL QUIT";
    #endregion

    #region Event Properties 参数
    public const string P_VALUE = "value";
    public const string P_TOTAL = "total";
    public const string P_TOTAL_DAILY = "total_daily";
    public const string P_USER_ID = "user_id";
    public const string P_PRODUCT_ID = "product_id";
    public const string P_PRICE = "price";
    public const string P_SKU = "sku";
    public const string P_REVENUE = "revenue";
    public const string P_CHAPTER_ID = "chapter_id";
    public const string P_LEVEL_ID = "level_id";
    public const string P_WAVE = "wave";
    public const string P_TYPE = "type";
    public const string P_AD_PROVIDER = "ad_provider";
    public const string P_INSTALL_STORE = "install_store";

    //用户分层属性
    public const string P_INSTALL_SOURCE = "install_source";
    public const string P_IS_PAY_USER = "is_paying_user";
    public const string P_USER_ACQUISITION = "user_acquisition";
    public const string P_AEO_TYPE = "aeo_type";
    #endregion

    #region Firebase/Meta点位
    //用户基础行为
    public const string Launch_Event = "Launch_Event";                      //发送发生启动游戏事件的次数
    public const string Launch_User = "Launch_User";                        //发生启动游戏事件时触发
    //付费事件
    public const string IAP_User = "IAP_User";                              //发生内购付费事件1次以上的用户
    //User
    public const string User_Join_Main_User = "User_Join_Main_User";        //用户通过Loading界面进入到主界面上传
    public const string User_Join_Num_User = "User_Join_Num_User";          //每天重新进去游戏次数，进入游戏时上传今天是第几次进入游戏
    public const string User_Day_1_User = "User_Day_1_User";                //用户第一天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_2_User = "User_Day_2_User";                //用户第二天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_3_User = "User_Day_3_User";                //用户第三天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_4_User = "User_Day_4_User";                //用户第四天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_5_User = "User_Day_5_User";                //用户第五天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_6_User = "User_Day_6_User";                //用户第六天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_7_User = "User_Day_7_User";                //用户第七天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_15_User = "User_Day_15_User";              //用户第十五天进入游戏（自然日后台返回游戏也算登录）
    public const string User_Day_30_User = "User_Day_30_User";              //用户第三十天进入游戏（自然日后台返回游戏也算登录）
    //广告_激励视频
    public const string Reward_Viedo1_Event = "Reward_Viedo1_Event";        //每天看一次激励广告
    public const string Reward_Viedo3_Event = "Reward_Viedo3_Event";        //每天看三次激励广告
    public const string Reward_Viedo5_Event = "Reward_Viedo5_Event";        //每天看五次激励广告
    public const string Reward_Viedo7_Event = "Reward_Viedo7_Event";        //每天看七次激励广告
    public const string Reward_Viedo10_Event = "Reward_Viedo10_Event";      //每天看十次激励广告
    public const string Reward_Viedo20_Event = "Reward_Viedo20_Event";      //每天看二十次激励广告
    public const string RewardAds_Request = "Ad_Reward_ClickTotal";     //所有激励点位按钮点击次数总计,会出现装载没完成的情况
    public const string RewardAds_Not_Ready  = "Ad_Reward_LoadingTotal";    //所有激励点位点击按钮后出现ad is loading的次数
    public const string RewardAds_Show_Event = "RewardAds_Show_Event";      //发生看游戏内激励广告（包含完成或未完成）事件时触发
    public const string RewardAds_Show_User = "RewardAds_Show_User";        //发生看游戏内激励广告（包含完成或未完成）事件时触发，看广告的用户数 (每日刷新)
    public const string RewardAds_Event = "RewardAds_Event";                //发生看游戏内激励广告完成事件时触发
    public const string RewardAds_User = "RewardAds_User";                  //发生看游戏内激励广告完成事件时触发，看广告的用户数 (每日刷新)

    //自定义
    public const string Chapter_Complete_X = "Chapter_Complete_";           //完成关卡
    #endregion

    #region Adjust点位
    //用户基础行为
    public const string Adjust_Launch_Event = "dev8tg";//发送发生启动游戏事件的次数
    public const string Adjust_Launch_User  = "qnmw21";//发生启动游戏事件时触发
    //付费事件
    public const string Adjust_IAP_User     = "fdfyco";//发生内购付费事件1次及以上的用户
    public const string Adjust_IAP_Event    = "9tguvu";//发生内购付费事件
    //User
    public const string Adjust_User_Join_Loading_User = "w5m38j";//用户点击桌面图标进入到Loading界面上传
    public const string Adjust_User_Join_Main_User    = "tmulxh";//用户通过Loading界面进入到主界面上传
    //Retention
    public const string Adjust_OpenDay1_User  = "p32kcl";//用户第一天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay2_User  = "p3r8rt";//用户第二天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay3_User  = "9xafmy";//用户第三天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay4_User  = "m28aqx";//用户第si天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay5_User  = "dsn9ip";//用户第五天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay6_User  = "qregsb";//用户第六天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay7_User  = "wa0us9";//用户第七天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay15_User = "4w8xlb";//用户第十五天进入游戏（自然日后台返回游戏也算登录）
    public const string Adjust_OpenDay30_User = "z6tqjh";//用户第三十天进入游戏（自然日后台返回游戏也算登录）

    //Playing Time
    public const string First_PlayGameOver_10m_User = "1edtp0";//发送首次安装用户打开应用且当日累计时长超过十分钟的用户数
    public const string First_PlayGameOver_20m_User = "ukmr3u";//发送首次安装用户打开应用且当日累计时长超过二十分钟的用户数
    public const string First_PlayGameOver_30m_User = "qm1j3w";//发送首次安装用户打开应用且当日累计时长超过三十分钟的用户数
    public const string PlayGameOver_210m_User      = "37rfpa";//发送打开应用且累计时长超过210分钟的用户数
    public const string PlayGameOver_420m_User      = "t9autp";//发送打开应用且累计时长超过420分钟的用户数
    public const string PlayGameOver_800m_User      = "vmfi9b";//发送打开应用且累计时长超过800分钟的用户数

    //广告激励视频
    public const string Adjust_Reward_Viedo1_Event  = "a3w8wa";//每天看1次激励广告
    public const string Adjust_Reward_Viedo3_Event  = "c12zqm";//每天看3次激励广告
    public const string Adjust_Reward_Viedo5_Event  = "rranii";//每天看5次激励广告
    public const string Adjust_Reward_Viedo7_Event  = "gfim1r";//每天看7次激励广告
    public const string Adjust_Reward_Viedo10_Event = "59uo2v";//每天看10次激励广告
    public const string Adjust_Reward_Viedo20_Event = "fqirv6";//每天看20次激励广告
    public const string Adjust_RewardAds_Event      = "9myio9";//发生看游戏内激励广告完成事件时触发
    public const string Adjust_RewardAds_User       = "evcqoq";//发生看游戏内激励广告完成事件时触发

    #endregion
}

public class AnalyticsControl : Singleton<AnalyticsControl>
{
    const string ADJUST_ID = "";
    //const string AMPLITUDE_API_KEY = "30f585d4fcd67129af9c0a9362327f7e"; // space 的 api key

    #region Maping
    //用户登陆次数
    int[] mapingLoginCount = { 1, 2, 3, 4, 5, 6, 7, 15, 30 };
    string[] loginCountEventFirebase =
    {   AnalyticsKey.User_Day_1_User,
        AnalyticsKey.User_Day_2_User,
        AnalyticsKey.User_Day_3_User,
        AnalyticsKey.User_Day_4_User,
        AnalyticsKey.User_Day_5_User,
        AnalyticsKey.User_Day_6_User,
        AnalyticsKey.User_Day_7_User,
        AnalyticsKey.User_Day_15_User,
        AnalyticsKey.User_Day_30_User,
    };
    string[] loginCountEventAdjust =
    {
        AnalyticsKey.Adjust_OpenDay1_User,
        AnalyticsKey.Adjust_OpenDay2_User,
        AnalyticsKey.Adjust_OpenDay3_User,
        AnalyticsKey.Adjust_OpenDay4_User,
        AnalyticsKey.Adjust_OpenDay5_User,
        AnalyticsKey.Adjust_OpenDay6_User,
        AnalyticsKey.Adjust_OpenDay7_User,
        AnalyticsKey.Adjust_OpenDay15_User,
        AnalyticsKey.Adjust_OpenDay30_User,
    };

    //每日看了几个广告
    int[] mapingAdDailyCount = { 1, 3, 5, 7, 10, 20 };
    string[] adDailyCountEventFirebase =
    {
        AnalyticsKey.Reward_Viedo1_Event,
        AnalyticsKey.Reward_Viedo3_Event,
        AnalyticsKey.Reward_Viedo5_Event,
        AnalyticsKey.Reward_Viedo7_Event,
        AnalyticsKey.Reward_Viedo10_Event,
        AnalyticsKey.Reward_Viedo20_Event,
    };
    string[] adDailyCountEventAdjust =
    {
        AnalyticsKey.Adjust_Reward_Viedo1_Event,
        AnalyticsKey.Adjust_Reward_Viedo3_Event,
        AnalyticsKey.Adjust_Reward_Viedo5_Event,
        AnalyticsKey.Adjust_Reward_Viedo7_Event,
        AnalyticsKey.Adjust_Reward_Viedo10_Event,
        AnalyticsKey.Adjust_Reward_Viedo20_Event,
    };

    int[] mapingPlayTimeFirstDay = { 600, 1200, 1800};
    string[] playTimeFirstDayEventAdjust =
    {
        AnalyticsKey.First_PlayGameOver_10m_User,
        AnalyticsKey.First_PlayGameOver_20m_User,
        AnalyticsKey.First_PlayGameOver_30m_User,
    };

    int[] mapingPlayTimeTotal = { 12600, 25200, 48000 };
    string[] playTimeTotallEventAdjust =
    {
        AnalyticsKey.PlayGameOver_210m_User,
        AnalyticsKey.PlayGameOver_420m_User,
        AnalyticsKey.PlayGameOver_800m_User,
    };
    #endregion

    public void InitSDK()
    {
        //有些分析平台需要在这个阶段就初始化，比如说需要抓报错
        //AMP不能在这里初始化，因为需要绑定ID，而ID需要等用户登陆后才能获取
    }
    
    #region Init
    //传入是否是新注册用户，上一步CloudAccount.cs中注册成功后会标记，等待这里初始化时候触发注册事件
    //必须先等待CloudAccount完成，因为需要有UDID后才能操作AMP
    public void Init(bool isNewRegisterUser = false)
    {
        //离线开发模式不初始化分析平台
        if(GameConfig.main.productMode == ProductMode.DevOffline)
        {
            Debug.Log("=== AnalyticsControl: init analytics (dev offline) ===");
            return;
        }

        InitAMP();
        InitGA();
        OnLaunch();

        if(isNewRegisterUser)
        {
            OnRegister();
        }
    }

    //初始化Amplitude
    void InitAMP()
    {
        if(!IsAmpOn()) return; //如果amp关闭，不初始化AMP

        Amplitude amplitude = Amplitude.getInstance();
        amplitude.setServerUrl("https://api2.amplitude.com");
        amplitude.logging = true;
        amplitude.trackSessionEvents(true);
        amplitude.init(GameConfig.main.amplitudeAppKey, GameData.userData.userAccount.userID.ToString()); //初始化amp, 绑定ID
    }

    //初始化Game Analytics
    void InitGA()
    {
        //要在Init之前设置自定义ID
        //GameAnalytics.SetCustomId(GameData.userData.userAccount.userID.ToString());

        //GameAnalytics.Initialize();
    }

/*
    void InitAdjust()
    {
        if (Game.Instance.isDebugMode)
        {
            Debug.Log("=== AnalyticsControl: init adjust (sandbox) ===");
            AdjustConfig config = new AdjustConfig(ADJUST_ID, AdjustEnvironment.Sandbox);
            Adjust.InitSdk(config);
        }
        else
        {
            Debug.Log("=== AnalyticsControl: init adjust (production) ===");
            AdjustConfig config = new AdjustConfig(ADJUST_ID, AdjustEnvironment.Production);
            config.AttributionChangedDelegate = this.AttributionChangedCallback;
            Adjust.InitSdk(config);
        }
    }

    //用户分层

    private class FacebookInstallReferrerData
    {
        [JsonProperty("campaign_name")]
        public string campaignName;
    }

    private void AttributionChangedCallback(AdjustAttribution attribution)
    {
        OnLogUserAttributionStep(AdjustAttributionStep.UserAttributionObtained);

        //Fb进来的使用fbInstallReferrer
        if (!string.IsNullOrEmpty(attribution.FbInstallReferrer))
        {
            string fbInfo = attribution.FbInstallReferrer;
            try
            {
                var referrerData = JsonUtility.FromJson<FacebookInstallReferrerData>(fbInfo);
                string campaignName = referrerData.campaignName;

                Debug.Log("=== AnalyticsControl: Facebook install referrer campaign: " + campaignName);

                GameData.userData.userTag.installSource = "Facebook";

                if (campaignName.Contains("AEO") || campaignName.Contains("Aeo") || campaignName.Contains("aeo"))
                {
                    GameData.userData.userTag.aeoType = AeoType.IAP;
                }

                //收到激活回调，进行用户分层打点
                OnLogUserAttribution();
            }
            catch (Exception e)
            {
                OnLogUserAttributionFail("Facebook");
                Debug.LogError("Error parsing fbInstallReferrer: " + e.Message);
            }

            //辅助打点
            OnLogUserAttributionStep(AdjustAttributionStep.FacebookInstallReferrerObtained);
            return;
        }
        else
        {
            //辅助打点
            OnLogUserAttributionStep(AdjustAttributionStep.FacebookInstallReferrerNotExist);
        }

        if (attribution != null)
        {
            //判断是否自然量用户
            bool isOrganic = string.IsNullOrEmpty(attribution.Network) || attribution.Network.Equals("organic", System.StringComparison.OrdinalIgnoreCase);
            if (isOrganic)
            {
                OnLogUserAttributionStep(AdjustAttributionStep.OrganicUserDetected);
                GameData.userData.userTag.installSource = "Organic";
                GameData.userData.userTag.userAcquisition = UserAcquisitionType.Ogranic;
            }
            else
            {
                OnLogUserAttributionStep(AdjustAttributionStep.PaidUserDetected);
                GameData.userData.userTag.userAcquisition = UserAcquisitionType.AD;
                //判断是否Aeo用户
                //GameData.userData.userTag.isAEO = true;
                if (attribution.Campaign.Contains("AEO"))
                {
                    GameData.userData.userTag.aeoType = AeoType.IAP;
                }
                GameData.userData.userTag.installSource = attribution.Network;
                Debug.Log("=== AnalyticsControl: attribution campaign: " + attribution.Campaign);
            }

            //收到激活回调，进行用户分层打点
            OnLogUserAttribution();
        }
        else
        {
            OnLogUserAttributionStep(AdjustAttributionStep.AttributionInfoNotExist);
            GameData.userData.userTag.installSource = "Organic";
            GameData.userData.userTag.userAcquisition = UserAcquisitionType.Ogranic;
            //用户分层失败
            OnLogUserAttributionFail("Organic");
        }
    }
    */
#endregion

    bool IsAmpOn()
    {
        return GameConfig.main.isAmpOn == AnalyticsTool.On;
    }

    #region Launch/Login
    public void OnLaunch()
    {
        //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "OnLaunch");
        /* 调优点位
        OnFirebaseLaunch();
        OnMetaLaunch();
        OnAdjustLaunch();
        */
    }

    /// <summary>
    /// 首次开启
    /// </summary>
    public void OnRegister()
    {
        string installStore = UtilityMisc.GetStoreName();
        int userID = GameData.userData.userAccount.userID;

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_INSTALL_STORE, installStore);
        eventProps.Add(AnalyticsKey.P_USER_ID, userID);
        Amplitude.Instance.logEvent(AnalyticsKey.REGISTER, eventProps);

        /* 调优点位
        OnFirebaseRegister();
        OnMetaRegister();
        OnAdjustRegister();
        */
    }

    /// <summary>
    /// 首次登陆
    /// </summary>
    public void OnLoginFirstTime()
    {
        /* 调优点位
        OnFirebaseLoginFirstTime();
        OnMetaLoginFirstTime();
        OnAdjustLoginFirstTime();
        */
    }

    public void OnLogin()
    {
        GameData.userData.userAnalytics.loginCount++;       //登陆总次数
        GameData.userData.userAnalytics.loginCountDaily++;  //当日登陆次数

        /* 调优点位
        OnFirebaseLogin();
        OnMetaLogin();
        */

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        string installStore = UtilityMisc.GetStoreName();
        int userID = GameData.userData.userAccount.userID;
        
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_USER_ID, userID);
        eventProps.Add(AnalyticsKey.P_INSTALL_STORE, installStore);
        //eventProps.Add(AnalyticsKey.P_USER_ACQUISITION, GameData.userData.userTag.userAcquisition);
        //eventProps.Add(AnalyticsKey.P_INSTALL_SOURCE, GameData.userData.userTag.installSource);
        //eventProps.Add(AnalyticsKey.P_AEO_TYPE, GameData.userData.userTag.aeoType);
        eventProps.Add(AnalyticsKey.P_IS_PAY_USER, GameData.userData.userTag.isPayUser);
        eventProps.Add(AnalyticsKey.LOGIN_DAY, GameData.userData.userAnalytics.loginDay);
        Amplitude.Instance.logEvent(AnalyticsKey.LOGIN, eventProps);

        /* 调优点位
        OnFirebaseRetention();
        OnMetaRetention();
        OnAdjustRetention();
        */

        Debug.Log("=== AnalyticsControl: on login ===");
    }
    #endregion

    #region Ads
    public void OnLogAdSelect(AdType adType, string adProvider, string adUnit)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        eventProps.Add(AnalyticsKey.P_AD_PROVIDER, adProvider);

        //Amplitude.Instance.logEvent(AnalyticsKey.AD_SELECT, eventProps);
    }
    
    //请求广告
    public void OnRequestAds(AdType adType)
    {
        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点
        
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.AD_REQUEST, eventProps);

        /* 调优点位
        OnFirebaseRequestAD();
        OnMetaRequestAD();
        */
    }

    public void OnLogAdShow(AdType adType, string adProvider, string adUnit)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        eventProps.Add(AnalyticsKey.P_AD_PROVIDER, adProvider);
        //Amplitude.Instance.logEvent(AnalyticsKey.AD_SHOW, eventProps);

        /* 调优点位
        OnFirebaseShowAD();
        OnFirebaseShowADUser();
        OnMetaShowAD();
        OnMetaShowADUser();
        */

        //GameData.userData.userAnalytics.isShowAdsDaily = true; //每日首次播放广告的用户数
        Debug.Log("=== AnalyticsControl: show ad ===");
    }

    //广告播放完成
    public void OnLogAdCompleted(AdType adType, string adProvider, float revenue)
    {
        GameData.userData.userAnalytics.rewardAdCount++;
        GameData.userData.userAnalytics.rewardAdCountDaily++;

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        int total = GameData.userData.userAnalytics.rewardAdCount;
        int totalDaily = GameData.userData.userAnalytics.rewardAdCountDaily;

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        eventProps.Add(AnalyticsKey.P_AD_PROVIDER, adProvider);
        eventProps.Add(AnalyticsKey.P_REVENUE, revenue);
        eventProps.Add(AnalyticsKey.P_TOTAL, total);
        eventProps.Add(AnalyticsKey.P_TOTAL_DAILY, totalDaily);
        Amplitude.Instance.logEvent(AnalyticsKey.AD_COMPLETE, eventProps);

        Debug.Log("=== AnalyticsControl: ad completed, ad provider: " + adProvider + " revenue: " + revenue + " ===");
    }

    public void OnLogAdInterstitialShow()
    {
        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        Amplitude.Instance.logEvent(AnalyticsKey.AD_INTER_SHOW, eventProps);

        Debug.Log("=== AnalyticsControl: ad interestitial show ===");
    }

    //插屏广告播放完成
    public void OnLogAdInterstitialCompleted()
    {
        GameData.userData.userAnalytics.interstitialAdCount++;
        GameData.userData.userAnalytics.interstitialAdCountDaily++;

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        int total = GameData.userData.userAnalytics.interstitialAdCount;
        int totalDaily = GameData.userData.userAnalytics.interstitialAdCountDaily;

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TOTAL, total);
        eventProps.Add(AnalyticsKey.P_TOTAL_DAILY, totalDaily);
        Amplitude.Instance.logEvent(AnalyticsKey.AD_INTER_COMPLETE, eventProps);

        Debug.Log("=== AnalyticsControl: ad interestitial completed ===");
    }

    //广告没有装填成功
    public void OnLogAdNotReady(AdType adType)
    {
        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.AD_NOT_LOADED, eventProps);

        /* 调优点位
        OnFirebaseNotReadyAD();
        OnMetaNotReadyAD();
        */
    }

    public void OnLogAdFailed(AdType adType)
    {
        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, adType.ToString());
        Amplitude.Instance.logEvent(AnalyticsKey.AD_FAILED, eventProps);

        Debug.Log("=== AnalyticsControl: ad failed ===");
    }

    #endregion

    #region IAP
    /*
    内购展示原因：参考 TGCString.cs 对应枚举项

    描述调用内购展示的页面来源，通常是页面或是活动相关的名称
    当前只需要处理直接访问商店 TGCString.storeNode_store.ToString() 

    未提及的默认使用 storeNode_other
    */

    /*
    public void OnLogIAPShow(TGCString storeNode = TGCString.storeNode_other)
    {
        TGDEvent.Instance.onStoreShow(storeNode.ToString());
    }

    public void OnLogIAPSelect(string sku, string productID, float price)
    {
        string storeID = productID + "_" + ((int)(price * 100)).ToString("D4");
        Debug.Log("=== AnalyticsControl: on log iap select, storeID: " + storeID + " ===");
        TGDEvent.Instance.onStoreClick(storeID);

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_PRODUCT_ID, productID);
        eventProps.Add(AnalyticsKey.P_SKU, sku);
        eventProps.Add(AnalyticsKey.P_PRICE, price);
        Amplitude.Instance.logEvent(AnalyticsKey.IAP_SELECT, eventProps);

        /*
        商品ID格式为【Product ID_价格 (四位数字，实际价格*100)】
        例  gem_tier_0199、pass_ticket_1499 (分别为 1.99 和 14.99 美元的商品)
        
    }*/

        
    public void OnLogIAPSucceed(string sku,string productID, float revenue) //string receipt, string receiptSignature)
    {
        GameData.userData.userAnalytics.isPayDaily = true;//最后设置为true，同一个用户本地不会再打

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_PRODUCT_ID, productID);
        eventProps.Add(AnalyticsKey.P_SKU, sku);
        eventProps.Add(AnalyticsKey.P_REVENUE, revenue);
        Amplitude.Instance.logEvent(AnalyticsKey.IAP_SUCCEED, eventProps);
        //Amplitude.Instance.logRevenue(productID, 1, revenue, receipt, receiptSignature);

        /* 调优点位
        OnFistbaseDailyIAPUser();
        OnMetaDailyIAPUser();

        OnMetaIAP(revenue);

        OnAdjustIAP(revenue);
        OnAdjustIAPDaily();
        */
        //TGDEvent.Instance.onStoreSuccess();
    }

    public void OnLogTGameIAPSucceed()
    {
        //TGDEvent.Instance.onStoreSuccess();
    }

    public void OnLogIAPFailed(string productID)
    {
        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_PRODUCT_ID, productID);
        Amplitude.Instance.logEvent(AnalyticsKey.IAP_FAILED, eventProps);
    }
    #endregion

    #region PlayTime
    public void OnLogPlayTime(int second)
    {
        /*
        Debug.Log("=== AnalyticsControl: on log play time, 10 mins passed ===");
        if (GameData.userData.userAnalytics.loginCount == 1)
        {
            for (int i = 0; i < mapingPlayTimeFirstDay.Length; i++)
            {
                if (second == mapingPlayTimeFirstDay[i])
                {
                    AdjustEvent adjustEvent = new AdjustEvent(playTimeFirstDayEventAdjust[i]);
                    Adjust.TrackEvent(adjustEvent);
                    Debug.Log("=== AnalyticsControl: log adjust event: " + playTimeFirstDayEventAdjust[i]);
                }
            }
        }
        else
        {
            Debug.Log("=== AnalyticsControl: the user is not the first day use, skipping first day play time ===");
        }

        for (int i = 0; i < mapingPlayTimeTotal.Length; i++)
        {
            if (second == mapingPlayTimeTotal[i])
            {
                AdjustEvent adjustEvent = new AdjustEvent(playTimeTotallEventAdjust[i]);
                Adjust.TrackEvent(adjustEvent);
                Debug.Log("=== AnalyticsControl: log adjust event: " + playTimeTotallEventAdjust[i]);
            }
        }*/
    }
    #endregion

    #region Progression

    //开始关卡打点
    public void OnLogLevelStart(LevelType levelType, int levelIndexID, int chapterID, LevelModeType chapterMode, int levelID, string levelDifficulty)
    {
        //非主线关卡不进行打点，跳出
        if(chapterMode != LevelModeType.Normal)
        {
            return;
        }

        /*
        当前的关卡序号，例如：关卡1-5，lv = 5 关卡2-1，lv = 6

        参考 TGCString.cs 对应枚举项
        【主线关卡】TGCString.main.ToString() 

        位于关卡时，传入当前的关卡难度(通常是SDK返回)

        当前关卡名称。例：关卡1-3，lv_id = "1-3" 
        */
        if(levelType == LevelType.Main)
        {
            /*
            //暂时只打普通关卡Normal的打点，等后续配置完成再加入其他
            TGDEvent.Instance.onLvStart(levelIndexID, TGCString.main.ToString(), levelDifficulty, new Hashtable()
            {
                { "lv_id", chapterID + "_" + levelID }
            });
            */
        }

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, levelType.ToString());          //关卡类型
        eventProps.Add(AnalyticsKey.P_CHAPTER_ID, chapterID.ToString());    //章节ID
        eventProps.Add(AnalyticsKey.P_LEVEL_ID, levelID.ToString());        //关卡ID
        eventProps.Add(AnalyticsKey.P_VALUE, chapterID + "_" + levelID);    //关卡名称
        Amplitude.Instance.logEvent(AnalyticsKey.LEVEL_START, eventProps);
    }

    //关卡失败打点
    /*
    TGDEvent.Instance.onLvFailed	string	lv_node	"
    失败原因：参考 TGCString.cs 对应枚举项
    【生命耗尽】TGCString.lvFailed_outLife.ToString() 

    int	lv_process	
    当前关卡的完成进度

    【=已完成波次/总波次*100】，取值范围 0 ~ 100）向上取整(Mathf.CeilToInt)
    例：在波次4失败时， (3/5*100)向上取整为 60 

    int	lv_wave	"
    当前波次
    例：在波次4失败时，lv_wave = 4
    
	string	lv_id	关卡名称。定义同【关卡开始】
    */
    public void OnLogLevelFail(LevelType levelType, int chapterID, LevelModeType chapterMode, int levelID, int currentWave, int totalWave)
    {
        //非主线关卡不进行打点，跳出
        if(chapterMode != LevelModeType.Normal)
        {
            return;
        }

        if(levelType == LevelType.Main)
        {
            float lvProgress = (float)currentWave / totalWave * 100;

            /*
            Hashtable ht = new Hashtable();
            ht.Add("lv_id", chapterID + "_" + levelID);
            ht.Add("lv_node", TGCString.lvFailed_outLife.ToString());
            ht.Add("lv_process", Mathf.CeilToInt(lvProgress));
            ht.Add("lv_wave", currentWave);
            TGDEvent.Instance.onLvFailed(ht);
            */
        }

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, levelType.ToString());
        eventProps.Add(AnalyticsKey.P_CHAPTER_ID, chapterID.ToString());
        eventProps.Add(AnalyticsKey.P_LEVEL_ID, levelID.ToString());
        eventProps.Add(AnalyticsKey.P_WAVE, currentWave.ToString());
        eventProps.Add(AnalyticsKey.P_VALUE, chapterID + "_" + levelID);
        Amplitude.Instance.logEvent(AnalyticsKey.LEVEL_FAIL, eventProps);
      
    }

    //关卡胜利打点
    public void OnLogLevelWin(LevelType levelType, int chapterID, LevelModeType chapterMode, int levelID)
    {
        //非主线关卡不进行打点，跳出
        if(chapterMode != LevelModeType.Normal)
        {
            return;
        }

        //TODO暂时只打普通关卡Normal的打点，等后续配置完成再加入其他
        if(levelType == LevelType.Main)
        {
            /*
            TGDEvent.Instance.onLvComplete(new Hashtable()
            {
                { "lv_id", chapterID + "_" + levelID }
            });*/
        }

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点
        
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, levelType.ToString());
        eventProps.Add(AnalyticsKey.P_CHAPTER_ID, chapterID.ToString());
        eventProps.Add(AnalyticsKey.P_LEVEL_ID, levelID.ToString());
        eventProps.Add(AnalyticsKey.P_VALUE, chapterID + "_" + levelID);
        Amplitude.Instance.logEvent(AnalyticsKey.LEVEL_WIN, eventProps);
    }

/*
    关卡复活  TGDEvent.Instance.onLvRevive	string	lv_type
    复活方式：参考 TGCString.cs 对应枚举项

    【视频复活】TGCString.lvRevive_video.ToString() 
    
    string	lv_node
    复活前的失败原因
    定义同【关卡失败】
    
    int	lv_wave
    当前波次
    定义同【关卡失败】

    string	lv_id	关卡名称。定义同【关卡开始】
*/
    public void OnLogLevelRevive(LevelType levelType, int chapterID, int levelID, int currentWave)
    {
        if(levelType == LevelType.Main)
        {
            /*
            TGDEvent.Instance.onLvRevive(new Hashtable()
            {
                { "lv_type", TGCString.lvRevive_video.ToString() },
                { "lv_node", TGCString.lvFailed_outLife.ToString() },
                { "lv_wave", currentWave },
                { "lv_id", chapterID + "_" + levelID }
            });*/
        }

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, levelType.ToString());
        eventProps.Add(AnalyticsKey.P_CHAPTER_ID, chapterID.ToString());
        eventProps.Add(AnalyticsKey.P_LEVEL_ID, levelID.ToString());
        eventProps.Add(AnalyticsKey.P_WAVE, currentWave.ToString());
        eventProps.Add(AnalyticsKey.P_VALUE, chapterID + "_" + levelID);
        Amplitude.Instance.logEvent(AnalyticsKey.LEVEL_REVIVE, eventProps);
    }

    //退出关卡打点
    public void OnLogLevelQuit(LevelType levelType, int chapterID, int levelID, int currentWave)
    {
        /*
        int	lv_wave
        string	lv_id
        */
        if(levelType == LevelType.Main)
        {
            /*
            TGDEvent.Instance.onLvQuit(new Hashtable()
            {
                { "lv_wave", currentWave },
                { "lv_id", chapterID + "_" + levelID }
            });*/
        }

        if(!IsAmpOn()) return; //如果amp关闭，则不进行打点

        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, levelType.ToString());
        eventProps.Add(AnalyticsKey.P_LEVEL_ID, levelID.ToString());
        eventProps.Add(AnalyticsKey.P_WAVE, currentWave.ToString());
        Amplitude.Instance.logEvent(AnalyticsKey.LEVEL_QUIT, eventProps);
    }
    #endregion

    #region Account
    //成功登入
    public void OnLogAccountLogin(AccountType accountType)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, accountType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_LOGIN, eventProps);
    }
    //成功登出
    public void OnLogAccountLogout(AccountType accountType)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, accountType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_LOGOUT, eventProps);
    }
    //账户绑定
    public void OnLogAccountLink(AccountType accountType)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, accountType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_LINK, eventProps);
    }
    //账户解绑
    public void OnLogAccountUnLink(AccountType accountType)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, accountType.ToString());
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_UNLINK, eventProps);
    }

    //账户连接错误
    public void OnLogAccountConnectError(AccountType accountType, string error)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_TYPE, accountType.ToString());
        eventProps.Add(AnalyticsKey.P_VALUE, error);
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_CONNECT_ERROR, eventProps);
    }
  
    //删除账户
    public void OnLogAccountDelete(int userID)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_USER_ID, userID);
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_DELETE, eventProps);
    }

    //下载数据
    public void OnLogAccountSyncDownload()
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_SYNC_DOWNLOAD, eventProps);
    }

    //上传数据
    public void OnLogAccountSyncUpload()
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_SYNC_UPLOAD, eventProps);
    }

    //登陆后使用本地数据
    public void OnLogAccountSyncApplyLocal()
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_SYNC_APPLY_LOCAL, eventProps);
    }

    //登陆后使用云端数据
    public void OnLogAccountSyncApplyCloud(string cloudUserID)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_USER_ID, cloudUserID);
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_SYNC_APPLY_CLOUD, eventProps);
    }

    //账户同步错误
    public void OnLogAccountSyncError(string error)
    {
        Dictionary<string, object> eventProps = new Dictionary<string, object>();
        eventProps.Add(AnalyticsKey.P_VALUE, error);
        //Amplitude.Instance.logEvent(AnalyticsKey.ACCOUNT_SYNC_ERROR, eventProps);
    }

#endregion

/*
    #region Firebase
    // Firebase 首次启动（唯一）
    void OnFirebaseRegister()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.Launch_Event);
    }

    //Firebase 启动打点
    void OnFirebaseLaunch()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.Launch_User);
    }

    //Firebase 首次登陆
    void OnFirebaseLoginFirstTime()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.User_Join_Main_User);
    }

    //Firebase 登陆打点
    void OnFirebaseLogin()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.User_Join_Num_User);
    }

    //Firebase 累计登陆次数打点
    void OnFirebaseRetention()
    {
        for (int i = 0; i < mapingLoginCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.loginCount == mapingLoginCount[i])
            {
                FirebaseAnalytics.LogEvent(loginCountEventFirebase[i]);
                Debug.Log("=== AnalyticsControl: log firebase event: " + loginCountEventFirebase[i]);
            }
        }
    }

    //Firebase 请求广告
    void OnFirebaseRequestAD()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Request);
        Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_Request);
    }

    //Firebase 广告没装填
    void OnFirebaseNotReadyAD()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Not_Ready);
        Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_Not_Ready);
    }

    //Firebase 每日广告次数打点
    void OnFirebaseCompleteAdDaily()
    {
        for (int i = 0; i < mapingAdDailyCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.rewardAdCountDaily == mapingAdDailyCount[i])
            {
                FirebaseAnalytics.LogEvent(adDailyCountEventFirebase[i]);
                Debug.Log("=== AnalyticsControl: log firebase event: " + adDailyCountEventFirebase[i]);
            }
        }
    }

    //Firebase 完成广告播放打点
    void OnFirebaseCompleteAD()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Event);
        //FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Event, AnalyticsKey.P_VALUE, GameData.userData.userAnalytics.rewardAdCount.ToString());
        Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_Event);
    }

    // Firebase 完成广告播放的用户数（每日刷新）
    void OnFirebaseCompleteAdUser()
    {
        if (!GameData.userData.userAnalytics.isCompleteAdsDaily)
        {
            FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_User);
            Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_User);
        }
    }

    //Firebase 播放广告的次数
    void OnFirebaseShowAD()
    {
        FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Show_Event);
        Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_Show_Event);
    }

    //Firebase 点击广告的用户
    void OnFirebaseShowADUser()
    {
        //当日是否点击了广告，每日只要打一次
        if (!GameData.userData.userAnalytics.isShowAdsDaily)
        {
            FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Show_User);
            Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.RewardAds_Show_User);
        }
    }

    //Firebase 付费打点
    void OnFistbaseDailyIAPUser()
    {
        //用户当日每付费才触发时间
        if (!GameData.userData.userAnalytics.isPayDaily)
        {
            FirebaseAnalytics.LogEvent(AnalyticsKey.IAP_User);
            Debug.Log("=== AnalyticsControl: log firebase event: " + AnalyticsKey.IAP_User);
        }
    }

    //Firebase 通关关卡
    void OnFirebaseChapterCompleteX(int chapterID)
    {
        //Chapter_Complete_x事件，后面chapter id会根据实际章节变化
        string eventName = AnalyticsKey.Chapter_Complete_X + chapterID;
        FirebaseAnalytics.LogEvent(eventName);
        Debug.Log("=== AnalyticsControl: log firebase event: " + eventName);
    }
    #endregion

    #region Meta
    // Meta 首次启动（唯一）
    void OnMetaRegister()
    {
        //留2秒给FB初始化
        StartCoroutine(TimerTick.Start(2, () =>
        {
            if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.Launch_Event);
            else Debug.Log("=== AnalyticsControl: log meta event failed: " + AnalyticsKey.Launch_Event);
            Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.Launch_Event);
        }));
    }

    // Meta 启动打点
    void OnMetaLaunch()
    {
        //留2秒给FB初始化
        StartCoroutine(TimerTick.Start(2, () =>
        {
            if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.Launch_User);
            else Debug.Log("=== AnalyticsControl: log meta event failed: " + AnalyticsKey.Launch_User);
            Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.Launch_User);
        }));
    }

    // Meta 首次登陆
    void OnMetaLoginFirstTime()
    {
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.User_Join_Main_User);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.User_Join_Main_User);
    }

    // Meta 登陆打点
    void OnMetaLogin()
    {
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.User_Join_Num_User);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.User_Join_Num_User);
    }

    // Meta 累计登陆次数打点
    void OnMetaRetention()
    {
        for (int i = 0; i < mapingLoginCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.loginCount == mapingLoginCount[i])
            {
                if(FB.IsInitialized) FB.LogAppEvent(loginCountEventFirebase[i]);
                Debug.Log("=== AnalyticsControl: log meta event: " + loginCountEventFirebase[i]);
            }
        }
    }

    // Meta 请求广告
    void OnMetaRequestAD()
    {
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_Request);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_Request);
    }

    // Meta 广告没装填
    void OnMetaNotReadyAD()
    {
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_Not_Ready);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_Not_Ready);
    }

    // Meta 每日广告次数打点
    void OnMetaCompleteADDaily()
    {
        for (int i = 0; i < mapingAdDailyCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.rewardAdCountDaily == mapingAdDailyCount[i])
            {
                if(FB.IsInitialized) FB.LogAppEvent(adDailyCountEventFirebase[i]);
                Debug.Log("=== AnalyticsControl: log meta event: " + adDailyCountEventFirebase[i]);
            }
        }
    }

    // Meta 完成广告播放打点
    void OnMetaCompleteAD()
    {
        //FirebaseAnalytics.LogEvent(AnalyticsKey.RewardAds_Event, AnalyticsKey.P_VALUE, GameData.userData.userAnalytics.rewardAdCount.ToString());
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_Event);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_Event);
    }

    // Meta 完成广告播放的用户数（每日刷新）
    void OnMetaCompleteAdUser()
    {
        if (!GameData.userData.userAnalytics.isCompleteAdsDaily)
        {
            if(FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_User);
            Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_User);
        }
    }

    // Meta 播放广告的次数
    void OnMetaShowAD()
    {
        if (FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_Show_Event);
        Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_Show_Event);
    }

    // Meta 点击广告的用户
    void OnMetaShowADUser()
    {
        //当日是否点击了广告，每日只要打一次
        if (!GameData.userData.userAnalytics.isShowAdsDaily)
        {
            if(FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.RewardAds_Show_User);
            Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.RewardAds_Show_User);
        }
    }

    // Meta 付费点位
    void OnMetaIAP(float revenue)
    {
        var iapParameters = new Dictionary<string, object>();
        if(FB.IsInitialized) FB.LogPurchase(revenue, "USD", iapParameters);
    }

    // Meta 付费打点
    void OnMetaDailyIAPUser()
    {
        //用户当日每付费才触发时间
        if (!GameData.userData.userAnalytics.isPayDaily)
        {
            if(FB.IsInitialized) FB.LogAppEvent(AnalyticsKey.IAP_User);
            Debug.Log("=== AnalyticsControl: log meta event: " + AnalyticsKey.IAP_User);
        }
    }

    // Meta 通关关卡
    void OnMetaChapterCompleteX(int chapterID)
    {
        //Chapter_Complete_x事件，后面chapter id会根据实际章节变化
        string eventName = AnalyticsKey.Chapter_Complete_X + chapterID;
        if(FB.IsInitialized) FB.LogAppEvent(eventName);
        Debug.Log("=== AnalyticsControl: log firebase event: " + eventName);
    }
    #endregion

    #region Adjust
    //Adjust 首次启动
    void OnAdjustRegister()
    {
        AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_Launch_User);
        Adjust.TrackEvent(adjustEvent);
        Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_Launch_User);
    }

    //Adjust 启动打点
    void OnAdjustLaunch()
    {
        AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_Launch_Event);
        Adjust.TrackEvent(adjustEvent);
        Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_Launch_Event);
    }

    //Adjust 首次登陆
    void OnAdjustLoginFirstTime()
    {
        AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_User_Join_Main_User);
        Adjust.TrackEvent(adjustEvent);
        Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_User_Join_Main_User);
    }

    //Adjust 登陆次数打点（暂无）
    void OnAdjustLogin()
    {

    }

    //Adjust 留存打点
    void OnAdjustRetention()
    {
        for (int i = 0; i < mapingLoginCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.loginCount == mapingLoginCount[i])
            {
                AdjustEvent adjustEvent = new AdjustEvent(loginCountEventAdjust[i]);
                Adjust.TrackEvent(adjustEvent);
                Debug.Log("=== AnalyticsControl: log adjust event: " + loginCountEventAdjust[i]);
            }
        }
    }

    //Adjust 每日广告次数打点
    void OnAdjustDailyAD()
    {
        for (int i = 0; i < mapingAdDailyCount.Length; i++)
        {
            if (GameData.userData.userAnalytics.rewardAdCountDaily == mapingAdDailyCount[i])
            {
                AdjustEvent adjustEvent = new AdjustEvent(adDailyCountEventAdjust[i]);
                Adjust.TrackEvent(adjustEvent);
                Debug.Log("=== AnalyticsControl: log adjust event: " + adDailyCountEventAdjust[i]);
            }
        }
    }

    //Adjust 完成广告播放打点
    void OnAdjustTotalAD()
    {
        AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_RewardAds_Event);
        Adjust.TrackEvent(adjustEvent);
        Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_RewardAds_Event);
    }

    void OnAdjustIAP(float revenue)
    {
        AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_IAP_Event);
        adjustEvent.SetRevenue(revenue, "USD");
        Adjust.TrackEvent(adjustEvent);
        Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_IAP_Event);
    }

    //Adjust 用户当日付费,每日刷新
    void OnAdjustIAPDaily()
    {
        //用户当日每付费才触发时间
        if (!GameData.userData.userAnalytics.isPayDaily)
        {
            AdjustEvent adjustEvent = new AdjustEvent(AnalyticsKey.Adjust_IAP_User);
            Adjust.TrackEvent(adjustEvent);
            Debug.Log("=== AnalyticsControl: log adjust event: " + AnalyticsKey.Adjust_IAP_User);
        } 
    }
    #endregion
*/

}

public enum AdjustAttributionStep
{
    UserAttributionObtained,
    FacebookInstallReferrerObtained,
    FacebookInstallReferrerNotExist,
    CampaignNameAeoDetected,
    PaidUserDetected,
    OrganicUserDetected,
    AttributionInfoNotExist
}