using System.Collections.Generic;

public class UserMarket
{
    public Dictionary<string, int> dictMarketSlot;
    public void InitData()
    {
        dictMarketSlot = new Dictionary<string, int>();
    }

    public void OnResetDaily()
    {
        //删除数据中重置类型为每日的数据
        dictMarketSlot.Clear();
    }
}
