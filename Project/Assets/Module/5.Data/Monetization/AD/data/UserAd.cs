using System.Collections.Generic;

public class UserAd
{
    public Dictionary<AdType, int> dictAdCount;//每日重置
    public void InitData()
    {
        dictAdCount = new Dictionary<AdType, int>();
    }

    public void OnResetDaily()
    {
        dictAdCount.Clear();
    }
}
