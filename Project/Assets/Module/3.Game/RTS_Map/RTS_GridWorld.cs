using System.Collections.Generic;
using BattleMap.Grid;
using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    [System.Serializable]
    public class RTS_GridWorld : GridGraph<RTS_GridNode>
    {
        private FlowField flowField;
        public RTS_GridWorld(float nodeWidth, Vector2Int gridXY, int offsetX, int offsetY) : base(nodeWidth, gridXY, offsetX, offsetY)
        {
            for (int x = 0; x < gridRange.x; x++)
            {
                for (int y = 0; y < gridRange.y; y++)
                {
                    Vector3 worldPos = new Vector3(nodeWidth * x + gridOffset.x, nodeWidth * y + gridOffset.y, 0);
                    nodes[x, y] = new RTS_GridNode(false, worldPos, new Vector2Int(x, y));
                }
            }
            //创建一个flowfield，该flowfield与grid world共享节点
            flowField = new FlowField(nodeWidth, nodes, offsetX, offsetY);
        }
        protected override internal void OnNodeUpdated(Dictionary<Vector2Int, RTS_GridNode> nodeDict)
        {
            //将更新后的节点数据传递给flowfield并执行刷新
            var flowfieldDict = new Dictionary<Vector2Int, FlowFieldNode>();
            foreach (var item in nodeDict)
            {
                flowfieldDict.Add(item.Key, item.Value);
            }
            flowField.OnNodeUpdated(flowfieldDict);
        }
        public void UpdateCostField(Dictionary<Vector2Int, byte> costDict)
        {
            flowField.UpdateCostField(costDict);
        }
    }
}
