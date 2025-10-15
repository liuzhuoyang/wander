using BattleMap.Grid.Builder;
using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTSNodeObject : GridNodeObj<RTSGridWorld, RTSGridNode>
    {
        [SerializeField] private FlowFieldNodeDirectionUtility.NodeCostType nodeCost;

        public override RTSGridNode GetGridNode(RTSGridWorld gridWorld)
        {
            RTSGridNode node = new RTSGridNode(transform.position, gridWorld.GetGridPointFromWorld(transform.position));
            node.SetCost((byte)nodeCost);
            return node;
        }
    }
}