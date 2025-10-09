using UnityEngine;

public static class UtilityGameSpeed 
{
    #region 游戏速度
    public static void OnChangeGameSpeeds(SpeedType speedType)
    {
        float gameSpeed = 1;

        switch (speedType)
        {
            case SpeedType.x1:
                gameSpeed = 1;
                break;
            case SpeedType.x2:
                gameSpeed = 1.5f;
                break;
            case SpeedType.x3:
                gameSpeed = 2;
                break;
        }
        
        Time.timeScale = gameSpeed;
    }

    //设置为1，但用户数据里还是原来的数，战斗结束后回到大厅用
    //进入战斗会根据用户记录再次设置速度
    public static void OnDefaultGameSpeed()
    {
        Time.timeScale = 1f;
    }
    #endregion
}
