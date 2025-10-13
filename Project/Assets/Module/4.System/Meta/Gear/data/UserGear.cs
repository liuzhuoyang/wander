using System.Collections.Generic;

public class UserGear
{
    //上阵武器
    public Dictionary<int, string> dictEquipGear;

    //武器数据
    public Dictionary<string, UserGearArgs> dictGear;

    //允许上证武器的数量
    public int unlockGearCount;
    public UserGear()
    {
        InitData();
    }

    //武器临时数据
    public void InitData()
    {
        dictEquipGear = new Dictionary<int, string>();
        dictGear = new Dictionary<string, UserGearArgs>();
        unlockGearCount = 4;
        //@todo新号 解锁
        OnUnlockGear("001_shuriken");
        OnUnlockGear("002_sword");
        OnUnlockGear("003_arrow");
        OnUnlockGear("004_axe");
        //临时装备上述四个
        dictEquipGear.Add(0, "001_shuriken");
        dictEquipGear.Add(1, "002_sword");
        dictEquipGear.Add(2, "003_arrow");
        dictEquipGear.Add(3, "004_axe");
    }


    public void OnUnlockGear(string gearName)
    {
        if (dictGear.ContainsKey(gearName)) return;
        dictGear.Add(gearName, new UserGearArgs()
        {
            level = 1,
        });

        if (dictGear.Count >= unlockGearCount)
        {
            unlockGearCount = dictGear.Count;
        }

        if (unlockGearCount >= 8)
        {
            unlockGearCount = 8;
        }
    }

    public bool IsGearUnlocked(string gearName)
    {
        if (dictGear.ContainsKey(gearName))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

public class UserGearArgs
{
    //等级
    public int level;
}