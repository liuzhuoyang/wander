using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowField
{
    public NodeArgs[,] nodeArray { get; private set; }
    public Vector2Int gridXY { get; private set; }
    public float nodeRadius { get; private set; }
    public NodeArgs targetNode;

    //节点直径
    private float nodeDiameter;

    int offsetX = 0;
    int offsetY = 0;

    public FlowField(float nodeRadius, Vector2Int gridXY, int offsetX, int offsetY)
    {
        this.nodeRadius = nodeRadius;
        nodeDiameter = this.nodeRadius * 2f;
        this.gridXY = gridXY;
        this.offsetX = offsetX;
        this.offsetY = offsetY;
    }
    
    //创建节点
    public void OnCreateNode()
    {
        nodeArray = new NodeArgs[gridXY.x, gridXY.y];

        for (int x = 0; x < gridXY.x; x++)
        {
            for (int y = 0; y < gridXY.y; y++)
            {
                Vector3 worldPos = new Vector3(nodeDiameter * x + nodeRadius + offsetX, nodeDiameter * y + nodeRadius + offsetY, 0);
                nodeArray[x, y] = new NodeArgs(worldPos, new Vector2Int(x, y));
            }
        }
    }

    //清除节点
    public void OnClearNode()
    {
        nodeArray = null;
        targetNode = null;
    }

    //更新成本场
    public void UpdateCostField(Dictionary<byte, List<string>> nodeDict)
    {
        foreach (byte cost in nodeDict.Keys)
        {
            foreach (string node in nodeDict[cost])
            {
                string[] nodePos = node.Split(',');
                int x = int.Parse(nodePos[0]);
                int y = int.Parse(nodePos[1]);
                nodeArray[x, y].SetCost(cost);
            }
        }
    }

    //更新积分场,离目标点越远，积分约高，优先级越低
    public void UpdateIntegrationField(NodeArgs targetNode)
    {
        this.targetNode = targetNode;

        this.targetNode.cost = (byte)NodeDirectionUtility.NodeCostType.Target;
        this.targetNode.bestCost = 0;

        Queue<NodeArgs> listNodeToCheck = new Queue<NodeArgs>();

        listNodeToCheck.Enqueue(this.targetNode);

        while (listNodeToCheck.Count > 0)
        {
            NodeArgs nodeArgs = listNodeToCheck.Dequeue();
            List<NodeArgs> listNeighborNode = GetNeighborNodeList(nodeArgs.gridXY, NodeDirectionUtility.CardinalDirections);
            foreach (NodeArgs neighborNode in listNeighborNode)
            {
                // 将block节点也纳入cost计算范围
                // if (neighborNode.cost == byte.MaxValue) { continue; }
                if (neighborNode.cost + nodeArgs.bestCost < neighborNode.bestCost)
                {
                    neighborNode.bestCost = (ushort)(neighborNode.cost + nodeArgs.bestCost);
                    listNodeToCheck.Enqueue(neighborNode);
                }
            }
        }
    }

    //创建路径场
    public void CreateFlowField()
    {
        foreach (NodeArgs nodeArgs in nodeArray)
        {
            //根据节点类型，选取相邻节点。阻挡节点会指向最近的非阻挡节点
            List<NodeArgs> listNeighborNode = new List<NodeArgs>();
            switch (nodeArgs.cost)
            {
                case (byte)NodeDirectionUtility.NodeCostType.Block:
                    listNeighborNode = GetNeighborNodeList(nodeArgs.gridXY, NodeDirectionUtility.CardinalDirections);
                    break;
                default:
                    listNeighborNode = GetNeighborNodeList(nodeArgs.gridXY, NodeDirectionUtility.AllDirections);
                    break;
            }
            
            int bestCost = nodeArgs.bestCost;

            NodeArgs nextNode = null;
            bool bestIsAdjacent = false;
            foreach (NodeArgs neighborNode in listNeighborNode)
            {
                if (neighborNode.bestCost < bestCost)
                {
                    nextNode = neighborNode;
                    bestCost = neighborNode.bestCost;
                    nodeArgs.bestDirection = NodeDirectionUtility.GetDirectionFromVectorXY(neighborNode.gridXY - nodeArgs.gridXY);
                    bestIsAdjacent = NodeDirectionUtility.IsAdjacent(nodeArgs.bestDirection);
                }
            //判断是否有等值的非斜线节点
                if(neighborNode.bestCost == bestCost && 
                   bestIsAdjacent && 
                   !NodeDirectionUtility.IsAdjacent(NodeDirectionUtility.GetDirectionFromVectorXY(neighborNode.gridXY - nodeArgs.gridXY)))
                {
                    nextNode = neighborNode;
                    bestCost = neighborNode.bestCost;
                    nodeArgs.bestDirection = NodeDirectionUtility.GetDirectionFromVectorXY(neighborNode.gridXY - nodeArgs.gridXY);
                    bestIsAdjacent = NodeDirectionUtility.IsAdjacent(nodeArgs.bestDirection);
                }
            }
            
            if(nextNode != null && (nextNode.cost == 0 || nextNode.cost == 2))
            {
                nodeArgs.bestDirection = NodeDirectionUtility.None;
                continue;
            }
        }
    }
    //获取邻居节点
    private List<NodeArgs> GetNeighborNodeList(Vector2Int nodeIndex, List<NodeDirectionUtility> directions)
    {
        List<NodeArgs> ListNeighborNode = new List<NodeArgs>();

        foreach (Vector2Int curDirection in directions)
        {
            NodeArgs newNeighbor = GetNodeAtRelativePos(nodeIndex, curDirection);
            if (newNeighbor != null)
            {
                ListNeighborNode.Add(newNeighbor);
            }
        }
        return ListNeighborNode;
    }
    
    //获取相对位置的节点
    private NodeArgs GetNodeAtRelativePos(Vector2Int orignPos, Vector2Int relativePos)
    {
        Vector2Int finalPos = orignPos + relativePos;

        if (finalPos.x < 0 || finalPos.x >= gridXY.x || finalPos.y < 0 || finalPos.y >= gridXY.y)
        {
            return null;
        }

        else { return nodeArray[finalPos.x, finalPos.y]; }
    }

    //获取世界坐标对应的节点
    public NodeArgs GetNodeFromWorldPos(Vector3 worldPos)
    {
        float percentX = (worldPos.x - offsetX) / (gridXY.x * nodeDiameter);
        float percentY = (worldPos.y - offsetY) / (gridXY.y * nodeDiameter);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.FloorToInt((gridXY.x) * percentX), 0, gridXY.x - 1);
        int y = Mathf.Clamp(Mathf.FloorToInt((gridXY.y) * percentY), 0, gridXY.y - 1);
        return nodeArray[x, y];
    }

    public NodeArgs GetNode(int x, int y)
    {
        if (x < 0 || x >= gridXY.x || y < 0 || y >= gridXY.y)
        {
            return null;
        }
        return nodeArray[x, y];
    }
}