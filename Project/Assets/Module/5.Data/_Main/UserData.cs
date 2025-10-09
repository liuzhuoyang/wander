
public class UserData
{
    #region Account
    public UserAccount userAccount;
    public UserServer userServer;
    public UserProgress userProgress; //用户进度
    #endregion

    #region Progression
    
    #endregion

    #region User
    public UserTag userTag;
    #endregion

    #region 2.Generic
    public UserItem userItem;
    public UserStats userStats;
    public UserEnergy userEnergy;
    #endregion

    #region 4.System - GamePlay
    public UserLevel userLevel;
    public UserTower userTower;
    public UserDungeon userDungeon;
    #endregion

    #region 4.System - Objective
    public UserTask userTask;
    public UserSequenceTask userSequenceTask;
    public UserAchievement userAchievement;
    public UserCheckIn userCheckIn;
    #endregion

    #region 4.System - Meta
    public UserGear userGear;
    public UserTalent userTalent;
    public UserLoot userLoot;
    public UserRelic userRelic;
    public UserPet userPet;
    #endregion

    #region 4.System - Social
    public UserProfile userProfile;
    public UserAvatar userAvatar;
    #endregion

    #region 4.System - Idle
    public UserAFK userAFK;
    #endregion
 
    #region 4.System - Monetization
    public UserShop userShop;
    public UserMarket userMarket;
    public UserFund userFund;
    public UserPrivilege userPrivilege;
    #endregion

    #region 4.System - Terminal
    public UserRoadmap userRoadmap;
    public UserHandbook userHandbook;
    #endregion

    #region 5.Data
    public UserAd userAd;
    #endregion

    #region 7.SDK
    public UserAnalytics userAnalytics;
    #endregion

    #region 10.Live Event
    public UserLiveEvent userLiveEvent;
    #endregion
    
    #region 11.Misc
    public UserMisc userMisc;
    #endregion

    public void Init()
    {
        //Account
        userAccount = new UserAccount();
        userProgress = new UserProgress();

        //2.Generic
        userItem = new UserItem();
        userStats = new UserStats();
        userEnergy = new UserEnergy();

        //4.System - Gameplay
        userLevel = new UserLevel();
        userDungeon = new UserDungeon();
        userTower = new UserTower();

        //4.System - Social
        userProfile = new UserProfile();
        userAvatar = new UserAvatar();

        //4.System - Meta
        userGear = new UserGear();
        userTalent = new UserTalent();
        userLoot = new UserLoot();
        userRelic = new UserRelic();
        userPet = new UserPet();

        //4.System - Objective
        userTask = new UserTask();
        userSequenceTask = new UserSequenceTask();
        userAchievement = new UserAchievement();
        userCheckIn = new UserCheckIn();

        //4.System - Idel
        userAFK = new UserAFK();

        //7.SDK
        userAnalytics = new UserAnalytics();
  
        //11.Misc
        userMisc = new UserMisc();
    }
}

public class UserProgress
{
    public bool isRegister;   //是否注册
    public bool isFirstGame;

    public UserProgress()
    {
        isRegister = false;
        isFirstGame = true;
    }
}

public class UserMisc
{
    public bool isNewDayLogin;
    public string loginVersion;

    public void ResetDaily()
    {
        isNewDayLogin = true;
    }
}

#region User Tag 用户标签
public class UserTag
{
    public bool isPayUser;                      //付费用户
    public bool isCheatUser;                    //是否是作弊用户
    public UserAcquisitionType userAcquisition; //用户获取方式
    public string installSource;                //安装来源

    public void InitData()
    {
        isCheatUser = false;
    }
}
#endregion
