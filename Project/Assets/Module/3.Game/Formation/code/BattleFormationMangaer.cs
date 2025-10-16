using System.Collections.Generic;
using UnityEngine;

public class BattleFormationMangaer : Singleton<BattleFormationMangaer>
{
    #region 字段和属性
    [Header("法阵节点")]
    public GameObject formatianNodePrefab;

    [SerializeField] public FormationItemDataCollection formationItemDataCollection;

    [Header("法阵管理")]
    [SerializeField] private Transform formatianParent; // 法阵节点的父对象
    
    [Header("法阵配置")]
    [SerializeField] private float nodeDetectionRadius = 0.6f; // 节点检测半径，适合1.2大小的正方形节点

    // 当前法阵的节点列表
    private List<GameObject> currentFormatianNodes = new List<GameObject>();

    // 节点索引到节点的映射
    private Dictionary<int, GameObject> nodeIndexMap = new Dictionary<int, GameObject>();
    #endregion

    #region 公共属性
    /// <summary>
    /// 节点检测半径
    /// </summary>
    public float NodeDetectionRadius => nodeDetectionRadius;
    #endregion

    #region Unity生命周期
    protected override void Awake()
    {
        base.Awake();

        // 如果没有设置父对象，创建一个
        if (formatianParent == null)
        {
            GameObject parentObj = new GameObject("FormatianNodes");
            formatianParent = parentObj.transform;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // 清理当前法阵
        DestroyCurrentFormatian();
    }
    #endregion

    #region 法阵创建和销毁
    public void CraftFormatian(string formatianName)
    {
        CraftFormatian(AllFormatian.dictData[formatianName]);
    }

    /// <summary>
    /// 创建法阵
    /// </summary>
    /// <param name="formatianData">法阵数据</param>
    public void CraftFormatian(FormatianData formatianData)
    {
        // 检查输入是否合法和Prefab是否设置
        if (formatianData == null || formatianNodePrefab == null || formatianData.listNodeData == null)
        {
            Debug.LogWarning("法阵数据或Prefab为空，无法创建法阵");
            return;
        }

        // 如果已存在法阵，先删除
        DestroyCurrentFormatian();

        // 遍历FormatianData中的每个节点，生成对应的Prefab实例并放置在指定位置
        foreach (var nodeData in formatianData.listNodeData)
        {
            if (nodeData == null) continue;

            Vector2 pos2D = nodeData.position;
            Vector3 worldPos = new Vector3(pos2D.x, pos2D.y, 0f);
            GameObject instance = Instantiate(formatianNodePrefab, worldPos, Quaternion.identity, formatianParent);

            // 获取FormationNode组件并初始化
            FormationNode formationNode = instance.GetComponent<FormationNode>();
            if (formationNode != null)
            {
                formationNode.Initialize(nodeData.index, nodeData.position, nodeData);
            }
            else
            {
                Debug.LogError($"FormatianNode 组件未找到在 {instance.name} 上");
            }

            // 添加到当前法阵节点列表和索引映射
            currentFormatianNodes.Add(instance);
            nodeIndexMap[nodeData.index] = instance;
        }

        Debug.Log($"成功创建法阵 '{formatianData.formatianName}'，包含 {currentFormatianNodes.Count} 个节点");

        // 输出每个节点的详细信息
        foreach (var node in currentFormatianNodes)
        {
            FormationNode formationNode = node.GetComponent<FormationNode>();
            if (formationNode != null)
            {
                Debug.Log($"  - {formationNode.GetNodeInfo()}");
            }
        }
    }

    /// <summary>
    /// 删除当前法阵
    /// </summary>
    public void DestroyCurrentFormatian()
    {
        // 删除所有节点实例
        foreach (var node in currentFormatianNodes)
        {
            if (node != null)
            {
                Destroy(node);
            }
        }

        // 清空列表和映射
        currentFormatianNodes.Clear();
        nodeIndexMap.Clear();

        Debug.Log("已删除当前法阵");
    }
    #endregion

    #region 法阵查询方法
    /// <summary>
    /// 获取当前法阵的所有节点
    /// </summary>
    /// <returns>节点列表</returns>
    public List<GameObject> GetCurrentFormatianNodes()
    {
        return new List<GameObject>(currentFormatianNodes);
    }

    /// <summary>
    /// 检查是否有法阵存在
    /// </summary>
    /// <returns>是否存在法阵</returns>
    public bool HasFormatian()
    {
        return currentFormatianNodes.Count > 0;
    }

    /// <summary>
    /// 根据索引获取节点
    /// </summary>
    /// <param name="index">节点索引</param>
    /// <returns>节点，如果不存在返回null</returns>
    public GameObject GetNodeByIndex(int index)
    {
        return nodeIndexMap.ContainsKey(index) ? nodeIndexMap[index] : null;
    }
    #endregion

    #region 节点导航方法
    /// <summary>
    /// 获取下一个节点的索引
    /// </summary>
    /// <param name="currentIndex">当前节点索引</param>
    /// <returns>下一个节点的索引，如果是最后一个节点返回-1</returns>
    public int GetNextNodeIndex(int currentIndex)
    {
        if (currentFormatianNodes.Count == 0) return -1;

        // 找到当前节点在列表中的位置
        int currentPosition = -1;
        for (int i = 0; i < currentFormatianNodes.Count; i++)
        {
            // 通过名称获取节点索引
            string nodeName = currentFormatianNodes[i].name;
            if (nodeName.Contains($"_{currentIndex}"))
            {
                currentPosition = i;
                break;
            }
        }

        if (currentPosition == -1) return -1;

        // 如果是最后一个节点，返回-1（表示没有下一个节点）
        if (currentPosition == currentFormatianNodes.Count - 1)
        {
            return -1;
        }

        // 返回下一个节点的索引
        string nextNodeName = currentFormatianNodes[currentPosition + 1].name;
        if (int.TryParse(nextNodeName.Split('_')[1], out int nextIndex))
        {
            return nextIndex;
        }

        return -1;
    }

    /// <summary>
    /// 获取下一个节点
    /// </summary>
    /// <param name="currentIndex">当前节点索引</param>
    /// <returns>下一个节点，如果不存在返回null</returns>
    public GameObject GetNextNode(int currentIndex)
    {
        int nextIndex = GetNextNodeIndex(currentIndex);
        return nextIndex == -1 ? null : GetNodeByIndex(nextIndex);
    }

    /// <summary>
    /// 获取上一个节点的索引
    /// </summary>
    /// <param name="currentIndex">当前节点索引</param>
    /// <returns>上一个节点的索引，如果是第一个节点返回-1</returns>
    public int GetPreviousNodeIndex(int currentIndex)
    {
        if (currentFormatianNodes.Count == 0) return -1;

        // 找到当前节点在列表中的位置
        int currentPosition = -1;
        for (int i = 0; i < currentFormatianNodes.Count; i++)
        {
            // 通过名称获取节点索引
            string nodeName = currentFormatianNodes[i].name;
            if (nodeName.Contains($"_{currentIndex}"))
            {
                currentPosition = i;
                break;
            }
        }

        if (currentPosition == -1) return -1;

        // 如果是第一个节点，返回-1（表示没有上一个节点）
        if (currentPosition == 0)
        {
            return -1;
        }

        // 返回上一个节点的索引
        string prevNodeName = currentFormatianNodes[currentPosition - 1].name;
        if (int.TryParse(prevNodeName.Split('_')[1], out int prevIndex))
        {
            return prevIndex;
        }

        return -1;
    }

    /// <summary>
    /// 获取上一个节点
    /// </summary>
    /// <param name="currentIndex">当前节点索引</param>
    /// <returns>上一个节点，如果不存在返回null</returns>
    public GameObject GetPreviousNode(int currentIndex)
    {
        int previousIndex = GetPreviousNodeIndex(currentIndex);
        return previousIndex == -1 ? null : GetNodeByIndex(previousIndex);
    }

    /// <summary>
    /// 检查是否为第一个节点
    /// </summary>
    /// <param name="index">节点索引</param>
    /// <returns>是否为第一个节点</returns>
    public bool IsFirstNode(int index)
    {
        if (currentFormatianNodes.Count == 0) return false;
        string firstName = currentFormatianNodes[0].name;
        return firstName.Contains($"_{index}");
    }

    /// <summary>
    /// 检查是否为最后一个节点
    /// </summary>
    /// <param name="index">节点索引</param>
    /// <returns>是否为最后一个节点</returns>
    public bool IsLastNode(int index)
    {
        if (currentFormatianNodes.Count == 0) return false;
        string lastName = currentFormatianNodes[currentFormatianNodes.Count - 1].name;
        return lastName.Contains($"_{index}");
    }
    #endregion

    #region 节点物品管理
    /// <summary>
    /// 为指定节点设置物品
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    /// <param name="config">物品配置</param>
    /// <returns>创建的物品</returns>
    public FormationItem SetNodeItem(int nodeIndex, FormationItemConfig config)
    {
        GameObject nodeObject = GetNodeByIndex(nodeIndex);
        if (nodeObject != null)
        {
            FormationNode formationNode = nodeObject.GetComponent<FormationNode>();
            if (formationNode != null)
            {
                return formationNode.SetItem(config);
            }
            else
            {
                Debug.LogError($"节点 {nodeIndex} 上没有 FormationNode 组件");
            }
        }
        else
        {
            Debug.LogError($"找不到索引为 {nodeIndex} 的节点");
        }
        return null;
    }

    /// <summary>
    /// 移除指定节点上的物品
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    public void RemoveNodeItem(int nodeIndex)
    {
        GameObject nodeObject = GetNodeByIndex(nodeIndex);
        if (nodeObject != null)
        {
            FormationNode formationNode = nodeObject.GetComponent<FormationNode>();
            if (formationNode != null)
            {
                formationNode.RemoveItem();
            }
        }
    }

    /// <summary>
    /// 获取指定节点上的物品
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    /// <returns>物品组件，如果没有返回null</returns>
    public FormationItem GetNodeItem(int nodeIndex)
    {
        GameObject nodeObject = GetNodeByIndex(nodeIndex);
        if (nodeObject != null)
        {
            FormationNode formationNode = nodeObject.GetComponent<FormationNode>();
            return formationNode?.Item;
        }
        return null;
    }

    /// <summary>
    /// 检查指定节点是否有物品
    /// </summary>
    /// <param name="nodeIndex">节点索引</param>
    /// <returns>是否有物品</returns>
    public bool HasNodeItem(int nodeIndex)
    {
        GameObject nodeObject = GetNodeByIndex(nodeIndex);
        if (nodeObject != null)
        {
            FormationNode formationNode = nodeObject.GetComponent<FormationNode>();
            return formationNode?.HasItem() ?? false;
        }
        return false;
    }

    /// <summary>
    /// 重置所有节点的物品状态
    /// </summary>
    public void ResetAllNodeItems()
    {
        foreach (var node in currentFormatianNodes)
        {
            if (node != null)
            {
                FormationNode formationNode = node.GetComponent<FormationNode>();
                formationNode?.ResetItem();
            }
        }
        Debug.Log("已重置所有节点的物品状态");
    }

    /// <summary>
    /// 获取所有有物品的节点
    /// </summary>
    /// <returns>有物品的节点列表</returns>
    public List<FormationNode> GetNodesWithItems()
    {
        List<FormationNode> nodesWithItems = new List<FormationNode>();

        foreach (var node in currentFormatianNodes)
        {
            if (node != null)
            {
                FormationNode formationNode = node.GetComponent<FormationNode>();
                if (formationNode != null && formationNode.HasItem())
                {
                    nodesWithItems.Add(formationNode);
                }
            }
        }

        return nodesWithItems;
    }
    #endregion

    #region 拖拽检测方法
    /// <summary>
    /// 检测世界坐标是否在法阵节点范围内
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <returns>最近的节点，如果没有则返回null</returns>
    public FormationNode GetNearestFormationNode(Vector2 worldPos)
    {
        FormationNode nearestNode = null;
        float nearestDistance = float.MaxValue;
        
        foreach (var node in currentFormatianNodes)
        {
            if (node == null) continue;
            
            FormationNode formationNode = node.GetComponent<FormationNode>();
            if (formationNode == null || !formationNode.IsActive) continue;
            
            // 计算到节点中心的距离
            float distance = Vector2.Distance(worldPos, formationNode.NodePosition);
            
            // 如果在检测范围内且是最近的节点
            if (distance <= nodeDetectionRadius && distance < nearestDistance)
            {
                nearestNode = formationNode;
                nearestDistance = distance;
            }
        }
        
        return nearestNode;
    }

    /// <summary>
    /// 获取所有在检测范围内的法阵节点
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <returns>在范围内的节点列表，按距离排序</returns>
    public List<FormationNode> GetFormationNodesInRange(Vector2 worldPos)
    {
        List<FormationNode> nodesInRange = new List<FormationNode>();
        
        foreach (var node in currentFormatianNodes)
        {
            if (node == null) continue;
            
            FormationNode formationNode = node.GetComponent<FormationNode>();
            if (formationNode == null || !formationNode.IsActive) continue;
            
            float distance = Vector2.Distance(worldPos, formationNode.NodePosition);
            if (distance <= nodeDetectionRadius)
            {
                nodesInRange.Add(formationNode);
            }
        }
        
        // 按距离排序，最近的在前
        nodesInRange.Sort((a, b) => 
        {
            float distA = Vector2.Distance(worldPos, a.NodePosition);
            float distB = Vector2.Distance(worldPos, b.NodePosition);
            return distA.CompareTo(distB);
        });
        
        return nodesInRange;
    }

    /// <summary>
    /// 检查是否可以放置物品到指定节点
    /// </summary>
    /// <param name="node">目标节点</param>
    /// <param name="itemConfig">物品配置</param>
    /// <returns>是否可以放置</returns>
    public bool CanPlaceItemOnNode(FormationNode node, FormationItemConfig itemConfig)
    {
        if (node == null || !node.IsActive) return false;
        
        // 检查节点是否已有物品
        if (node.HasItem()) return false;
        
        // 可以在这里添加更多检查逻辑
        // 比如：物品类型限制、节点类型限制等
        
        return true;
    }

    /// <summary>
    /// 检查是否可以升级节点上的物品
    /// </summary>
    /// <param name="node">目标节点</param>
    /// <param name="draggedItemConfig">拖拽的物品配置</param>
    /// <returns>是否可以升级</returns>
    public bool CanUpgradeItemOnNode(FormationNode node, FormationItemConfig draggedItemConfig)
    {
        if (node == null || !node.IsActive) return false;
        
        // 检查节点是否有物品
        if (!node.HasItem()) return false;
        
        FormationItem existingItem = node.Item;
        if (existingItem == null) return false;
        
        // 检查物品名称和类型是否相同
        if (existingItem.ItemName != draggedItemConfig.itemName || 
            existingItem.ItemType != draggedItemConfig.itemType)
        {
            return false;
        }
        
        // 检查拖拽的物品是否可以升级
        if (!draggedItemConfig.canUpgrade) return false;
        
        // 检查是否达到最大等级
        if (existingItem.Level >= draggedItemConfig.maxLevel) return false;
        
        return true;
    }

    /// <summary>
    /// 升级节点上的物品
    /// </summary>
    /// <param name="node">目标节点</param>
    /// <param name="draggedItemConfig">拖拽的物品配置</param>
    /// <returns>是否升级成功</returns>
    public bool UpgradeItemOnNode(FormationNode node, FormationItemConfig draggedItemConfig)
    {
        if (!CanUpgradeItemOnNode(node, draggedItemConfig)) return false;
        
        FormationItem existingItem = node.Item;
        if (existingItem == null) return false;
        
        // 升级物品等级
        existingItem.UpgradeLevel();
        
        return true;
    }

    /// <summary>
    /// 获取最近的可放置节点
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <param name="itemConfig">物品配置</param>
    /// <returns>最近的可放置节点，如果没有则返回null</returns>
    public FormationNode GetNearestPlaceableNode(Vector2 worldPos, FormationItemConfig itemConfig)
    {
        var nodesInRange = GetFormationNodesInRange(worldPos);
        
        foreach (var node in nodesInRange)
        {
            if (CanPlaceItemOnNode(node, itemConfig))
            {
                return node;
            }
        }
        
        return null;
    }

    /// <summary>
    /// 获取最近的可交互节点（可放置或可升级）
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <param name="itemConfig">物品配置</param>
    /// <returns>最近的可交互节点，如果没有则返回null</returns>
    public FormationNode GetNearestInteractableNode(Vector2 worldPos, FormationItemConfig itemConfig)
    {
        var nodesInRange = GetFormationNodesInRange(worldPos);
        
        foreach (var node in nodesInRange)
        {
            // 优先检查是否可以升级
            if (CanUpgradeItemOnNode(node, itemConfig))
            {
                return node;
            }
            // 然后检查是否可以放置
            else if (CanPlaceItemOnNode(node, itemConfig))
            {
                return node;
            }
        }
        
        return null;
    }
    #endregion
}
