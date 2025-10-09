public class UserCheckIn
{
    public int checkInDay;//当前签到天数
    public int canReceiveDay;//可领取天数
    public int isFirst;//是否是首次七日签到
    public void InitData()
    {
        checkInDay = 0;
        canReceiveDay = 0;
        isFirst = 1;
    }

    public void OnResetDaily()
    {
        if (checkInDay == 7)
        {
            checkInDay = 0;
            canReceiveDay = 0;
            isFirst = 0;
        }
        canReceiveDay = canReceiveDay >= 7 ? 7 : canReceiveDay + 1;
    }
}
