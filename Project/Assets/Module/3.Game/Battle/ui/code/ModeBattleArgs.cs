using System.Collections.Generic;
using System;
using UnityEngine;
public class EventNameModeBattle
{
    //Mode UI事件
    public const string EVENT_BATTLE_ON_SELECT_UI = "EVENT_BATTLE_ON_SELECT_UI";  //选择Mode UI
    public const string EVENT_BATTLE_ON_CLOSE_UI = "EVENT_BATTLE_ON_CLOSE_UI";    //关闭Mode UI
    public const string EVENT_BATTLE_PREPARE_INIT_UI = "EVENT_BATTLE_PREPARE_INIT_UI";
    public const string EVENT_BATTLE_PREPARE_REFRESH_UI = "EVENT_BATTLE_PREPARE_REFRESH_UI";
    public const string EVENT_BATTLE_FIGHT_INIT_UI = "EVENT_BATTLE_FIGHT_INIT_UI";
    public const string EVENT_BATTLE_REFRESH_ITEM_UI = "EVENT_BATTLE_REFRESH_ITEM_UI";
 
    //注册事件
    public const string EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI = "EVENT_BATTLE_FIGHT_ON_REGISTER_EVENT_UI";
    public const string EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI = "EVENT_BATTLE_FIGHT_ON_UNREGISTER_EVENT_UI";
    public const string EVENT_BATTLE_PREPARE_ON_REGISTER_EVENT_UI = "EVENT_BATTLE_PREPARE_ON_REGISTER_EVENT_UI";
    public const string EVENT_BATTLE_PREPARE_ON_UNREGISTER_EVENT_UI = "EVENT_BATTLE_PREPARE_ON_UNREGISTER_EVENT_UI";

    public const string EVENT_BATTLE_SHOW_HEALTH_UI = "EVENT_BATTLE_SHOW_HEALTH_UI";
    //伤害统计事件
    public const string EVENT_BATTLE_DAMAGE_STATS_INIT = "EVENT_BATTLE_DAMAGE_STATS_INIT";
}

public class UIModeBattleArgs : UIBaseArgs
{
    public string targetPage;
    public string displayArgs;
}

public class UIBattlePrepareArgs : UIBaseArgs
{
    public bool isShowHealthUI;
}

public class UIBattleForgeArgs : UIBaseArgs
{
    public List<Vector2> listPos;
    public int forgeValue;
}

public class UIBattleFightArgs : UIBaseArgs
{
    public bool isActiveSkillOn;
}

public class UIBattleCastArgs : UIBaseArgs
{
    public Action onCancel;
}

public class DamageStatsArgs
{
    public string damageStatsName;
    public float damage;
    public float damagePercent;
}
public class UIDamageStatsArgs : UIBaseArgs
{
    public List<DamageStatsArgs> listDamageStats;
}

public class UIBattleFightAmmoArgs : UIBaseArgs
{
    public int ammoCount;
    public float ReloadSpeed;
}

public class UIBattlePreviewArgs : UIBaseArgs
{
    public Dictionary<string, float> dictEnemyWeight;
    public bool haveBoss;
    public bool haveElite;
}