using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//地图节点控制器，用于管理，计算地图节点，用于寻路等功能
public class MapNodeControl : Singleton<MapNodeControl>
{
    // FlowFieldControl flowFieldControl;
    MapNodeDebugControl debugControl;
    int row = 22;
    int col = 40;
    int offsetX = -11;
    int offsetY = -20;

    //初始化
    public void Init()
    {

    }

    public void OnEnableMapNode(Dictionary<byte, List<string>> nodeDict)
    {
        // flowFieldControl = new GameObject("node").AddComponent<FlowFieldControl>();
        // flowFieldControl.transform.SetParent(transform);

        Vector2Int gridSize = new Vector2Int(row, col);

        // 设置中心周围3圈的节点为Base
        int centerX = -offsetX;
        int centerY = -offsetY;
        // 设置中心周围的矩形区域为Base
        int startX = centerX - 2; // 矩形起始X坐标
        int startY = centerY - 1; // 矩形起始Y坐标
        int endX = centerX + 1; // 矩形结束X坐标
        int endY = centerY + 2; // 矩形结束Y坐标

        //将坐标的值添加到nodeDict中，再利用nodeDict完成初始化
        //可能要考虑初始化的数据传递是否合理
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                // 跳过中心点
                if (x == centerX && y == centerY)
                {
                    LevelRawData.AddNodeToDict(ref nodeDict, (byte)NodeDirectionUtility.NodeCostType.Target, x, y);
                    continue;
                }
                if (x >= 0 && x < row && y >= 0 && y < col)
                {
                    LevelRawData.AddNodeToDict(ref nodeDict, (byte)NodeDirectionUtility.NodeCostType.Base, x, y);
                }
            }
        }
        // flowFieldControl.Init(gridSize, 0.5f, offsetX, offsetY, nodeDict);
    }

    public void OnRecalculateFlowField()
    {
        // flowFieldControl.OnRecalculateFlowField();
    }

    #region 辅助方法

    public NodeArgs GetNodeFromXY(int x, int y)
    {
        // return flowFieldControl.flowField.GetNode(x, y);
        return null;
    }

    // // 获取随机可用节点
    // // 可优化成每次计算存下可用节点，然后随机选择
    // // 但因为触发次数不频繁，且格子不大，可以直接遍历
    //  public Vector2Int GetRandomAvailableNode()
    // {
    //     // 获取流场控制器的节点数组
    //     NodeArgs[,] nodeArray = flowFieldControl.flowField.nodeArray;
    //     // 创建可用节点列表
    //     List<NodeArgs> availableNodes = new List<NodeArgs>();
        
    //     // 计算中间一半的范围，太外面的区域没有必要使用
    //     int startX = row / 4;      // 从1/4处开始
    //     int endX = row * 3 / 4;    // 到3/4处结束
    //     int startY = col / 4;      // 从1/4处开始
    //     int endY = col * 3 / 4;    // 到3/4处结束
        
    //     // 遍历中间一半的节点
    //     for (int x = startX; x < endX; x++)
    //     {
    //         for (int y = startY; y < endY; y++)
    //         {
    //             // 如果节点成本为Normal（可通行）
    //             if (nodeArray[x, y].cost == (byte)NodeDirectionUtility.NodeCostType.Normal)
    //             {
    //                 // 将可用节点加入列表
    //                 availableNodes.Add(nodeArray[x, y]);
    //             }
    //         }
    //     }
    //     // 从可用节点列表中随机选择一个节点，返回其网格坐标
    //     return availableNodes[Random.Range(0, availableNodes.Count)].gridXY;
    // }
    // //根据坐标信息，获得最近的可用节点
    // public Vector2 GetClosestAvailableGridAtPos(Vector2 worldPos)
    // {
    // //没有流场时，直接返回世界坐标
    //     if(flowFieldControl==null || flowFieldControl.flowField==null) return worldPos;
    // //根据坐标计算起始点
    //     NodeArgs start = flowFieldControl.flowField.GetNodeFromWorldPos(worldPos);
    // //如果该节点可用，直接返回坐标
    //     if(start.cost == (byte)NodeDirectionUtility.NodeCostType.Normal)
    //     {
    //         return worldPos;
    //     }
    // //否则开始做填充
    //     else
    //     {
    //         Queue<NodeArgs> front = new Queue<NodeArgs>(); //创建填充队列
    //         List<NodeArgs> reached = new List<NodeArgs>(); //创建已搜索列表
    //         front.Enqueue(start);
    //         reached.Add(start);
    //         do
    //         {
    //             var node = front.Dequeue();
    //             foreach(var neightbor in GetNeightbor(node))
    //             {
    //                 if(!reached.Contains(neightbor))
    //                 {
    //                 //当找到可用节点时，返回坐标
    //                     if(neightbor.cost == (byte)NodeDirectionUtility.NodeCostType.Normal)
    //                         return neightbor.worldPos;
    //                     reached.Add(neightbor);
    //                     front.Enqueue(neightbor);
    //                 }
    //             }
    //         }while(front.Count>0);
    //     //无到可用节点时，返回自身坐标
    //         return start.worldPos;
    //     }
    // }
    //获取相邻节点
    NodeArgs[] GetNeightbor(NodeArgs node)
    {
        List<NodeArgs> neightbors = new List<NodeArgs>(4);
        Vector2Int grid = node.gridXY;
        // if(grid.x > 0)
        // {
        //     neightbors.Add(flowFieldControl.flowField.nodeArray[grid.x-1, grid.y]);
        // }
        // if(grid.x < row-1)
        // {
        //     neightbors.Add(flowFieldControl.flowField.nodeArray[grid.x+1, grid.y]);
        // }
        // if(grid.y < col-1)
        // {
        //     neightbors.Add(flowFieldControl.flowField.nodeArray[grid.x, grid.y+1]);
        // }
        // if(grid.y > 0)
        // {
        //     neightbors.Add(flowFieldControl.flowField.nodeArray[grid.x, grid.y-1]);
        // }

        return neightbors.ToArray();
    }
    #endregion

    //#region Editor 编辑器用
    //关闭地图节点
    public void OnDisableMapNode()
    {
        debugControl.OnClear();
        // FlowFieldControl.Instance.OnClear();
    }

    public void OnShowNodeGizmos()
    {
        // flowFieldControl.gameObject.SetActive(true);
    }

    public void OnHideNodeGizmos()
    {
        // flowFieldControl.gameObject.SetActive(false);
    }

    public void InitEditor()
    {
        Init();
    }

    //设置可用节点, 默认cost为1
    public void SetNodeAvailable(Vector2 worldPos, int range = 0)
    {
        // NodeArgs nodeArgs = flowFieldControl.flowField.GetNodeFromWorldPos(worldPos);
        // Debug.Log($"=== SetNodeAvailable: 设置可用节点 {nodeArgs.gridXY.x},{nodeArgs.gridXY.y} ===");
        // nodeArgs.ResetCost();
        // MapControl.Instance.LevelData.RemoveNode((byte)NodeDirectionUtility.NodeCostType.Block, nodeArgs.gridXY.x, nodeArgs.gridXY.y);
        // flowFieldControl.flowField.UpdateCostField(MapControl.Instance.LevelData.nodeData);
        // debugControl.OnRefreshNode();
    }

    //设置不可用节点, cost为255
    public void SetNodeBlock(Vector2 worldPos, int range = 0)
    {
        // NodeArgs nodeArgs = flowFieldControl.flowField.GetNodeFromWorldPos(worldPos);
        // Debug.Log($"=== SetNodeAvailable: 设置Block节点 {nodeArgs.gridXY.x},{nodeArgs.gridXY.y} ===");
       
        // List<NodeArgs> listNodeArgs = new List<NodeArgs>();
        // listNodeArgs.Add(nodeArgs);
        // // 遍历周围节点
        // for (int x = -range; x <= range; x++)
        // {
        //     for (int y = -range; y <= range; y++)
        //     {
        //         // 跳过中心节点
        //         if (x == 0 && y == 0) continue;

        //         // 计算目标节点的位置
        //         int targetX = nodeArgs.gridXY.x + x;
        //         int targetY = nodeArgs.gridXY.y + y;

        //         // 获取目标节点
        //         NodeArgs targetNode = flowFieldControl.flowField.GetNode(targetX, targetY);
        //         if (targetNode != null)
        //         {
        //             if(targetNode.cost == (byte)NodeDirectionUtility.NodeCostType.Base)
        //             {
        //                 //基地节点跳过
        //                 continue;
        //             }
        //             if(targetNode.cost == (byte)NodeDirectionUtility.NodeCostType.Block)
        //             {
        //                 //阻挡节点跳过
        //                 continue;
        //             }
        //             listNodeArgs.Add(targetNode);
        //         }
        //     }
        // }

        // byte cost = (byte)NodeDirectionUtility.NodeCostType.Block;
        // foreach(var node in listNodeArgs)
        // {
        //     node.SetCost(cost);
        //     //如果节点是Normal，则添加到节点数据中Block的cost中，如果是其他的话则跳过
        //     MapControl.Instance.LevelData.AddNode(cost, node.gridXY.x, node.gridXY.y);
        // }

        // flowFieldControl.flowField.UpdateCostField(MapControl.Instance.LevelData.nodeData);
        // debugControl.OnRefreshNode();
    }

/*
    public void OnPlaceNodeCollider(Vector2 worldPos)
    {
        NodeArgs nodeArgs = flowFieldControl.flowField.GetNodeFromWorldPos(worldPos);
        MapControl.Instance.LevelData.AddCollider(nodeArgs.gridXY.x, nodeArgs.gridXY.y);
        Debug.Log($"=== PlaceNodeCollider: 设置碰撞体 {nodeArgs.gridXY.x},{nodeArgs.gridXY.y} ===");
        //debugControl.OnRefresh();
        debugControl.OnRefreshCollider();
    }

    public void OnRemoveNodeCollider(Vector2 worldPos)
    {
        NodeArgs nodeArgs = flowFieldControl.flowField.GetNodeFromWorldPos(worldPos);
        MapControl.Instance.LevelData.RemoveCollider(nodeArgs.gridXY.x, nodeArgs.gridXY.y);
        Debug.Log($"=== RemoveNodeCollider: 移除碰撞体 {nodeArgs.gridXY.x},{nodeArgs.gridXY.y} ===");
        //debugControl.OnRefresh();
        debugControl.OnRefreshCollider();
    }
*/


#if UNITY_EDITOR
    //如果需要拓展格子大小，需要把数据都清楚掉
    public void OnClearAllNode()
    {
        // 显示确认弹窗
        bool isConfirmed = EditorUtility.DisplayDialog("确认操作", "确定要清除所有节点吗？", "确定", "取消");

        if (isConfirmed)
        {
            // 用户点击确认后执行的操作
            // flowFieldControl.OnClear();
            MapControl.Instance.LevelData.ClearAllNode();
            
            // 显示成功提示
            EditorUtility.DisplayDialog("操作成功", "清除成功！请点击保存，然后重新加载地图以生效。", "确定");
        }
        else
        {
            // 用户点击取消后的操作（可选）
            Debug.Log("用户取消了操作");
        }
    }

    public int GetNodeX(Vector2 worldPos)
    {
        // return flowFieldControl.flowField.GetNodeFromWorldPos(worldPos).gridXY.x;
        return 0;
    }

    public int GetNodeY(Vector2 worldPos)
    {
        // return flowFieldControl.flowField.GetNodeFromWorldPos(worldPos).gridXY.y;
        return 0;
    }
#endif
}

