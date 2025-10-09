using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldControl : Singleton<FlowFieldControl>
{
    //节点数量
    public Vector2Int gridXY;
    //节点半径
    public float nodeRadius = 0.5f;
    //FlowField
    public FlowField flowField;

    //整个面板的偏移
    int offsetX = 0;
    int offsetY = 0;

    public async void Init(Vector2Int gridXY, float nodeRadius, int offsetX, int offsetY, Dictionary<byte, List<string>> nodeDict)
    {
        this.gridXY = gridXY;
        this.nodeRadius = nodeRadius;
        this.offsetX = offsetX;
        this.offsetY = offsetY;

        //初始化FlowField
        InitializeFlowField();

        //创建Cost场
        flowField.UpdateCostField(nodeDict);

        //创建一次路径场，后面定义目标节点后，需要再刷新一次
        flowField.CreateFlowField();

        if (MapNodeDebugControl.Instance != null)
        {
            //创建节点Debug视图
            GameObject prefabDebugView = await GameAsset.GetPrefabAsync("debug_map_node");
            foreach (NodeArgs node in flowField.nodeArray)
            {
                GameObject debugView = Instantiate(prefabDebugView);
                debugView.transform.SetParent(transform);
                debugView.transform.localPosition = new Vector3(node.worldPos.x, node.worldPos.y, 0);
                MapNodeDebugView debugViewComponent = debugView.GetComponent<MapNodeDebugView>();
                debugViewComponent.SetCost(node.cost);
                MapNodeDebugControl.Instance.AddNodeView(node.gridXY.x, node.gridXY.y, debugView);
            }
        }

        OnRecalculateFlowField();
    }

    public void OnRecalculateFlowField()
    {
        //定义目标节点
        NodeArgs targetNode = flowField.GetNodeFromWorldPos(new Vector3(0f, 0f, 0));
        targetNode.cost = (byte)NodeDirectionUtility.NodeCostType.Target;
        flowField.UpdateIntegrationField(targetNode);

        //创建路径场
        flowField.CreateFlowField();

        //刷新Debug视图
        if(MapNodeDebugControl.Instance != null)
        {
            MapNodeDebugControl.Instance.OnRefreshNode();   
        }        
    }

    public void RefreshBaseNode()
    {
        Vector2 minPos, maxPos;
        //BaseControl.Instance.GetMinMaxWallPoint(out minPos, out maxPos);
        minPos = new Vector2(-1, -1);
        maxPos = new Vector2(1, 1);
        NodeArgs minBasePoint = flowField.GetNodeFromWorldPos(minPos);
        NodeArgs maxBasePoint = flowField.GetNodeFromWorldPos(maxPos);

        for(int x = minBasePoint.gridXY.x; x<maxBasePoint.gridXY.x; x++){
            for(int y = minBasePoint.gridXY.y; y<maxBasePoint.gridXY.y; y++){
                bool isEdge = x == minBasePoint.gridXY.x || x == maxBasePoint.gridXY.x-1 || y == minBasePoint.gridXY.y || y == maxBasePoint.gridXY.y-1;
                if(isEdge)
                    flowField.nodeArray[x,y].SetCost((byte)NodeDirectionUtility.NodeCostType.Base);
                else
                    flowField.nodeArray[x,y].SetCost((byte)NodeDirectionUtility.NodeCostType.Target);
            }
        }

        OnRecalculateFlowField();
    }

    //初始化FlowField
    private void InitializeFlowField()
    {
        flowField = new FlowField(nodeRadius, gridXY, offsetX, offsetY);
        flowField.OnCreateNode();
    }

    public void OnClear()
    {
        flowField.OnClearNode();
    }
}
