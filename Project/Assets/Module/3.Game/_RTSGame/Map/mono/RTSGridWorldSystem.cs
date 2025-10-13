using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTSGridWorldSystem : Singleton<RTSGridWorldSystem>
    {
        private RTSGridWorld gridWorld;

        public void Init(RTSGridWorld gridWorld) => this.gridWorld = gridWorld;
        public bool HasNode()=>gridWorld!=null && gridWorld.m_hasNodes;
        public RTSGridNode GetGridNode(Vector2Int gridPoint) => gridWorld.GetNode(gridPoint);
        public RTSGridNode GetGridNode(int gridX, int gridY) => GetGridNode(new Vector2Int(gridX, gridY));
        public RTSGridNode GetNodeFromWorldPos(Vector2 worldPos) => gridWorld.GetNode(worldPos);
        public Vector2Int GetGridPointFromWorld(Vector2 worldPos) => gridWorld.GetGridPointFromWorld(worldPos);
        public Vector2 GetWorldPosFromGrid(Vector2Int gridPoint) => gridWorld.GetWorldPosFromGrid(gridPoint);
        public Vector2 GetWorldPosFromGrid(int gridX, int gridY) => GetWorldPosFromGrid(new Vector2Int(gridX, gridY));
        public float GetGridWidth() => gridWorld.m_gridWidth;
        public void UpdateFlowField(System.Collections.Generic.Dictionary<Vector2Int, byte> costDict) => gridWorld.UpdateCostField(costDict);
    }
}