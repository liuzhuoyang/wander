using UnityEngine;
using PlayerInteraction;
using System;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 世界层物品的UI拖拽处理器
/// 专门处理从世界层节点拖拽到UI层的物品回收和合成逻辑
/// 使用自定义拖拽系统，不依赖DraggableUI
/// </summary>
public class FormationItemWorldDragHandlerUI : MonoBehaviour
{
    private Camera mainCam;
    private Vector2 initPos;
    private FormationNode originalNode; // 原始节点引用


    private Action<bool> onEndDrag; // 拖拽结束回调，参数表示是否成功处理

    // 自定义拖拽状态
    private Vector2 dragOffset;
    private Vector2 lastMousePosition;


    public UIBattleItemSlot uIBattleItemSlot;
    public FormationItem itemData;

    // 拖拽状态
    private bool isOverRecycleZone = false;
    private FormationNode currentHoverNode = null;

    void Awake()
    {
        mainCam = Camera.main;
        var playerInput = PlayerInputManager.Instance?.m_currentPlayerInput;
        if (playerInput != null)
        {
            playerInput.onTouchPressCanceled += OnInputRelease;
        }
    }


    void OnDisable()
    {
        // 取消监听
        var playerInput = PlayerInputManager.Instance?.m_currentPlayerInput;
        if (playerInput != null)
        {
            playerInput.onTouchPressCanceled -= OnInputRelease;
        }
    }

    void OnInputRelease()
    {
        // 当用户松开输入时，结束拖拽
        EndDrag();
    }

    void Update()
    {
        UpdateDragPosition();
        UpdateDragState();
        CheckForEndDrag();
    }

    void UpdateDragPosition()
    {
        // 获取当前鼠标位置
        Vector2 currentMousePosition = GetCurrentMousePosition();

        // 更新UI位置
        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 将屏幕坐标转换为UI坐标
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                currentMousePosition,
                null,
                out localPoint
            );
            rectTransform.anchoredPosition = localPoint + dragOffset;
        }

        lastMousePosition = currentMousePosition;
    }

    Vector2 GetCurrentMousePosition()
    {
        return PlayerInputManager.Instance.m_currentPlayerInput.m_pointerScreenPos;
    }

    void CheckForEndDrag()
    {
        // 松开事件已经通过OnInputRelease()处理了
        // 这里可以添加其他结束拖拽的条件，比如拖拽时间过长等
    }

    void UpdateDragState()
    {
        // 检测回收区域
        bool wasOverRecycle = isOverRecycleZone;
        isOverRecycleZone = IsOverRecycleZone();

        if (wasOverRecycle != isOverRecycleZone)
        {
            OnRecycleZoneStateChanged(isOverRecycleZone);
        }


        // 检测节点悬停（排除原始节点）
        FormationNode newHoverNode = GetHoveredNode();
        if (newHoverNode != currentHoverNode)
        {
            OnNodeHoverChanged(currentHoverNode, newHoverNode);
            currentHoverNode = newHoverNode;
        }
    }

    #region 区域检测
    bool IsOverRecycleZone()
    {
        // 检测是否在回收区域内
        // 这里需要根据实际的回收区域UI来检测
        // 暂时返回false，后续实现
        return false;
    }

    FormationNode GetHoveredNode()
    {
        // 将UI屏幕坐标转换为世界坐标
        Vector2 screenPos = GetCurrentMousePosition();
        Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);

        // 获取最近的可交互节点（排除原始节点）
        return BattleFormationMangaer.Instance.GetNearestInteractableNodeExcluding(worldPos, itemData.itemConfig, originalNode);
    }
    #endregion

    #region 状态变化处理
    void OnRecycleZoneStateChanged(bool isOver)
    {
        if (isOver)
        {
            Debug.Log("[世界层拖拽] 进入回收区域");
            // 显示回收提示UI
        }
        else
        {
            Debug.Log("[世界层拖拽] 离开回收区域");
            // 隐藏回收提示UI
        }
    }

    void OnNodeHoverChanged(FormationNode oldNode, FormationNode newNode)
    {
        // 清除旧节点的高亮
        if (oldNode != null)
        {
            oldNode.ClearHighlight();
        }

        // 设置新节点的高亮
        if (newNode != null)
        {
            SetNodeHighlight(newNode);
        }
    }

    void SetNodeHighlight(FormationNode node)
    {
        // 优先检查是否可以升级
        bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(node, itemData.itemConfig);

        if (canUpgrade)
        {
            // 蓝色高亮 - 可以升级
            Debug.Log($"[世界层拖拽] 节点 {node.name} 可以升级");
            // node.SetHighlight(true, Color.blue); // 暂时注释，等FormationNode实现高亮方法
        }
        else
        {
            // 检查是否可以放置
            bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(node, itemData.itemConfig);

            if (canPlace)
            {
                // 绿色高亮 - 可以放置
                Debug.Log($"[世界层拖拽] 节点 {node.name} 可以放置");
                // node.SetValidHighlight(); // 暂时注释，等FormationNode实现高亮方法
            }
            else
            {
                // 红色高亮 - 不能放置
                Debug.Log($"[世界层拖拽] 节点 {node.name} 不能放置");
                // node.SetInvalidHighlight(); // 暂时注释，等FormationNode实现高亮方法
            }
        }
    }

    void ClearAllHighlights()
    {
        if (currentHoverNode != null)
        {
            // currentHoverNode.ClearHighlight(); // 暂时注释，等FormationNode实现高亮方法
            Debug.Log($"[世界层拖拽] 清除节点 {currentHoverNode.name} 的高亮");
        }
    }
    #endregion

    #region 拖拽控制
    public void StartDrag(Vector2 mousePosition)
    {
        Debug.Log($"[世界层拖拽] 开始拖拽物品: {itemData?.itemName}");

        initPos = transform.position;
        lastMousePosition = mousePosition;

        // 计算拖拽偏移量
        var rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                mousePosition,
                null,
                out localPoint
            );
            dragOffset = rectTransform.anchoredPosition - localPoint;
        }
    }

    void EndDrag()
    {
        Debug.Log($"[世界层拖拽] 结束拖拽");

        bool handled = false;

        // 优先处理回收
        if (isOverRecycleZone)
        {
            Debug.Log("[世界层拖拽] 执行回收操作");
            handled = ExecuteRecycle();
        }
        // 处理节点操作
        else if (currentHoverNode != null)
        {
            Debug.Log($"[世界层拖拽] 执行节点操作: {currentHoverNode.name}");
            handled = ExecuteNodeOperation(currentHoverNode);
        }

        // 清理状态
        ClearAllHighlights();
        currentHoverNode = null;
        isOverRecycleZone = false;

        // 通知结果
        onEndDrag?.Invoke(handled);

        Destroy(gameObject);
    }
    #endregion

    #region 操作执行
    bool ExecuteRecycle()
    {
        try
        {
            // 计算回收价值
            int recycleValue = CalculateRecycleValue(itemData.itemConfig);

            // 这里应该调用资源管理器增加资源
            // ResourceManager.Instance.AddCoins(recycleValue);
            Debug.Log($"[世界层拖拽] 回收物品获得: {recycleValue} 金币");

            // 从原始节点移除物品
            if (originalNode != null)
            {
                originalNode.RemoveItem();
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[世界层拖拽] 回收操作失败: {e.Message}");
            return false;
        }
    }

    bool ExecuteNodeOperation(FormationNode targetNode)
    {
        try
        {
            // 优先检查是否可以升级
            bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(targetNode, itemData.itemConfig);

            if (canUpgrade)
            {
                Debug.Log("[世界层拖拽] 执行升级操作");
                return BattleFormationMangaer.Instance.UpgradeItemOnNode(targetNode, itemData.itemConfig);
            }
            else
            {
                // 检查是否可以放置
                bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(targetNode, itemData.itemConfig);

                if (canPlace)
                {
                    Debug.Log("[世界层拖拽] 执行放置操作");
                    targetNode.SetItem(itemData);
                    return true;
                }
            }

            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"[世界层拖拽] 节点操作失败: {e.Message}");
            return false;
        }
    }

    void RestoreOriginalState()
    {
        Debug.Log("[世界层拖拽] 恢复原始状态");
        // 销毁UI物件
        Destroy(gameObject);
    }
    #endregion

    #region 工具方法
    int CalculateRecycleValue(FormationItemConfig config)
    {
        // 根据稀有度和等级计算回收价值
        int baseValue = 10; // 基础价值
        int rarityMultiplier = GetRarityMultiplier(config.rarity);
        int levelMultiplier = config.level;

        return baseValue * rarityMultiplier * levelMultiplier;
    }

    int GetRarityMultiplier(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 1;
            case Rarity.Rare: return 2;
            case Rarity.Epic: return 4;
            case Rarity.Legendary: return 8;
            default: return 1;
        }
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 初始化世界层拖拽物件
    /// </summary>
    /// <param name="config">物品配置</param>
    /// <param name="originalNode">原始节点</param>
    /// <param name="onEndDragCallback">拖拽结束回调</param>
    public void Initialize(FormationItem data, FormationNode originalNode, Action<bool> onEndDragCallback)
    {
        itemData = data;
        this.originalNode = originalNode;
        onEndDrag = onEndDragCallback;

        // 设置UI显示
        uIBattleItemSlot.Init(itemData);
        Debug.Log($"[世界层拖拽] 初始化完成: {data.itemName} (等级 {data.level})");
    }

    #endregion
}