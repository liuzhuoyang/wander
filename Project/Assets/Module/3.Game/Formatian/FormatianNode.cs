using UnityEngine;

/// <summary>
/// 法阵节点组件，挂在formatianPrefab上
/// </summary>
public class FormatianNode : MonoBehaviour
{
    [Header("节点信息")]
    [SerializeField] private int nodeIndex;
    [SerializeField] private Vector2 nodePosition;
    
    [Header("节点状态")]
    [SerializeField] private bool isActive = true;


    
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
        return BattleFormatianMangaer.Instance.GetNextNodeIndex(nodeIndex);
    }
    
    /// <summary>
    /// 获取下一个节点
    /// </summary>
    /// <returns>下一个节点，如果不存在返回null</returns>
    public FormatianNode GetNextNode()
    {
        GameObject nextGameObject = BattleFormatianMangaer.Instance.GetNextNode(nodeIndex);
        return nextGameObject?.GetComponent<FormatianNode>();
    }
    
    /// <summary>
    /// 获取上一个节点的索引
    /// </summary>
    /// <returns>上一个节点的索引，如果是第一个节点返回-1</returns>
    public int GetPreviousNodeIndex()
    {
        return BattleFormatianMangaer.Instance.GetPreviousNodeIndex(nodeIndex);
    }
    
    /// <summary>
    /// 获取上一个节点
    /// </summary>
    /// <returns>上一个节点，如果不存在返回null</returns>
    public FormatianNode GetPreviousNode()
    {
        GameObject prevGameObject = BattleFormatianMangaer.Instance.GetPreviousNode(nodeIndex);
        return prevGameObject?.GetComponent<FormatianNode>();
    }
    
    /// <summary>
    /// 检查是否为第一个节点
    /// </summary>
    /// <returns>是否为第一个节点</returns>
    public bool IsFirstNode()
    {
        return BattleFormatianMangaer.Instance.IsFirstNode(nodeIndex);
    }
    
    /// <summary>
    /// 检查是否为最后一个节点
    /// </summary>
    /// <returns>是否为最后一个节点</returns>
    public bool IsLastNode()
    {
        return BattleFormatianMangaer.Instance.IsLastNode(nodeIndex);
    }
    
    /// <summary>
    /// 获取到指定节点的距离
    /// </summary>
    /// <param name="targetNodeIndex">目标节点索引</param>
    /// <returns>距离</returns>
    public float GetDistanceToNode(int targetNodeIndex)
    {
        GameObject targetGameObject = BattleFormatianMangaer.Instance.GetNodeByIndex(targetNodeIndex);
        if (targetGameObject == null) return float.MaxValue;
        
        FormatianNode targetNode = targetGameObject.GetComponent<FormatianNode>();
        if (targetNode == null) return float.MaxValue;
        
        return Vector2.Distance(nodePosition, targetNode.nodePosition);
    }
    
    /// <summary>
    /// 获取到指定节点的距离
    /// </summary>
    /// <param name="targetNode">目标节点</param>
    /// <returns>距离</returns>
    public float GetDistanceToNode(FormatianNode targetNode)
    {
        if (targetNode == null) return float.MaxValue;
        
        return Vector2.Distance(nodePosition, targetNode.nodePosition);
    }
    
    private void OnDrawGizmos()
    {
        // 在Scene视图中绘制节点信息
        Gizmos.color = isActive ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        
        // 绘制节点索引
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.8f, $"Node {nodeIndex}");
        #endif
    }
}
