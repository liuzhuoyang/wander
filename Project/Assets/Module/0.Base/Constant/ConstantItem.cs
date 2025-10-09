public class ConstantItem
{
    //战斗，战斗物品只在战斗内使用，不会进入背包数据（或者进入背包数据但每次战斗结束后，战斗开始时候重置）
    public const string BATTLE_EXP = "item_battle_exp";          //战斗经验
    public const string BATTLE_TOKEN = "item_battle_token";      //战斗代币（银币）
    //货币
    public const string COIN = "item_currency_coin";
    public const string GEM = "item_currency_gem";
    public const string ENERGY = "item_currency_energy";

    //Token
    public const string TOKEN_TAVERN = "item_token_tavern";
    public const string TOKEN_TICKET_AD = "item_token_ticket_ad";

    //Key
    public const string KEY_ARENA = "item_key_arena";
    public const string KEY_DUNGEON_1 = "item_key_dungeon_1";
    public const string KEY_DUNGEON_2 = "item_key_dungeon_2";
    public const string KEY_TOWER = "item_key_tower";

    //Point
    public const string POINT_TASK = "item_point_task";

    //Pass
    public const string PASS_SEASON = "item_pass_season";
}
