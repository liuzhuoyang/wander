using System;
using UnityEngine;

namespace BattleGear
{
    public static class GearEvent
    {
        //拖拽Gear时检测格子的事件，每当Gear的吸附位置变化时触发
        public static Action<GearPlaceArg> E_OnValidateGridPointForGear;
        public static void Call_OnValidateGridForGear(GearPlaceArg arg) => E_OnValidateGridPointForGear?.Invoke(arg);

    }
    public struct GearPlaceArg
    {
        public string gearKey;
        public Vector2Int snapPoint;
    }
}
