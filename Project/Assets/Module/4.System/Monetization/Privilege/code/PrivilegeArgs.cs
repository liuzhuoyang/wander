public class EventNamePrivilege
{
    public const string EVENT_PRIVILEGE_INIT_UI = "EVENT_PRIVILEGE_INIT_UI";
}

public class UIPrivilegeArgs : UIBaseArgs
{

}

//特权类型
public enum PrivilegeType
{
    None = 0,
    InterstitalAdFree = 1,//免插屏广告
    BattleCoinBonus = 2,//战斗额外获得酒馆币
}