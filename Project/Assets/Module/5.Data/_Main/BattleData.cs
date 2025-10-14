using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class BattleData
{
    public static bool isVictory; //战斗是否胜利
    public static int enemyKilled;
    public static int mergeCount;
    public static int battleTokenEarned;
    public static bool isPassLevel;

    //战斗货币
    public static Subject<int> battleToken;

    public static void InitBattleData()
    {
        isVictory = false; //战斗是否胜利，进入战斗时重制为false，达到条件后设置为true
        enemyKilled = 0;
    }
}
