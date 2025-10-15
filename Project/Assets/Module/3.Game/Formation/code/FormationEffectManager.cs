using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 法阵效果管理器，负责执行各种法阵效果
/// </summary>
public class FormationEffectManager : Singleton<FormationEffectManager>
{
    // 效果处理器字典
    private Dictionary<EffectType, System.Action<FormationEffectData, GameObject>> effectHandlers;

    protected override void Awake()
    {
        base.Awake();
        InitializeEffectHandlers();
    }

    /// <summary>
    /// 初始化效果处理器
    /// </summary>
    private void InitializeEffectHandlers()
    {
        effectHandlers = new Dictionary<EffectType, System.Action<FormationEffectData, GameObject>>
        {
            { EffectType.SpeedBoost, HandleSpeedBoost },
            { EffectType.DirectionReverse, HandleDirectionReverse },
            { EffectType.Shield, HandleShield },
        };
    }

    /// <summary>
    /// 执行效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    public void ExecuteEffect(FormationEffectData effectData, GameObject triggerer)
    {
        if (effectHandlers.ContainsKey(effectData.effectType))
        {
            effectHandlers[effectData.effectType](effectData, triggerer);
        }
        else
        {
            Debug.LogWarning($"未找到效果类型 {effectData.effectType} 的处理器");
        }
    }

    #region 效果处理器

    /// <summary>
    /// 处理血量恢复效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleHealthRestore(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行血量恢复效果：恢复 {effectData.value} 点血量给 {triggerer.name}");

        // TODO: 实现血量恢复逻辑
        // 例如：triggerer.GetComponent<HealthComponent>()?.RestoreHealth(effectData.value);
    }

    /// <summary>
    /// 处理加速效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleSpeedBoost(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行加速效果：增加 {effectData.value} 速度给 {triggerer.name}，持续 {effectData.duration} 秒");

        // TODO: 实现加速逻辑
        // 例如：triggerer.GetComponent<MovementComponent>()?.ApplySpeedBoost(effectData.value, effectData.duration);
    }

    /// <summary>
    /// 处理方向反转效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleDirectionReverse(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行方向反转效果：反转 {triggerer.name} 的移动方向");

        // TODO: 实现方向反转逻辑
        // 例如：triggerer.GetComponent<MovementComponent>()?.ReverseDirection();
    }

    /// <summary>
    /// 处理金币奖励效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleCoinReward(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行金币奖励效果：给予 {triggerer.name} {effectData.value} 金币");

        // TODO: 实现金币奖励逻辑
        // 例如：GameManager.Instance.AddCoins((int)effectData.value);
    }

    /// <summary>
    /// 处理武器效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleWeaponEffect(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行武器效果：给予 {triggerer.name} 武器效果");

        // TODO: 通过其他接口处理武器效果
        // 例如：WeaponManager.Instance.ApplyWeaponEffect(triggerer, effectData);
    }

    /// <summary>
    /// 处理护盾效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    /// <param name="triggerer">触发者</param>
    private void HandleShield(FormationEffectData effectData, GameObject triggerer)
    {
        Debug.Log($"执行护盾效果：给予 {triggerer.name} 护盾，持续 {effectData.duration} 秒");

        // TODO: 实现护盾逻辑
        // 例如：triggerer.GetComponent<ShieldComponent>()?.ApplyShield(effectData.value, effectData.duration);
    }


    #endregion

    /// <summary>
    /// 注册自定义效果处理器
    /// </summary>
    /// <param name="effectType">效果类型</param>
    /// <param name="handler">处理器</param>
    public void RegisterEffectHandler(EffectType effectType, System.Action<FormationEffectData, GameObject> handler)
    {
        if (effectHandlers.ContainsKey(effectType))
        {
            effectHandlers[effectType] = handler;
        }
        else
        {
            effectHandlers.Add(effectType, handler);
        }

        Debug.Log($"注册了效果处理器：{effectType}");
    }

    /// <summary>
    /// 移除效果处理器
    /// </summary>
    /// <param name="effectType">效果类型</param>
    public void UnregisterEffectHandler(EffectType effectType)
    {
        if (effectHandlers.ContainsKey(effectType))
        {
            effectHandlers.Remove(effectType);
            Debug.Log($"移除了效果处理器：{effectType}");
        }
    }
}
