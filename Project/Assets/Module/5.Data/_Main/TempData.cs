using UnityEngine;

public static class TempData
{
    public static bool isUnknownUser;           //来源不明玩家
    public static Vector3 lobbyPlanetPosition;  //大厅星球位置

    public static void Init()
    {
        isUnknownUser = false;
    }

    public static void Reset()
    {

    }
}
