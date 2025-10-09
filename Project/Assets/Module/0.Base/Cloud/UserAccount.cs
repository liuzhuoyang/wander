
public class UserAccount
{
    public string udid;       //唯一ID
    public int userID;        //显示数字名字
    public string openID;     //微信openID
    public long registerTime; //注册时间
    public long saveTime;
    public long loginTime;

    public void SetUserLoginTime(long timespan)
    {
        if (registerTime <= 0)
        {
            registerTime = timespan;
        }

        loginTime = timespan;
    }
}

public class UserServer
{
    public long serverRegisterTime;
    public int serverID;
    public int serverRandomSeed;
    public int initialUserID;
}

public class UserCharacter
{
    public int serverID = 1;
    public string displayName = "Character A";
    public int power = 999;
    public int level = 1;
    //public string teamID = "";
}