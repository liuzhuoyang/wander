using System;
using UnityEngine;

namespace RTSDemo.Grid
{
    public static class RTSGridEvent
    {
        public static event Action E_OnGridNodeChange;
        public static void Call_OnGridNodeChange() => E_OnGridNodeChange?.Invoke();
    }
}