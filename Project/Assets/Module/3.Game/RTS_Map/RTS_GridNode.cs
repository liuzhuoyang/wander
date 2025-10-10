using BattleMap.Grid.FlowField;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTS_GridNode : FlowFieldNode
    {
        public bool isMountable { get; private set; }

        public RTS_GridNode(bool isMountable, Vector2 worldPos, Vector2Int gridXY):base(worldPos, gridXY)
        {
            this.isMountable = isMountable;
        }
        public void SetMountable(bool isMountable) => this.isMountable = isMountable;
    }
}