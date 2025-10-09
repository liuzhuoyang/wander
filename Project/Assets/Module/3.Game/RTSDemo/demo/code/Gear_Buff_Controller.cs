using BattleBuff;
using Sirenix.OdinInspector;
using UnityEngine;

public class Gear_Buff_Controller : MonoBehaviour
{
    [SerializeField] private Gear_Demo_Controller gear_Demo_Controller;
    [SerializeField] private BuffData_SO buffData_SO;
    [Button("Add Ability")]
    public void AddBuffToGear()
    {
        gear_Demo_Controller.m_currentGear.m_buffHandler.TryAddBuff(buffData_SO.m_buffID);
    }
    [Button("Remove Ability")]
    public void RemoveBuffFromGear()
    {
        gear_Demo_Controller.m_currentGear.m_buffHandler.RemoveBuff(buffData_SO.m_buffID);
    }
}