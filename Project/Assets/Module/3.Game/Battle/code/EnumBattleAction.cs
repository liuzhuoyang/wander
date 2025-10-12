
//局内事件
public enum BattleActionType
{
    None = 0,
    //战斗事件
    EnemyKilled = 101,            //击杀时间,不区分Boss和普通敌人，通过传入参数判断
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