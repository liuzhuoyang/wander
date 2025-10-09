using UnityEngine;
using BattleLaunch.Demo;

public class Launch_Demo_Controller : MonoBehaviour
{
    [SerializeField] private SimpleLauncher gear;
    void Start()
    {
        gear.ResetGear();
    }
}
