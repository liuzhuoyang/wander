using System.Collections.Generic;

public class UserFund
{
    public Dictionary<string, UserFundData> dictFund;
    public void InitData()
    {
        dictFund = new Dictionary<string, UserFundData>();
    }
}

public class UserFundData
{
    public int rewardProgress;
    public int rewardProgressVIP;
    public bool isComplete;
    public int endTime;//结束时间（-1表示无限）
}
