using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 法阵节点上的物品组件，负责处理触发逻辑和显示效果
/// </summary>
public class FormationItem : MonoBehaviour
{
    [Header("物品信息")]
    [SerializeField] private string itemName;
    [SerializeField] private FormationItemType itemType;
    [SerializeField] private int level;

    [Header("充能计数")]
    [SerializeField] private int requiredChargeCount = 1;  // 需要触发的次数
    [SerializeField] private int currentChargeCount = 0;   // 当前触发次数

    [SerializeField] private bool isActivated = false;     // 是否激活

    [Header("充能消耗")]
    [SerializeField] private int requiredEnergyConsumption = 0;     // 需要消耗的能量
    [SerializeField] private int energyConsumption = 0;     // 消耗能量

    [Header("冷却系统")]
    [SerializeField] private bool hasCooldown = false;      // 是否有冷却
    [SerializeField] private float cooldownTime = 0f;       // 冷却时间
    [SerializeField] private float lastTriggerTime = 0f;    // 上次触发时间
    [SerializeField] private bool isInCooldown = false;     // 是否在冷却中

    [Header("效果配置")]
    [SerializeField] private List<FormationEffectData> effects = new List<FormationEffectData>();

    [Header("显示设置")]
    [SerializeField] private GameObject visualEffect;
    [SerializeField] private SpriteRenderer itemSprite;

    // 关联的节点引用
    private FormationNode parentNode;

    // 触发进度事件
    public System.Action<int, int> OnTriggerProgress; // (当前次数, 需要次数)
    public System.Action OnTriggerComplete;           // 触发完成

    /// <summary>
    /// 物品名称
    /// </summary>
    public string ItemName => itemName;

    /// <summary>
    /// 物品类型
    /// </summary>
    public FormationItemType ItemType => itemType;

    /// <summary>
    /// 关联的节点
    /// </summary>
    public FormationNode ParentNode => parentNode;

    /// <summary>
    /// 需要触发的次数
    /// </summary>
    public int RequiredTriggerCount => requiredChargeCount;

    /// <summary>
    /// 当前触发次数
    /// </summary>
    public int CurrentTriggerCount => currentChargeCount;


    /// <summary>
    /// 是否有冷却
    /// </summary>
    public bool HasCooldown => hasCooldown;

    /// <summary>
    /// 冷却时间
    /// </summary>
    public float CooldownTime => cooldownTime;

    /// <summary>
    /// 是否在冷却中
    /// </summary>
    public bool IsInCooldown => isInCooldown;

    /// <summary>
    /// 剩余冷却时间
    /// </summary>
    public float RemainingCooldownTime => hasCooldown ? Mathf.Max(0f, cooldownTime - (Time.time - lastTriggerTime)) : 0f;

    /// <summary>
    /// 效果列表
    /// </summary>
    public List<FormationEffectData> Effects => effects;

    /// <summary>
    /// 初始化物品
    /// </summary>
    /// <param name="config">物品配置</param>
    /// <param name="node">关联的节点</param>
    public void Initialize(FormationItemConfig config, FormationNode node)
    {
        itemName = config.itemName;
        itemType = config.itemType;
        level = config.level;
        parentNode = node;

        // 触发设置
        requiredChargeCount = config.requiredChargeCount;
        currentChargeCount = 0;
        isActivated = false;

        // 冷却设置
        hasCooldown = config.hasCooldown;
        cooldownTime = config.cooldownTime;
        isInCooldown = false;
        lastTriggerTime = 0f;

        // 充能消耗设置
        requiredEnergyConsumption = config.energyConsumption;
        energyConsumption = 0;

        // 效果配置
        effects.Clear();
        effects.AddRange(config.effects);

        // 设置GameObject名称
        gameObject.name = $"FormationItem_{itemName}_{itemType}";

        // 初始化显示
        InitializeVisuals(config);
    }

    /// <summary>
    /// 初始化视觉效果
    /// </summary>
    private void InitializeVisuals(FormationItemConfig config = null)
    {

    }


    /// <summary>
    /// 触发物品效果
    /// </summary>
    /// <param name="triggerer">触发者（人物）</param>
    public void Trigger(GameObject triggerer)
    {
        // 检查冷却状态
        if (hasCooldown && isInCooldown)
        {
            float remainingTime = RemainingCooldownTime;
            if (remainingTime > 0)
            {
                Debug.Log($"物品 {itemName} 还在冷却中，剩余 {remainingTime:F1} 秒");
                return;
            }
            else
            {
                // 冷却结束
                isInCooldown = false;
            }
        }

        Debug.Log($"物品 {itemName} 被 {triggerer.name} 触发 ({currentChargeCount}/{requiredChargeCount})");
        lastTriggerTime = Time.time;

        if (isActivated)
        {
            energyConsumption--;
            ExecuteEffects(triggerer);
            if (energyConsumption <= 0)
            {
                if (hasCooldown)
                {
                    isInCooldown = true;
                }
                isActivated = false;
            }
        }
        else
        {
            currentChargeCount++;
            // 通知进度更新
            OnTriggerProgress?.Invoke(currentChargeCount, requiredChargeCount);


            if (currentChargeCount >= requiredChargeCount)
            {
                //激活完成
                OnTriggerComplete?.Invoke();
                currentChargeCount = 0;
                isActivated = true;
                energyConsumption = requiredEnergyConsumption;

                //直接触发
                if (energyConsumption == 0)
                {
                    ExecuteEffects(triggerer);

                    // 开始冷却
                    if (hasCooldown)
                    {
                        isInCooldown = true;
                    }
                }
            }
        }

        // 播放触发视觉效果
        RefreshShow();
    }

    /// <summary>
    /// 执行完整效果
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void ExecuteEffects(GameObject triggerer)
    {
        foreach (var effect in effects)
        {
            FormationEffectManager.Instance.ExecuteEffect(effect, triggerer);
            
            Debug.Log($"执行完整效果: {effect.effectType} 给 {triggerer.name}");

        }
    }



    /// <summary>
    /// 播放触发视觉效果
    /// </summary>
    private void RefreshShow()
    {

    }

    /// <summary>
    /// 重置物品状态
    /// </summary>
    public void Reset()
    {
        currentChargeCount = 0;
        isInCooldown = false;
        lastTriggerTime = 0f;
        InitializeVisuals();

        if (visualEffect != null)
        {
            visualEffect.SetActive(false);
        }
    }

}

/// <summary>
/// 法阵物品类型枚举
/// </summary>
public enum FormationItemType
{
    Gear,  // 武器
    Skill,   // 技能
}

/// <summary>
/// 效果类型枚举
/// </summary>
public enum EffectType
{
    ReverseDirection,   // 反转方向
    Health,             // 恢复血量
    Shield,             // 护盾
    Coin,               // 金币奖励
    SpeedBoost,         // 加速
    Dash,               // 冲刺
    DoubleJump,         // 双跳
    Clone,              // 复制
}

/// <summary>
/// 法阵物品配置类
/// </summary>
[System.Serializable]
public class FormationItemConfig
{
    [Header("基础信息")]
    public string itemName;
    public FormationItemType itemType;
    public int level = 1;

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
    public float duration;        // 持续时间（如果需要）
}
