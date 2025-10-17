using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// 法阵物品数据类
/// </summary>
[System.Serializable]
public class FormationItemConfig
{
    [Header("基础信息")]
    public string itemName;
    public string displayName;
    public string info;
    public FormationItemType itemType;
    public int level = 1;
    public int coinCost = 0;
    public Rarity rarity = Rarity.Common;

    [Header("合成设置")]
    public bool canUpgrade = true;        // 是否可以升级
    public int maxLevel = 5;              // 最大等级

    [Header("是否通过广告获取")]
    public bool AdGet = false;

    [Header("触发设置")]
    public int requiredChargeCount = 1;  // 需要触发的次数
    public bool hasCooldown = false;      // 是否有冷却
    public float cooldownTime = 0f;       // 冷却时间（秒）
    public int energyConsumption = 0;     // 消耗能量


    [Header("效果配置")]
    public List<FormationEffectData> effects = new List<FormationEffectData>();

    [Header("显示设置")]
    public GameObject visualEffectPrefab;
    public Sprite itemIcon;
}
/// <summary>
/// 法阵效果数据结构
/// </summary>
[System.Serializable]
public class FormationEffectData
{
    [Header("效果基础信息")]
    public EffectType effectType;
    public float value;           // 效果数值
}
