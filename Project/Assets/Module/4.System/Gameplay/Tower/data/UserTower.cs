
public class UserTower
{
    public int currentFloor;//当前挑战的层数
    public bool claimed;//是否领取每日奖励
    public int dailyClaimFloor;//每日领取奖励的层数
    public bool rewardFloor;//阶段奖励是否领取
    public UserTower ()
    {
        currentFloor = 1;
        claimed = false;
        rewardFloor = false;
        dailyClaimFloor = 0;
    }

    public void OnResetDaily()
    {
        claimed = false;
        dailyClaimFloor = currentFloor - 1;
    }
}
