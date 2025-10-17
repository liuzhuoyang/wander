using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 法阵节点上的物品组件，负责处理触发逻辑和显示效果
/// </summary>
public class FormationWorldItem : MonoBehaviour
{
    public FormationItem item;
    public UIBattleItemSlot uIBattleItemSlot;



    [Header("效果配置")]
    [SerializeField] private List<FormationEffectData> effects = new List<FormationEffectData>();


    // 关联的节点引用
    private FormationNode parentNode;

    // 触发进度事件
    public System.Action<int, int> OnTriggerProgress; // (当前次数, 需要次数)
    public System.Action OnTriggerComplete;           // 触发完成

    /// <summary>
    /// 物品名称
    /// </summary>
    public string ItemName => item.itemName;

    /// <summary>
    /// 物品类型
    /// </summary>
    public FormationItemType ItemType => item.itemType;

    /// <summary>
    /// 物品等级
    /// </summary>
    public int Level => item.level;

    /// <summary>
    /// 关联的节点
    /// </summary>
    public FormationNode ParentNode => parentNode;

    /// <summary>
    /// 需要触发的次数
    /// </summary>
    public int RequiredTriggerCount => item.requiredChargeCount;

    /// <summary>
    /// 当前触发次数
    /// </summary>
    public int CurrentTriggerCount => item.currentChargeCount;


    /// <summary>
    /// 是否有冷却
    /// </summary>
    public bool HasCooldown => item.hasCooldown;

    /// <summary>
    /// 冷却时间
    /// </summary>
    public float CooldownTime => item.cooldownTime;

    /// <summary>
    /// 是否在冷却中
    /// </summary>
    public bool IsInCooldown => item.isInCooldown;

    /// <summary>
    /// 剩余冷却时间
    /// </summary>
    public float RemainingCooldownTime => item.hasCooldown ? Mathf.Max(0f, item.cooldownTime - (Time.time - item.lastTriggerTime)) : 0f;

    /// <summary>
    /// 效果列表
    /// </summary>
    public List<FormationEffectData> Effects => effects;

    /// <summary>
    /// 初始化物品
    /// </summary>
    /// <param name="config">物品配置</param>
    /// <param name="node">关联的节点</param>
    public void Initialize(FormationItem formationItem, FormationNode node)
    {
        item = formationItem;

        uIBattleItemSlot.Init(formationItem);
        // 效果配置
        effects.Clear();
        effects.AddRange(item.itemConfig.effects);

        // 设置GameObject名称
        gameObject.name = $"FormationItem_{item.itemName}_{item.itemType}";

    }


    /// <summary>
    /// 触发物品效果
    /// </summary>
    /// <param name="triggerer">触发者（人物）</param>
    public void Trigger(GameObject triggerer)
    {
        // 检查冷却状态
        if (item.hasCooldown && item.isInCooldown)
        {
            float remainingTime = item.hasCooldown ? Mathf.Max(0f, item.cooldownTime - (Time.time - item.lastTriggerTime)) : 0f;
            if (remainingTime > 0)
            {
                Debug.Log($"物品 {item.itemName} 还在冷却中，剩余 {remainingTime:F1} 秒");
                return;
            }
            else
            {
                // 冷却结束
                item.isInCooldown = false;
            }
        }

        Debug.Log($"物品 {item.itemName} 被 {triggerer.name} 触发 ({item.currentChargeCount}/{item.requiredChargeCount})");
        item.lastTriggerTime = Time.time;

        if (item.isActivated)
        {
            item.energyConsumption--;
            ExecuteEffects(triggerer);
            if (item.energyConsumption <= 0)
            {
                if (item.hasCooldown)
                {
                    item.isInCooldown = true;
                }
                item.isActivated = false;
            }
        }
        else
        {
            item.currentChargeCount++;
            // 通知进度更新
            OnTriggerProgress?.Invoke(item.currentChargeCount, item.requiredChargeCount);


            if (item.currentChargeCount >= item.requiredChargeCount)
            {
                //激活完成
                OnTriggerComplete?.Invoke();
                item.currentChargeCount = 0;
                item.isActivated = true;
                item.energyConsumption = item.requiredEnergyConsumption;

                //直接触发
                if (item.energyConsumption == 0)
                {
                    ExecuteEffects(triggerer);

                    // 开始冷却
                    if (item.hasCooldown)
                    {
                        item.isInCooldown = true;
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
        uIBattleItemSlot.RefreshSetBar();
    }

    /// <summary>
    /// 重置物品状态
    /// </summary>
    public void Reset()
    {
        item.currentChargeCount = 0;
        item.isInCooldown = false;
        item.lastTriggerTime = 0f;
    }

    /// <summary>
    /// 升级物品等级
    /// </summary>
    public void UpgradeLevel()
    {
        item.level++;
        Debug.Log($"物品 {item.itemName} 升级到等级 {item.level}");
    }



    /// <summary>
    /// 显示物品
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏物品
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
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



