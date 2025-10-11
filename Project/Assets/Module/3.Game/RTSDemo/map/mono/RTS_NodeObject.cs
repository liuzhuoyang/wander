using BattleMap.Grid.Builder;
using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTS_NodeObject : GridNodeObj<RTS_GridWorld, RTS_GridNode>
    {
        [SerializeField] private FlowFieldNodeDirectionUtility.NodeCostType nodeCost;
        [SerializeField] private bool isMountPoint = false;

        public override RTS_GridNode GetGridNode(RTS_GridWorld gridWorld)
        {
            RTS_GridNode node = new RTS_GridNode(isMountPoint, transform.position, gridWorld.GetGridPointFromWorld(transform.position));
            node.SetCost((byte)nodeCost);
            return node;
        }
    }
}