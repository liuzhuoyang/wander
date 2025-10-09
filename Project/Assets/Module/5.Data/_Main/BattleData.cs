using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class BattleData
{
    public static int enemyKilled;
    public static int mergeCount;
    public static int battleTokenEarned;
    public static bool isPassLevel;

    //战斗货币
    public static Subject<int> battleToken;

    public static void InitBattleData()
    {
        enemyKilled = 0;
    }
}
