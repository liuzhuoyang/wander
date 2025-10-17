using UnityEngine;

/// <summary>
/// 法阵节点组件，挂在formatianPrefab上
/// </summary>
public class FormationNode : MonoBehaviour
{
    #region 字段和属性

    [Header("节点信息")]
    [SerializeField] private int nodeIndex;
    [SerializeField] private Vector2 nodePosition;

    [Header("节点状态")]
    [SerializeField] private bool isActive = true;

    [Header("节点物品")]
    [SerializeField] private FormationWorldItem worldItem;


    public GameObject itemPrefab;

    // 节点数据引用
    private FormatianNodaData nodeData;

    #endregion

    #region 公共属性

    /// <summary>
    /// 节点索引
    /// </summary>
    public int NodeIndex => nodeIndex;

    /// <summary>
    /// 节点位置
    /// </summary>
    public Vector2 NodePosition => nodePosition;

    /// <summary>
    /// 节点是否激活
    /// </summary>
    public bool IsActive => isActive;

    /// <summary>
    /// 节点数据
    /// </summary>
    public FormatianNodaData NodeData => nodeData;

    /// <summary>
    /// 节点上的物品
    /// </summary>
    public FormationWorldItem WorldItem => worldItem;

    #endregion

    #region 初始化方法

    /// <summary>
    /// 初始化节点
    /// </summary>
    /// <param name="index">节点索引</param>
    /// <param name="position">节点位置</param>
    /// <param name="data">节点数据</param>
    public void Initialize(int index, Vector2 position, FormatianNodaData data)
    {
        nodeIndex = index;
        nodePosition = position;
        nodeData = data;

        // 设置GameObject名称
        gameObject.name = $"FormatianNode_{index}";

        // 设置位置
        transform.position = new Vector3(position.x, position.y, 0f);

        // 激活节点
        SetActive(true);
    }

    #endregion

    #region 节点状态管理

    /// <summary>
    /// 设置节点激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(active);
    }

    #endregion

    #region 节点关系查询

    /// <summary>
    /// 获取下一个节点的索引
    /// </summary>
    /// <returns>下一个节点的索引，如果是最后一个节点返回-1</returns>
    public int GetNextNodeIndex()
    {
        return BattleFormationMangaer.Instance.GetNextNodeIndex(nodeIndex);
    }

    /// <summary>
    /// 获取下一个节点
    /// </summary>
    /// <returns>下一个节点，如果不存在返回null</returns>
    public FormationNode GetNextNode()
    {
        GameObject nextGameObject = BattleFormationMangaer.Instance.GetNextNode(nodeIndex);
        return nextGameObject?.GetComponent<FormationNode>();
    }

    /// <summary>
    /// 获取上一个节点的索引
    /// </summary>
    /// <returns>上一个节点的索引，如果是第一个节点返回-1</returns>
    public int GetPreviousNodeIndex()
    {
        return BattleFormationMangaer.Instance.GetPreviousNodeIndex(nodeIndex);
    }

    /// <summary>
    /// 获取上一个节点
    /// </summary>
    /// <returns>上一个节点，如果不存在返回null</returns>
    public FormationNode GetPreviousNode()
    {
        GameObject prevGameObject = BattleFormationMangaer.Instance.GetPreviousNode(nodeIndex);
        return prevGameObject?.GetComponent<FormationNode>();
    }

    /// <summary>
    /// 检查是否为第一个节点
    /// </summary>
    /// <returns>是否为第一个节点</returns>
    public bool IsFirstNode()
    {
        return BattleFormationMangaer.Instance.IsFirstNode(nodeIndex);
    }

    /// <summary>
    /// 检查是否为最后一个节点
    /// </summary>
    /// <returns>是否为最后一个节点</returns>
    public bool IsLastNode()
    {
        return BattleFormationMangaer.Instance.IsLastNode(nodeIndex);
    }

    /// <summary>
    /// 获取到指定节点的距离
    /// </summary>
    /// <param name="targetNodeIndex">目标节点索引</param>
    /// <returns>距离</returns>
    public float GetDistanceToNode(int targetNodeIndex)
    {
        GameObject targetGameObject = BattleFormationMangaer.Instance.GetNodeByIndex(targetNodeIndex);
        if (targetGameObject == null) return float.MaxValue;

        FormationNode targetNode = targetGameObject.GetComponent<FormationNode>();
        if (targetNode == null) return float.MaxValue;

        return Vector2.Distance(nodePosition, targetNode.nodePosition);
    }

    /// <summary>
    /// 获取到指定节点的距离
    /// </summary>
    /// <param name="targetNode">目标节点</param>
    /// <returns>距离</returns>
    public float GetDistanceToNode(FormationNode targetNode)
    {
        if (targetNode == null) return float.MaxValue;

        return Vector2.Distance(nodePosition, targetNode.nodePosition);
    }

    #endregion

    #region 节点信息查询

    /// <summary>
    /// 检查节点是否已正确初始化
    /// </summary>
    /// <returns>是否已初始化</returns>
    public bool IsInitialized()
    {
        return nodeData != null;
    }

    /// <summary>
    /// 获取节点数据的字符串表示（用于调试）
    /// </summary>
    /// <returns>节点数据信息</returns>
    public string GetNodeInfo()
    {
        if (nodeData == null)
        {
            return $"Node {nodeIndex}: 未初始化";
        }

        string itemInfo = worldItem != null ? $", 物品: {worldItem.ItemName}({worldItem.ItemType})" : ", 无物品";
        return $"Node {nodeIndex}: 位置({nodePosition.x:F2}, {nodePosition.y:F2}), 激活状态: {isActive}{itemInfo}";
    }

    #endregion

    #region 物品管理

    /// <summary>
    /// 设置节点上的物品
    /// </summary>
    /// <param name="config">物品配置</param>
    /// <returns>创建的物品</returns>
    public FormationWorldItem SetItem(FormationItem formationItem)
    {
        // 如果已有物品，先移除
        if (worldItem != null)
        {
            RemoveItem();
        }

        // 创建新的物品
        GameObject itemObject = Instantiate(itemPrefab, transform);
        worldItem = itemObject.GetComponent<FormationWorldItem>();

        worldItem.Initialize(formationItem, this);
        return worldItem;
    }

    /// <summary>
    /// 移除节点上的物品
    /// </summary>
    public void RemoveItem()
    {
        if (worldItem != null)
        {
            Debug.Log($"节点 {nodeIndex} 移除了物品: {worldItem.ItemName}");
            Destroy(worldItem.gameObject);
            worldItem = null;
        }
    }

    /// <summary>
    /// 检查节点是否有物品
    /// </summary>
    /// <returns>是否有物品</returns>
    public bool HasItem()
    {
        return worldItem != null;
    }

    #endregion

    #region 物品交互

    /// <summary>
    /// 触发节点上的物品
    /// </summary>
    /// <param name="triggerer">触发者</param>
    public void TriggerItem(GameObject triggerer)
    {
        if (worldItem != null)
        {
            worldItem.Trigger(triggerer);
        }
    }

    /// <summary>
    /// 重置节点上的物品状态
    /// </summary>
    public void ResetItem()
    {
        if (worldItem != null)
        {
            worldItem.Reset();
        }
    }

    /// <summary>
    /// 获取触发进度
    /// </summary>
    /// <returns>触发进度 (0-1)</returns>
    public float GetTriggerProgress()
    {
        return worldItem != null ? (float)worldItem.CurrentTriggerCount / worldItem.RequiredTriggerCount : 0f;
    }

    /// <summary>
    /// 获取当前触发次数
    /// </summary>
    /// <returns>当前触发次数</returns>
    public int GetCurrentTriggerCount()
    {
        return worldItem?.CurrentTriggerCount ?? 0;
    }

    /// <summary>
    /// 获取需要触发的次数
    /// </summary>
    /// <returns>需要触发的次数</returns>
    public int GetRequiredTriggerCount()
    {
        return worldItem?.RequiredTriggerCount ?? 0;
    }

    #endregion

    #region 高亮效果方法

    /// <summary>
    /// 清除节点高亮效果
    /// </summary>
    public void ClearHighlight()
    {

    }

    /// <summary>
    /// 设置悬停高亮
    /// </summary>
    public void SetHoverHighlight()
    {

    }

    /// <summary>
    /// 设置有效放置高亮
    /// </summary>
    public void SetValidHighlight()
    {

    }

    /// <summary>
    /// 设置无效放置高亮
    /// </summary>
    public void SetInvalidHighlight()
    {

    }

    #endregion

    #region 物品效果管理

    /// <summary>
    /// 为物品添加效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    public void AddItemEffect(FormationEffectData effectData)
    {
        if (worldItem != null)
        {
            worldItem.Effects.Add(effectData);
            Debug.Log($"为节点 {nodeIndex} 的物品 {worldItem.ItemName} 添加了效果: {effectData.effectType}");
        }
        else
        {
            Debug.LogWarning($"节点 {nodeIndex} 没有物品，无法添加效果");
        }
    }

    #endregion

    #region Unity编辑器方法

    private void OnDrawGizmos()
    {
        // 在Scene视图中绘制节点信息
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 如果有物品，绘制物品指示器
        if (worldItem != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }

        // 绘制节点索引和物品信息
#if UNITY_EDITOR
        string label = $"Node {nodeIndex}";
        if (worldItem != null)
        {
            label += $"\n{worldItem.ItemName}";
        }
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.8f, label);
#endif
    }

    #endregion
}
