public static class UtilityReward
{
    
    #region 奖励拼接
    public static string GetRewardString(string reward, int count)
    {
        return reward + "^" + count;
    }

    public static (string reward, int count) GetRewardString(string rewardString)
    {
        string[] reward = rewardString.Split("^");
        return (reward[0], int.Parse(reward[1]));
    }
    #endregion
}
