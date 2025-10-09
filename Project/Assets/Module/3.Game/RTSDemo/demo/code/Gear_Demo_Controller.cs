using UnityEngine;
using BattleGear;
using Sirenix.OdinInspector;

public class Gear_Demo_Controller : MonoBehaviour
{
    [SerializeField] private GearData_SO gearData_SO;
    private GearBase currentGear;
    public GearBase m_currentGear => currentGear;
    void Start()
    {
        currentGear = GearManager.Instance.CreateGear(gearData_SO.m_gearKey, Vector2.zero, 1);
        currentGear.RestartGear();
    }

    [Button("Flush Gear")]
    public void FlushGear()
    {
        GearManager.Instance.RemoveAllGear();
        Start();
    }
}