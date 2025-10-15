using BattleMap.Grid.Builder;
using BattleMap.Grid.FlowField;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RTSDemo.Grid
{
    public class RTSGridWorldBuilder : GridBuilder<RTSGridWorld, RTSGridNode>
    {
        private RTSGridWorld gridWorld;
        void Start()
        {
            gridWorld = BuildGrid();
            RTSGridWorldSystem.Instance.Init(gridWorld);
        }
        void OnEnable()
        {
            RTSGridEvent.E_OnGridNodeChange += RefreshGridWorld;
        }
        void OnDisable()
        {
            RTSGridEvent.E_OnGridNodeChange -= RefreshGridWorld;
        }
        protected override RTSGridWorld CreateGraph() => new RTSGridWorld(nodeWidth, gridSize, gridOffset.x, gridOffset.y);

        [Button("Refresh Grid World")]
        public void RefreshGridWorld()
        {
            Start();
        }
        void OnDrawGizmosSelected()
        {
            if (RTSGridWorldSystem.Instance != null && gridWorld != null && gridWorld.m_hasNodes)
            {
                for (int i = 0; i < gridSize.x; i++)
                {
                    for (int j = 0; j < gridSize.y; j++)
                    {
                        Vector3 position = new Vector3(gridOffset.x + i, gridOffset.y + j, 0);
                        var node = gridWorld.GetNode(position);
                        Vector2 dir = node.bestDirection.Vector;
                        if (node.cost == (byte)FlowFieldNodeDirectionUtility.NodeCostType.Block)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawCube(position, Vector3.one * 0.5f);
                        }
                        else if (node.cost == (byte)FlowFieldNodeDirectionUtility.NodeCostType.Target)
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawSphere(position, 0.2f);
                        }
                        else
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawLine(position, position - (Vector3)dir.normalized * 0.3f);
                            Gizmos.color = Color.green;
                            Gizmos.DrawLine(position, position + (Vector3)dir.normalized * 0.3f);
                        }
                    }
                }
            }
            else
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < gridSize.x; i++)
                {
                    for (int j = 0; j < gridSize.y; j++)
                    {
                        Vector3 position = new Vector3(gridOffset.x + i, gridOffset.y + j, 0);
                        Gizmos.DrawSphere(position, 0.1f);
                    }
                }
            }
        }
    }
}