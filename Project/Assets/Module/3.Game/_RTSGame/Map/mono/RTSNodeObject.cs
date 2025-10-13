using BattleMap.Grid.Builder;
using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTSNodeObject : GridNodeObj<RTSGridWorld, RTSGridNode>
    {
        [SerializeField] private FlowFieldNodeDirectionUtility.NodeCostType nodeCost;
        [SerializeField] private bool isMountPoint = false;

        public override RTSGridNode GetGridNode(RTSGridWorld gridWorld)
        {
            RTSGridNode node = new RTSGridNode(isMountPoint, transform.position, gridWorld.GetGridPointFromWorld(transform.position));
            node.SetCost((byte)nodeCost);
            return node;
        }
    }
}