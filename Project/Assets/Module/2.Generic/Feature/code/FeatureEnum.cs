public enum FeatureType
{
    None        = 0,

    // 0开头为底部导航栏功能 Navigation
    Lobby       = 001,  //大厅
    Gear        = 002,  //装备库
    Talent      = 003,  //天赋
    Headquarter = 004,  //家园
    Shop        = 005,  //商店

    // 1开头为玩法功能
    Battle      = 100,  //战斗
    Tavern      = 101,   //酒馆抽卡
    Dungeon     = 102,  //地牢
    Tower       = 103,

    // 2开头为养成功能

    Pet         = 202,  //宠物
    Relic       = 203,  //遗物
    Loot        = 204,  //宝箱

    // 3开头为社交功能
    Ranking     = 301,  //排行榜
    Arena       = 302,  //竞技场
    Guild       = 303,  //公会

    // 4开始为商业化功能
    Market      = 401,  //市场
    Fund        = 402,  //基金
    Season      = 403,  //赛季
    Privilege   = 404,  //特权
    Armory      = 405,  //军械库
 

    // 5开头为目标功能
    Task        = 501,  //任务
    Achievement = 502,  //成就
    Challenge   = 503,  //挑战
    Check_In    = 504,  //签到

    // 6开头为杂项
    Setting     = 601,  //设置
    Profile     = 602,  //个人中心
    Terminal    = 603,  //终端，汉堡包折叠功能
}

//功能入口被创建的位置类型
public enum FeatureEntranceType
{
    Default         = 0, //预先放置
    TopBar          = 1, //顶部栏
    BottomBar       = 2, //底部栏
    LeftBar         = 3, //左侧栏
    RightBar        = 4, //右侧栏
    BottomRightBar  = 5, //右下角
    BottomLeftBar   = 6, //左下角
    Banner          = 7, //横幅
}

public enum FeatureLockType
{
    Dynamic          = 0, //动态添加
    Static           = 1, //预先放置
}

public enum FeatureUnlockConditionType
{
    None        = 0,
    Level    = 1, //进度
    Coming      = 99, //即将
}