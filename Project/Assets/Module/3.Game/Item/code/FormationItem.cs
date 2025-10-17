using UnityEngine;

//实际参数
public class FormationItem
{
    public FormationItemConfig itemConfig;
    public void Init(FormationItemConfig config)
    {
        itemConfig = config;
        itemConfig = config;
        itemName = config.itemName;
        itemType = config.itemType;
        level = config.level;

        // 触发设置
        requiredChargeCount = config.requiredChargeCount;
        currentChargeCount = 0;
        isActivated = false;
        rarity = config.rarity;

        // 冷却设置
        hasCooldown = config.hasCooldown;
        cooldownTime = config.cooldownTime;
        isInCooldown = false;
        lastTriggerTime = 0f;

        // 充能消耗设置
        requiredEnergyConsumption = config.energyConsumption;
        energyConsumption = 0;

    }


    public string itemName;
    public FormationItemType itemType;
    public int level;

    [Header("充能计数")]
    public int requiredChargeCount = 1;  // 需要触发的次数
    public int currentChargeCount = 0;   // 当前触发次数

    public bool isActivated = false;     // 是否激活

    [Header("充能消耗")]
    public int requiredEnergyConsumption = 0;     // 需要消耗的能量
    public int energyConsumption = 0;              // 消耗能量

    [Header("冷却系统")]
    public bool hasCooldown = false;      // 是否有冷却
    public float cooldownTime = 0f;       // 冷却时间
    public float lastTriggerTime = 0f;    // 上次触发时间
    public bool isInCooldown = false;     // 是否在冷却中
    public Rarity rarity = Rarity.Common; // 稀有度

}
