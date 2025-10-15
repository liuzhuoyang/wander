using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTSGridNode : FlowFieldNode
    {
        public bool isMountable => true;
        public RTSGridNode(Vector2 worldPos, Vector2Int gridXY):base(worldPos, gridXY){}
    }
}