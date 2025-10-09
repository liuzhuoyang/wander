using System.Collections.Generic;

public class UserPrivilege
{
    public Dictionary<string, UserPrivilegeData> dictPrivilege;//赛季卡名seasonCardName - 赛季卡名
    public void InitData()
    {
        dictPrivilege = new Dictionary<string, UserPrivilegeData>();
    }

    public void OnResetDaily()
    {
        foreach (var item in dictPrivilege)
        {
            item.Value.claimed = false;
        }
    }
}

public class UserPrivilegeData
{
    public bool claimed;
}
