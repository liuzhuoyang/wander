using System.Collections.Generic;

public class UserTalent
{
    public int advancedLevel;
    public int takeRewardLevel;
    public List<int> listTalentLevel;
    public void InitData()
    {
        takeRewardLevel = 0;
        advancedLevel = 0;
        listTalentLevel = new List<int>() { 0, 0, 0 };
    }
}
