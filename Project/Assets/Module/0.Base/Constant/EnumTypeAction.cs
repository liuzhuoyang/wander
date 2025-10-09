
public enum ActionType
{
    None = 0,
    //系统事件
    Login = 101,                  //玩家登陆账号    
    CompleteAD = 102,             //看完广告
    DailyLogin = 103,             //每日登陆
    UseEnergy = 104,              //使用体力
    ChangeProfile = 105,          //更换头像等
    IAPComplete = 106,            //内购完成
    CompleteOpeningBattle = 107,  //完成开场战斗
    Register = 108,               //注册账号
    UseItem = 109,                //使用物品
    OpenCheat = 110,           //作弊
    UpdateGroupAB = 111,          //更新AB测试

    //UI事件
    OnBackToLobby = 200,          //从任意界面返回Lobby
    OnRewardClosed = 203,         //领取奖励完成

    //玩法事件
    Draw = 301,                   //抽卡
    ItemReward = 303,             //获取奖励物品
    SummonGearSuccess = 304,      //召唤装备成功(包含动画结束)
    GearUpgrade = 306,            //装备升级成功
    GearUpdate = 307,             //装备数据更新
    TalentUpgrade = 309,          //天赋升级成功
    GearUnlock = 311,             //装备解锁(包含解锁popup的关闭)
    ArenaDataUpdate = 312,        //竞技场数据更新
    RankUpdate = 314,             //排行榜数据更新
    TaskClaimed = 315,            //任务完成
    FundClaimed = 316,            //基金领取奖励
    ChallengeClaimed = 317,       //挑战领取奖励
    MarketBuy = 318,              //市场购买
    ShopBuy = 319,                //商城购买
    BattleBaseUnlock = 320,       //战斗基地解锁
    RelicUpgrade = 321,           //遗物升级
    RelicUnlock = 322,           //遗物解锁
    EnergyBuy = 323,            //购买体力
    TavernRefresh = 324,        //酒馆刷新
    UpdatePromo = 325,          //更新礼包
    UpdateLiveEvent = 326,      //更新活动
    GetLiveEventToken = 327,    //获取活动token
    RankingLike = 328,          //排行榜点赞
    EnergyRecover = 329,        //体力恢复（包含离线恢复和自动恢复）
    MailDataUpdate = 331,      //邮件数据更新
    TowerDataUpdate = 332,      //塔数据更新

    //关卡事件
    LevelStart = 401,             //关卡开始
    LevelEnd = 402,               //关卡结束
    UnlockChapter = 405,          //解锁章节
    DungeonStart = 406,           //地下城进入
    DungeonEnd = 407,             //地下城结束
    ArenaStart = 408,             //竞技场开始
    ArenaEnd = 409,               //竞技场结束
    TowerStart = 410,             //塔开始
    TowerEnd = 411,               //塔结束

    //序列事件节点
    OnSeqTaskComplete = 501,      //序列任务完成
}

//局内事件
public enum BattleActionType
{
    None = 0,
    //战斗事件
    EnemyKilled = 101,              //击杀时间,不区分Boss和普通敌人，通过传入参数判断
    PlayerDead = 102,             //玩家死亡
    FriendDead = 103,             //友方单位死亡

    //流程事件
    WaveStart = 201,              //波段开始
    WaveEnd = 202,                //波段结束
    OnFight = 203,                //进入战斗
    LevelUp = 204,                //升级获取能力

    //合成页面事件
    GearMoveSuccess = 301,        //拖入装备成功
}