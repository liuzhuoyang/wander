using UnityEngine;

/// <summary>
/// 法阵节点组件，挂在formatianPrefab上
/// </summary>
public class FormationNode : MonoBehaviour
{
    [Header("节点信息")]
    [SerializeField] private int nodeIndex;
    [SerializeField] private Vector2 nodePosition;

    [Header("节点状态")]
    [SerializeField] private bool isActive = true;


    [Header("节点物品")]
    [SerializeField] private FormationItem item;

    // 节点数据引用
    private FormatianNodaData nodeData;

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
    public FormationItem Item => item;

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

    /// <summary>
    /// 设置节点激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetActive(bool active)
    {
        isActive = active;
        gameObject.SetActive(active);
    }

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

        string itemInfo = item != null ? $", 物品: {item.ItemName}({item.ItemType})" : ", 无物品";
        return $"Node {nodeIndex}: 位置({nodePosition.x:F2}, {nodePosition.y:F2}), 激活状态: {isActive}{itemInfo}";
    }

    /// <summary>
    /// 设置节点上的物品
    /// </summary>
    /// <param name="config">物品配置</param>
    /// <returns>创建的物品</returns>
    public FormationItem SetItem(FormationItemConfig config)
    {
        // 如果已有物品，先移除
        if (item != null)
        {
            RemoveItem();
        }

        // 创建新的物品
        GameObject itemObject = new GameObject();
        itemObject.transform.SetParent(transform);
        itemObject.transform.localPosition = Vector3.zero;

        item = itemObject.AddComponent<FormationItem>();
        item.Initialize(config, this);

        Debug.Log($"节点 {nodeIndex} 设置了物品: {config.itemName} ({config.itemType}), 需要触发 {config.requiredChargeCount} 次, 冷却: {config.hasCooldown}");

        return item;
    }

    /// <summary>
    /// 移除节点上的物品
    /// </summary>
    public void RemoveItem()
    {
        if (item != null)
        {
            Debug.Log($"节点 {nodeIndex} 移除了物品: {item.ItemName}");
            Destroy(item.gameObject);
            item = null;
        }
    }

    /// <summary>
    /// 检查节点是否有物品
    /// </summary>
    /// <returns>是否有物品</returns>
    public bool HasItem()
    {
        return item != null;
    }


    /// <summary>
    /// 触发节点上的物品
    /// </summary>
    /// <param name="triggerer">触发者</param>
    public void TriggerItem(GameObject triggerer)
    {
        if (item != null)
        {
            item.Trigger(triggerer);
        }
    }

    /// <summary>
    /// 重置节点上的物品状态
    /// </summary>
    public void ResetItem()
    {
        if (item != null)
        {
            item.Reset();
        }
    }

    /// <summary>
    /// 获取触发进度
    /// </summary>
    /// <returns>触发进度 (0-1)</returns>
    public float GetTriggerProgress()
    {
        return item != null ? (float)item.CurrentTriggerCount / item.RequiredTriggerCount : 0f;
    }

    /// <summary>
    /// 获取当前触发次数
    /// </summary>
    /// <returns>当前触发次数</returns>
    public int GetCurrentTriggerCount()
    {
        return item?.CurrentTriggerCount ?? 0;
    }

    /// <summary>
    /// 获取需要触发的次数
    /// </summary>
    /// <returns>需要触发的次数</returns>
    public int GetRequiredTriggerCount()
    {
        return item?.RequiredTriggerCount ?? 0;
    }

    /// <summary>
    /// 为物品添加效果
    /// </summary>
    /// <param name="effectData">效果数据</param>
    public void AddItemEffect(FormationEffectData effectData)
    {
        if (item != null)
        {
            item.Effects.Add(effectData);
            Debug.Log($"为节点 {nodeIndex} 的物品 {item.ItemName} 添加了效果: {effectData.effectType}");
        }
        else
        {
            Debug.LogWarning($"节点 {nodeIndex} 没有物品，无法添加效果");
        }
    }

    private void OnDrawGizmos()
    {
        // 在Scene视图中绘制节点信息
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        // 如果有物品，绘制物品指示器
        if (item != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }

        // 绘制节点索引和物品信息
#if UNITY_EDITOR
        string label = $"Node {nodeIndex}";
        if (item != null)
        {
            label += $"\n{item.ItemName}";
        }
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.8f, label);
#endif
    }
}
