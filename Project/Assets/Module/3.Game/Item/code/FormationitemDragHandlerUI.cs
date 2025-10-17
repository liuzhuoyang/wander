using UnityEngine;
using PlayerInteraction;
using System;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(DraggableUI))]
public class FormationitemDragHandlerUI : MonoBehaviour
{
    private DraggableUI draggable;
    private Camera mainCam;
    private Vector2 initPos;
    private FormationNode currentHoverNode; // 当前悬停的节点
                                            // private FormationitemData itemData; // 物品配置
    private Action onEndDrag;
    public UIBattleItemSlot uIBattleItemSlot;

    public FormationItem itemData;


    void Awake()
    {
        mainCam = Camera.main;
        draggable = GetComponent<DraggableUI>();
    }
    void OnEnable()
    {
        draggable.onBeginDrag += HandleBeginDrag;
        draggable.onEndDrag += HandleEndDrag;
    }
    void OnDisable()
    {
        draggable.onBeginDrag -= HandleBeginDrag;
        draggable.onEndDrag -= HandleEndDrag;
    }
    void HandleBeginDrag()
    {
        initPos = transform.position;
    }
    void Update()
    {
        if (draggable.m_isDragging)
        {
            // 将UI屏幕坐标转换为世界坐标
            Vector2 screenPos = transform.position;
            Vector2 worldPos = mainCam.ScreenToWorldPoint(screenPos);

            // 检测最近的可交互节点（可放置或可升级）
            FormationNode nearestNode = BattleFormationMangaer.Instance.GetNearestInteractableNode(worldPos, itemData.itemConfig);

            // 如果悬停的节点发生变化
            if (nearestNode != currentHoverNode)
            {
                // 清除之前的悬停效果
                if (currentHoverNode != null)
                {
                    OnNodeHoverExit(currentHoverNode);
                }

                currentHoverNode = nearestNode;

                // 应用新的悬停效果
                if (currentHoverNode != null)
                {
                    OnNodeHoverEnter(currentHoverNode);
                }
            }
        }
    }
    void HandleEndDrag(Vector2 scrPos)
    {
        Debug.Log($"[调试] HandleEndDrag 被调用。currentHoverNode: {(currentHoverNode != null ? currentHoverNode.name : "null")}, itemData: {(itemData != null ? itemData.ItemName : "null")}, 拖拽屏幕坐标: {scrPos}");

        if (currentHoverNode != null)
        {
            Debug.Log($"[调试] 当前悬停节点: {currentHoverNode.name}, 物品配置: {(itemData != null ? itemData.ItemName : "null")}");

            // 优先检查是否可以升级
            bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(currentHoverNode, itemData.itemConfig);
            Debug.Log($"[调试] 是否可升级: {canUpgrade}");

            if (canUpgrade)
            {
                Debug.Log("[调试] 执行升级节点上的物品操作。");
                UpgradeItemOnNode(currentHoverNode);
            }
            else
            {
                // 检查是否可以放置
                bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(currentHoverNode, itemData.itemConfig);
                Debug.Log($"[调试] 是否可放置: {canPlace}");

                if (canPlace)
                {
                    Debug.Log("[调试] 执行放置物品到节点操作。");
                    PlaceItemOnNode(currentHoverNode);
                }
                else
                {
                    Debug.Log("[调试] 既不能放置也不能升级，物品回到原位。");
                    transform.position = initPos;
                }
            }
        }
        else
        {
            Debug.Log("[调试] 没有悬停到任何节点，物品回到原位。");
            transform.position = initPos;
        }

        // 清理悬停状态
        if (currentHoverNode != null)
        {
            Debug.Log($"[调试] 清理悬停节点状态: {currentHoverNode.name}");
            OnNodeHoverExit(currentHoverNode);
            currentHoverNode = null;
        }
        else
        {
            Debug.Log("[调试] 无需清理悬停节点状态。");
        }
    }

    #region 节点悬停效果
    void OnNodeHoverEnter(FormationNode node)
    {
        // 优先检查是否可以升级
        bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(node, itemData.itemConfig);

        if (canUpgrade)
        {

        }
        else
        {
            // 检查是否可以放置
            bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(node, itemData.itemConfig);

            // 显示悬停效果
            if (canPlace)
            {
                // 绿色高亮 - 可以放置
                node.SetValidHighlight();
            }
            else
            {
                // 红色高亮 - 不能放置
                node.SetInvalidHighlight();
            }
        }
    }

    void OnNodeHoverExit(FormationNode node)
    {
        // 清除悬停效果
        node.ClearHighlight();
    }
    #endregion

    #region 物品放置和升级
    void PlaceItemOnNode(FormationNode node)
    {
        // 在节点上创建物品
        FormationWorldItem item = node.SetItem(itemData);

        onEndDrag?.Invoke();
        // 销毁UI物品
        Destroy(gameObject);
    }

    void UpgradeItemOnNode(FormationNode node)
    {
        // 升级节点上的物品
        bool success = BattleFormationMangaer.Instance.UpgradeItemOnNode(node, itemData.itemConfig);

        if (success)
        {
            onEndDrag?.Invoke();
            // 销毁UI物品
            Destroy(gameObject);
        }
        else
        {
            // 升级失败，返回原位
            transform.position = initPos;
        }
    }
    #endregion

    #region 公共方法
    /// <summary>
    /// 设置物品配置
    /// </summary>
    /// <param name="config">物品配置</param>
    public void SetitemData(FormationItem formationItemData, Action onEndDrag)
    {
        this.itemData = formationItemData;
        this.onEndDrag = onEndDrag;
        uIBattleItemSlot.Init(formationItemData);

    }

    #endregion
}

