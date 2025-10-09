using UnityEngine;

public class UserStats
{
    public int highestPower;

    public int totalPlayTime;
    public int battleCount;
    public int winCount;
    public int failCount;
    public int mergeCount;
    public int wavePassed;
    public int battleTokenEarned;
    public int enemyKill;
    public int eliteKill;
    public int bossKill;


    public UserStats ()
    {

    }

    public void OnLevelWin()
    {
        winCount++;
    }
    
    public void OnLevelFail()
    {
        failCount++;
    }

    public void OnLevelStart()
    {
        battleCount++;
    }

    public void OnBattleMerge()
    {
        mergeCount++;
    }

    public void OnWavePassed()
    {
        wavePassed++;
    }

    public void OnBattleTokenEarned(int num)
    {
        battleTokenEarned += num;
    }

    public void OnEnemyKill()
    {
        enemyKill++;
    }

    public void OnEliteKill()
    {
        eliteKill++;
    }

    public void OnBossKill()
    {
        bossKill++;
    }
}