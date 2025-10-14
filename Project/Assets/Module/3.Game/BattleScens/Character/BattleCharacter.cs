using UnityEngine;
using System.Collections;

public class BattleCharacter : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 2f;
    
    [Header("状态")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private int currentNodeIndex = -1;
    
    private Coroutine moveCoroutine;
    
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool IsMoving => isMoving;
    
    /// <summary>
    /// 当前节点索引
    /// </summary>
    public int CurrentNodeIndex => currentNodeIndex;
    
    /// <summary>
    /// 初始化角色，放置在指定节点
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    public void Initialize(int nodeIndex)
    {
        currentNodeIndex = nodeIndex;
        
        // 获取节点位置并放置角色
        GameObject node = BattleFormationMangaer.Instance.GetNodeByIndex(nodeIndex);
        if (node != null)
        {
            transform.position = node.transform.position;
            Debug.Log($"角色初始化在节点 {nodeIndex}，位置: {transform.position}");
        }
        else
        {
            Debug.LogError($"无法找到节点 {nodeIndex}");
        }
    }
    
    /// <summary>
    /// 开始按节点顺序移动
    /// </summary>
    public void StartMoving()
    {
        if (isMoving)
        {
            Debug.LogWarning("角色已经在移动中");
            return;
        }
        
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        moveCoroutine = StartCoroutine(MoveToNextNode());
    }
    
    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMoving()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        
        isMoving = false;
        Debug.Log("角色停止移动");
    }
    
    /// <summary>
    /// 移动到下一个节点的协程
    /// </summary>
    private IEnumerator MoveToNextNode()
    {
        isMoving = true;
        
        while (true)
        {
            // 获取下一个节点
            int nextNodeIndex = BattleFormationMangaer.Instance.GetNextNodeIndex(currentNodeIndex);
            
            if (nextNodeIndex == -1)
            {
                // 到达最后一个节点，回到第一个节点
                nextNodeIndex = GetFirstNodeIndex();
                if (nextNodeIndex == -1)
                {
                    Debug.LogError("无法找到第一个节点");
                    break;
                }
                Debug.Log($"已到达最后一个节点，回到第一个节点 {nextNodeIndex}");
            }
            
            // 移动到下一个节点
            yield return StartCoroutine(MoveToNode(nextNodeIndex));
            
            // 触发当前节点
            TriggerCurrentNode();
            
            // 更新当前节点索引
            currentNodeIndex = nextNodeIndex;
        }
        
        isMoving = false;
        moveCoroutine = null;
    }
    
    /// <summary>
    /// 获取第一个节点的索引
    /// </summary>
    /// <returns>第一个节点的索引，如果找不到返回-1</returns>
    private int GetFirstNodeIndex()
    {
        var formatianNodes = BattleFormationMangaer.Instance.GetCurrentFormatianNodes();
        if (formatianNodes.Count == 0) return -1;
        
        int firstNodeIndex = int.MaxValue;
        
        foreach (var node in formatianNodes)
        {
            string nodeName = node.name;
            if (nodeName.Contains("_"))
            {
                string[] parts = nodeName.Split('_');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int nodeIndex))
                {
                    if (nodeIndex < firstNodeIndex)
                    {
                        firstNodeIndex = nodeIndex;
                    }
                }
            }
        }
        
        return firstNodeIndex == int.MaxValue ? -1 : firstNodeIndex;
    }
    
    /// <summary>
    /// 移动到指定节点
    /// </summary>
    /// <param name="targetNodeIndex">目标节点索引</param>
    private IEnumerator MoveToNode(int targetNodeIndex)
    {
        GameObject targetNode = BattleFormationMangaer.Instance.GetNodeByIndex(targetNodeIndex);
        if (targetNode == null)
        {
            Debug.LogError($"无法找到目标节点 {targetNodeIndex}");
            yield break;
        }
        
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetNode.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float moveTime = distance / moveSpeed;
        
        Debug.Log($"开始移动到节点 {targetNodeIndex}，距离: {distance:F2}，预计时间: {moveTime:F2}秒");
        
        float elapsedTime = 0f;
        
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;
            
            // 使用线性插值移动
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            yield return null;
        }
        
        // 确保最终位置准确
        transform.position = targetPosition;
        Debug.Log($"到达节点 {targetNodeIndex}，位置: {transform.position}");
    }
    
    /// <summary>
    /// 触发当前节点
    /// </summary>
    private void TriggerCurrentNode()
    {
        Debug.Log($"触发节点 {currentNodeIndex}");
        
        // 这里可以添加具体的节点触发逻辑
        // 例如：播放特效、触发技能、改变节点状态等
        
        // 获取节点组件（如果存在）
        GameObject currentNode = BattleFormationMangaer.Instance.GetNodeByIndex(currentNodeIndex);
        if (currentNode != null)
        {
            FormationNode formatianNode = currentNode.GetComponent<FormationNode>();
            if (formatianNode != null)
            {
                // 可以在这里调用FormatianNode的特定方法
                Debug.Log($"节点 {currentNodeIndex} 被角色触发");
            }
        }
    }
    
    /// <summary>
    /// 立即移动到指定节点（不播放移动动画）
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    public void TeleportToNode(int nodeIndex)
    {
        GameObject node = BattleFormationMangaer.Instance.GetNodeByIndex(nodeIndex);
        if (node != null)
        {
            transform.position = node.transform.position;
            currentNodeIndex = nodeIndex;
            Debug.Log($"角色瞬移到节点 {nodeIndex}，位置: {transform.position}");
        }
        else
        {
            Debug.LogError($"无法找到节点 {nodeIndex}");
        }
    }
    
    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed">移动速度</param>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
        Debug.Log($"角色移动速度设置为: {speed}");
    }
    
    private void OnDestroy()
    {
        // 清理协程
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
    }
}
