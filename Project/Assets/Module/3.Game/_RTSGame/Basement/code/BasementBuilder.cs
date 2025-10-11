using UnityEngine;

using RTSDemo.Grid;

namespace BattleActor.Basement
{
    public class BasementBuilder : MonoBehaviour
    {
        [SerializeField] private BasementData basementData_SO; 
        [SerializeField] private Vector2Int defaultGridSize = new Vector2Int(3, 3);
        [SerializeField] private RTSNodeObject basementCenterNode;
        async void Start()
        {
            await BasementControl.Instance.Init();
            Vector2Int targetGrid = RTSGridWorldSystem.Instance.GetGridPointFromWorld(basementCenterNode.transform.position);
            BasementControl.Instance.CreateBasement(basementData_SO.m_basementKey, targetGrid-defaultGridSize/2, defaultGridSize);
        }
    }
}