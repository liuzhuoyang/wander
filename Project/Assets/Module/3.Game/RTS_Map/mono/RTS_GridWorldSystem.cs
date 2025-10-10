using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTS_GridWorldSystem : Singleton<RTS_GridWorldSystem>
    {
        private RTS_GridWorld gridWorld;

        public void Init(RTS_GridWorld gridWorld) => this.gridWorld = gridWorld;
        public bool HasNode()=>gridWorld!=null && gridWorld.m_hasNodes;
        public RTS_GridNode GetGridNode(Vector2Int gridPoint) => gridWorld.GetNode(gridPoint);
        public RTS_GridNode GetGridNode(int gridX, int gridY) => GetGridNode(new Vector2Int(gridX, gridY));
        public RTS_GridNode GetNodeFromWorldPos(Vector2 worldPos) => gridWorld.GetNode(worldPos);
        public Vector2Int GetGridPointFromWorld(Vector2 worldPos) => gridWorld.GetGridPointFromWorld(worldPos);
        public Vector2 GetWorldPosFromGrid(Vector2Int gridPoint) => gridWorld.GetWorldPosFromGrid(gridPoint);
        public Vector2 GetWorldPosFromGrid(int gridX, int gridY) => GetWorldPosFromGrid(new Vector2Int(gridX, gridY));
        public float GetGridWidth() => gridWorld.m_gridWidth;
        public void UpdateFlowField(System.Collections.Generic.Dictionary<Vector2Int, byte> costDict) => gridWorld.UpdateCostField(costDict);
    }
}