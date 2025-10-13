using UnityEngine;
using System.Collections.Generic;

public class UIGearArgs : UIBaseArgs
{
    //所有武器的数据
    public List<GearSlotViewArgs> listGearSlotViewArgs;

    public int unlockSlot;

    public Dictionary<int, string> dictEquipGear;

}

public class GearEventName
{
    public const string EVENT_GEAR_REFRESH_UI = "EVENT_GEAR_REFRESH_UI";
}