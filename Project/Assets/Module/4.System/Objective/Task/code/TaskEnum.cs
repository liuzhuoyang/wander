public enum TaskType
{
    None,
    DailyTask,
    WeeklyTask,
    Achievenment,
    OnBoardingTask
}

public enum TaskActionType
{
    None,
    //一般
    Login = 101,      //登陆
    CompleteAd = 102, //看广告
    BuyItem = 103,    //购买非内购物品  
    TavernDraw = 105, //酒馆抽卡
    UseEnergy = 106,  //使用体力

    //Meta 养成  
    GearUpgrade = 201,//装备升级
    TalentUpgrade = 204,//升级天赋

    //关卡
    PassLevel       = 302,  //通关章节
    StartDungeon    = 303,  //通关地牢 

    //战斗内
    Kill = 401,             //击杀敌人(战斗)
    Merge = 402,            //合成(战斗)
    EarnBattleToken = 403,  //赚取的战斗币数量(战斗)
}
