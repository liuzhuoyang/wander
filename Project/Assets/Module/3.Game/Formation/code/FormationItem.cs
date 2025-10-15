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
    [SerializeField] private bool isTriggered = false;
    [SerializeField] private int level;

    [Header("触发计数")]
    [SerializeField] private int requiredTriggerCount = 1;  // 需要触发的次数
    [SerializeField] private int currentTriggerCount = 0;   // 当前触发次数
    [SerializeField] private bool autoResetOnComplete = true;  // 完成后是否自动重置计数
    
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
    /// 是否已被触发
    /// </summary>
    public bool IsTriggered => isTriggered;

    /// <summary>
    /// 关联的节点
    /// </summary>
    public FormationNode ParentNode => parentNode;

    /// <summary>
    /// 需要触发的次数
    /// </summary>
    public int RequiredTriggerCount => requiredTriggerCount;

    /// <summary>
    /// 当前触发次数
    /// </summary>
    public int CurrentTriggerCount => currentTriggerCount;

    /// <summary>
    /// 是否完成后自动重置
    /// </summary>
    public bool AutoResetOnComplete => autoResetOnComplete;
    
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
        isTriggered = false;
        
        // 触发设置
        requiredTriggerCount = config.requiredTriggerCount;
        currentTriggerCount = 0;
        autoResetOnComplete = config.autoResetOnComplete;
        
        // 冷却设置
        hasCooldown = config.hasCooldown;
        cooldownTime = config.cooldownTime;
        isInCooldown = false;
        lastTriggerTime = 0f;
        
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
        // 设置图标
        if (config != null && config.itemIcon != null && itemSprite != null)
        {
            itemSprite.sprite = config.itemIcon;
        }
        
        // 创建视觉效果
        if (config != null && config.visualEffectPrefab != null)
        {
            visualEffect = Instantiate(config.visualEffectPrefab, transform);
            visualEffect.SetActive(false);
        }
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

        // 检查是否已完成且不自动重置
        if (isTriggered && !autoResetOnComplete) return;

        currentTriggerCount++;
        lastTriggerTime = Time.time;

        Debug.Log($"物品 {itemName} 被 {triggerer.name} 触发 ({currentTriggerCount}/{requiredTriggerCount})");

        // 通知进度更新
        OnTriggerProgress?.Invoke(currentTriggerCount, requiredTriggerCount);

        // 检查是否达到触发条件
        bool shouldExecute = false;

        if (currentTriggerCount >= requiredTriggerCount)
        {
            shouldExecute = true;
            isTriggered = true;
            OnTriggerComplete?.Invoke();

            // 开始冷却
            if (hasCooldown)
            {
                isInCooldown = true;
            }

            // 自动重置计数
            if (autoResetOnComplete)
            {
                currentTriggerCount = 0;
                isTriggered = false;
            }
        }

        // 执行效果
        if (shouldExecute)
        {
            ExecuteEffects(triggerer);
        }
        else
        {
            // 执行部分效果（如果配置了中途触发）
            ExecutePartialEffects(triggerer);
        }

        // 播放触发视觉效果
        PlayTriggerEffect();
    }

    /// <summary>
    /// 处理武器触发
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void HandleWeaponTrigger(GameObject triggerer)
    {
        Debug.Log($"武器 {itemName} 被触发，给予 {triggerer.name} 武器效果");
        // TODO: 实现武器效果逻辑
    }

    /// <summary>
    /// 处理技能触发
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void HandleSkillTrigger(GameObject triggerer)
    {
        Debug.Log($"技能 {itemName} 被触发，给予 {triggerer.name} 技能效果");
        // TODO: 实现技能效果逻辑
    }

    /// <summary>
    /// 执行完整效果
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void ExecuteEffects(GameObject triggerer)
    {
        foreach (var effect in effects)
        {
            if (effect.requireFullCount)
            {
                FormationEffectManager.Instance.ExecuteEffect(effect, triggerer);
                Debug.Log($"执行完整效果: {effect.effectType} 给 {triggerer.name}");
            }
        }
    }

    /// <summary>
    /// 执行部分效果
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void ExecutePartialEffects(GameObject triggerer)
    {
        foreach (var effect in effects)
        {
            if (!effect.requireFullCount && currentTriggerCount >= effect.triggerThreshold)
            {
                FormationEffectManager.Instance.ExecuteEffect(effect, triggerer);
                Debug.Log($"执行部分效果: {effect.effectType} 给 {triggerer.name}");
            }
        }
    }


    /// <summary>
    /// 播放触发视觉效果
    /// </summary>
    private void PlayTriggerEffect()
    {
        if (visualEffect != null)
        {
            visualEffect.SetActive(true);
            // TODO: 播放特效动画
        }
    }

    /// <summary>
    /// 重置物品状态
    /// </summary>
    public void Reset()
    {
        isTriggered = false;
        currentTriggerCount = 0;
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
    Weapon,  // 武器
    Skill,   // 技能
}

/// <summary>
/// 效果类型枚举
/// </summary>
public enum EffectType
{
    DirectionReverse,   // 反转方向
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
    public int requiredTriggerCount = 1;  // 需要触发的次数
    public bool hasCooldown = false;      // 是否有冷却
    public float cooldownTime = 0f;       // 冷却时间（秒）
    public bool autoResetOnComplete = true; // 完成后是否自动重置计数
    
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
    public string targetTag;      // 目标标签

    [Header("条件设置")]
    public bool requireFullCount; // 是否需要完整计数才生效
    public int triggerThreshold;  // 触发阈值（部分效果可能中途触发）
}
