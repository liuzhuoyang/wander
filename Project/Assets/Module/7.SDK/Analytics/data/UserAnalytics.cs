public class UserAnalytics
{
    public int loginCount;       //登陆次数
    public int loginCountDaily;  //当日登陆次数
    public int loginDay;         //累计登陆天数

    public bool isPayDaily;             //用户当日是否有付费
    public int iapCount;                //总付费次数
    public float iapTotalValue;         //总付费额度

    public int rewardAdCount;           //总观看奖励广告次数
    public int rewardAdCountDaily;      //本日观看奖励广告次数
    public int interstitialAdCount;     //总观看插屏广告次数
    public int interstitialAdCountDaily;//本日观看插屏广告次数

    public GroupABType groupABType; //AB测试类型

    public UserAnalytics()
    {
        loginDay = 0; //累计登陆天数初始化为0,后续ResetDaily会加上1
        loginCount = 0;
        loginCountDaily = 0;
        isPayDaily = false;
        iapCount = 0;
        iapTotalValue = 0;
    }

    public void ResetDaily()
    {
        loginDay ++; //累计登陆天数+1
        loginCountDaily = 0;
        rewardAdCountDaily = 0;
        interstitialAdCountDaily = 0;
        isPayDaily = false;
    }
}