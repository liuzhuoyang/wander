using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleLaunch;
using BattleGear;
using BattleActor;

/// <summary>
/// 法阵节点上的物品组件，负责处理触发逻辑和显示效果
/// </summary>
public class FormationWorldItem : MonoBehaviour
{
    public FormationItem item;
    public UIBattleItemSlot uIBattleItemSlot;



    [Header("效果配置")]
    [SerializeField] private List<FormationEffectData> effects = new List<FormationEffectData>();

    // 武器发射相关组件
    private BattleLaunchTargetFinder targetFinder;
    private BattleLaunchControl launchControl;
    private BattleLaunchCommandData battleLaunchData;


    // 关联的节点引用
    private FormationNode parentNode;

    // 触发进度事件
    public System.Action<int, int> OnTriggerProgress; // (当前次数, 需要次数)
    public System.Action OnTriggerComplete;           // 触发完成

    /// <summary>
    /// 物品名称
    /// </summary>
    public string ItemName => item.ItemName;

    /// <summary>
    /// 物品类型
    /// </summary>
    public FormationItemType ItemType => item.ItemType;

    /// <summary>
    /// 物品等级
    /// </summary>
    public int Level => item.Level;

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
    /// <param name="formationItem">物品实例</param>
    /// <param name="node">关联的节点</param>
    /// <param name="gearData">武器数据（仅武器类型需要）</param>
    public void Initialize(FormationItem formationItem, FormationNode node)
    {
        item = formationItem;
        parentNode = node;
        // this.gearData = gearData;

        if (formationItem.itemConfig.formationGearData != null && formationItem.itemConfig.itemType == FormationItemType.Gear)
        {
            // 如果是武器类型，初始化发射组件
            InitializeGearLauncher();
        }
        uIBattleItemSlot.Init(formationItem);
        // 效果配置
        effects.Clear();
        effects.AddRange(item.itemConfig.effects);



        // 设置GameObject名称
        gameObject.name = $"FormationItem_{item.ItemName}_{item.ItemType}";
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
                Debug.Log($"物品 {item.ItemName} 还在冷却中，剩余 {remainingTime:F1} 秒");
                return;
            }
            else
            {
                // 冷却结束
                item.isInCooldown = false;
            }
        }

        Debug.Log($"物品 {item.ItemName} 被 {triggerer.name} 触发 ({item.currentChargeCount}/{item.requiredChargeCount})");
        item.lastTriggerTime = Time.time;

        if (item.isActivated)
        {
            item.energyConsumption--;
            ActionEffects(triggerer);
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
                    ActionEffects(triggerer);

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
    private void ActionEffects(GameObject triggerer)
    {
        switch (item.ItemType)
        {
            case FormationItemType.Gear:
                ExecuteGearLaunch(triggerer);
                break;
            case FormationItemType.Skill:
                foreach (var effect in effects)
                {
                    FormationEffectManager.Instance.ExecuteEffect(effect, triggerer);
                    Debug.Log($"执行完整效果: {effect.effectType} 给 {triggerer.name}");
                }
                break;
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

        // 清理武器发射组件
        if (item.ItemType == FormationItemType.Gear)
        {
            CleanupGearLauncher();
        }
    }

    /// <summary>
    /// 升级物品等级
    /// </summary>
    public void UpgradeLevel()
    {
        item.Level++;
        Debug.Log($"物品 {item.ItemName} 升级到等级 {item.Level}");
        uIBattleItemSlot.Refresh();
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

    #region 武器发射相关方法

    /// <summary>
    /// 初始化武器发射器
    /// </summary>
    private void InitializeGearLauncher()
    {
        if (item.itemConfig.formationGearData == null)
        {
            Debug.LogError($"FormationGearData 或 gearData 为空: {item.ItemName}");
            return;
        }

        // 初始化目标搜索器
        targetFinder = gameObject.AddComponent<BattleLaunchTargetFinder>();
        targetFinder.Init(TeamMask.Enemy, item.itemConfig.formationGearData.launchConfig.scanOrder, BattleActorMotionLayerMask.All);

        // 初始化发射控制器
        launchControl = gameObject.AddComponent<BattleLaunchControl>();

        // 创建发射数据
        battleLaunchData = new BattleLaunchCommandData(item.itemConfig.formationGearData.launchConfig);

        Debug.Log($"武器发射器初始化完成: {item.ItemName}");
    }

    /// <summary>
    /// 执行武器发射
    /// </summary>
    /// <param name="triggerer">触发者</param>
    private void ExecuteGearLaunch(GameObject triggerer)
    {
        if (targetFinder == null || launchControl == null || battleLaunchData == null)
        {
            Debug.LogError($"武器发射组件未初始化: {item.ItemName}");
            return;
        }

        if (item.itemConfig.formationGearData == null)
        {
            Debug.LogError($"FormationGearData 为空: {item.ItemName}");
            return;
        }

        // 搜索目标
        targetFinder.FlushTarget(99, battleLaunchData.retargetPerCount ? (int)battleLaunchData.burstCount.cachedValue : 1);

        var targets = targetFinder.GetActiveTargetList();

        // 如果索敌列表有目标，则发射
        if (!battleLaunchData.requireTargetToLaunch || targets.Count > 0)
        {
            // 创建新的发射参数
            var currentLaunchBatch = new BattleLaunchCommand_Batch(battleLaunchData,
                                                                GetAttackData(item.itemConfig.formationGearData),
                                                                transform);

            // 目标列表，由于敌人搜索会动态更新敌人列表，此列表必须拷贝，不能引用
            if (battleLaunchData.trackTargetIfCan)
                currentLaunchBatch.AssignTargets(targets);
            else
            {
                List<Vector2> posList = new List<Vector2>();
                foreach (var target in targets)
                {
                    posList.Add(target.position);
                }
                currentLaunchBatch.AssignTargets(posList);
            }

            currentLaunchBatch.ExcludeTeam(BattleActorService.GetOppositeTeam(targetFinder.m_targetTeam));

            // 添加发射命令，并在成功时执行callback
            currentLaunchBatch.OnLaunchBegin(OnLaunchExecute)
                              .OnLaunchEnd(OnLaunchComplete)
                              .OnHitTarget(OnHitTarget);

            //  launchControl.AddLaunch(currentLaunchBatch);
            launchControl.DoLaunch(currentLaunchBatch);
            Debug.Log($"武器 {item.ItemName} 发射，目标数量: {targets.Count}");
        }
        else
        {
            Debug.Log($"武器 {item.ItemName} 没有找到目标");
        }
    }

    /// <summary>
    /// 获取攻击数据
    /// </summary>
    private AttackData GetAttackData(FormationGearData gearData)
    {
        var baseGearData = gearData.gearData;
        float damage = baseGearData.attack;//GearManager.Instance.GetGearDamageByLevel(baseGearData.m_gearKey, item.Level);

        return new AttackData(damage,
                            damage * 1,
                            damage * 1,
                         false,
                            1,
                            1,
                            ElementType.Physical);
    }


    /// <summary>
    /// 发射开始回调
    /// </summary>
    private void OnLaunchExecute()
    {
        Debug.Log($"武器 {item.ItemName} 开始发射");
        // 可以在这里添加发射开始的视觉效果
    }

    /// <summary>
    /// 发射完成回调
    /// </summary>
    private void OnLaunchComplete()
    {
        Debug.Log($"武器 {item.ItemName} 发射完成");
        // 可以在这里添加发射完成的视觉效果
    }

    /// <summary>
    /// 命中目标回调
    /// </summary>
    private void OnHitTarget(BattleHitData hitData)
    {
        //Debug.Log($"武器 {item.ItemName} 命中目标: {hitData.hitActor.gameObject.name}");
        // 可以在这里添加命中效果的视觉反馈
    }

    /// <summary>
    /// 清理武器发射组件
    /// </summary>
    private void CleanupGearLauncher()
    {
        if (targetFinder != null)
        {
            targetFinder.CleanCachedTarget();
        }

        if (launchControl != null)
        {
            launchControl.AbortLaunch();
        }
    }

    #endregion

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



