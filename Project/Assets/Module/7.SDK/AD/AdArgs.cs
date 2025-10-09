public class AdArgs
{

}

public enum AdType
{
    None,
    //1 战斗内
    BattleEndRevive = 101,           //战斗结束复活
 
    //2 游戏流程
    BattleEndBonus = 201,            //2倍奖励        

    //3 外围模块
    MetaMarketExchange = 301,        //市场兑换
  
    //9 插屏
    InterstitalBattleEnd = 901,       //关卡完成,不看2倍广告
}