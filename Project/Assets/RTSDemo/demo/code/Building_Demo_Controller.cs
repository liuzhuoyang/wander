using UnityEngine;
using BattleActor.Building;

public class Building_Demo_Controller : MonoBehaviour
{
    [SerializeField] private BuildingData_SO buildingData_SO;
    void Start()
    {
        BuildingManager.Instance.CreateBuilding(buildingData_SO.m_actorKey, 1, Vector2.zero, false, true);
    }
}