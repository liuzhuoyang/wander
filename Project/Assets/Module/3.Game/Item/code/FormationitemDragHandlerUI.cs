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
    private FormationItemConfig itemConfig; // 物品配置
    private Action onEndDrag;


    public Transform commonTransform;
    public Transform rareTransform;
    public Transform epicTransform;
    public Transform legendaryTransform;

    public Image imgIcon;

    public TextMeshProUGUI level;
    public TextMeshProUGUI info;

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
            FormationNode nearestNode = BattleFormationMangaer.Instance.GetNearestInteractableNode(worldPos, itemConfig);

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
        if (currentHoverNode != null)
        {
            // 优先检查是否可以升级
            bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(currentHoverNode, itemConfig);

            if (canUpgrade)
            {
                // 升级节点上的物品
                UpgradeItemOnNode(currentHoverNode);
            }
            else
            {
                // 检查是否可以放置
                bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(currentHoverNode, itemConfig);

                if (canPlace)
                {
                    // 放置物品到节点
                    PlaceItemOnNode(currentHoverNode);
                }
                else
                {
                    // 返回原位
                    transform.position = initPos;
                }
            }
        }
        else
        {
            // 没有悬停到任何节点，返回原位
            transform.position = initPos;
        }

        // 清理悬停状态
        if (currentHoverNode != null)
        {
            OnNodeHoverExit(currentHoverNode);
            currentHoverNode = null;
        }
    }

    #region 节点悬停效果
    void OnNodeHoverEnter(FormationNode node)
    {
        // 优先检查是否可以升级
        bool canUpgrade = BattleFormationMangaer.Instance.CanUpgradeItemOnNode(node, itemConfig);

        if (canUpgrade)
        {
            // 蓝色高亮 - 可以升级
            node.SetHighlight(true, Color.blue);
        }
        else
        {
            // 检查是否可以放置
            bool canPlace = BattleFormationMangaer.Instance.CanPlaceItemOnNode(node, itemConfig);

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
        FormationItem item = node.SetItem(itemConfig);

        onEndDrag?.Invoke();
        // 销毁UI物品
        Destroy(gameObject);
    }

    void UpgradeItemOnNode(FormationNode node)
    {
        // 升级节点上的物品
        bool success = BattleFormationMangaer.Instance.UpgradeItemOnNode(node, itemConfig);

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
    public void SetItemConfig(FormationItemConfig config, Action onEndDrag)
    {
        itemConfig = config;
        this.onEndDrag = onEndDrag;
        //GameAssetControl.AssignIcon(config.itemName, imgIcon);
        imgIcon.sprite = config.itemIcon;
        level.text = config.level.ToString();
        info.text = UtilityLocalization.GetLocalization(config.itemName);
        SetRarityImage(config.rarity);
    }

    public void SetRarityImage(Rarity rarity)
    {
        commonTransform.gameObject.SetActive(rarity == Rarity.Common);
        rareTransform.gameObject.SetActive(rarity == Rarity.Rare);
        epicTransform.gameObject.SetActive(rarity == Rarity.Epic);
        legendaryTransform.gameObject.SetActive(rarity == Rarity.Legendary);
    }
    #endregion
}

