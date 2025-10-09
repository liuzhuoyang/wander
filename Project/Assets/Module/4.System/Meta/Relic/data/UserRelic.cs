using System.Collections.Generic;

public class UserRelic
{
    public Dictionary<string, int> dictRelic;//-1表示未解锁
    public void InitData()
    {
        dictRelic = new Dictionary<string, int>();
    }
    public void UpdateOrInitializeData()
    {
        if (dictRelic == null || dictRelic.Count <= 0)
        {
            InitData();
        }
    }
}

