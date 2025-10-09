using System.Collections.Generic;

public class UserGear
{
    public List<string> listEquipGear;
    public Dictionary<string, UserGearArgs> dictGear;
    public Dictionary<string, List<int>> dictVolatileData;
    public void InitData()
    {
        listEquipGear = new List<string>();
        dictGear = new Dictionary<string, UserGearArgs>();
        //@todo新号 解锁
        OnUnlockGear("101_gatling");
        OnUnlockGear("102_missile");
        OnUnlockGear("103_bomb");
        OnUnlockGear("104_frost");
        OnUnlockGear("203_mine");
        OnUnlockGear("201_drone");
        //临时装备上述六个
        listEquipGear.Add("101_gatling");
        listEquipGear.Add("102_missile");
        listEquipGear.Add("103_bomb");
        listEquipGear.Add("203_mine");
        listEquipGear.Add("104_frost");
        listEquipGear.Add("201_drone");
        dictVolatileData = new Dictionary<string, List<int>>();
    }
    public void OnUnlockGear(string gearName)
    {
        if (dictGear.ContainsKey(gearName)) return;
        dictGear.Add(gearName, new UserGearArgs()
        {
            level = 1,
            star = 0,
            evolve = 0
        });
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

    public bool IsUltimate2Unlock(string gearName)
    {
        if (dictGear.ContainsKey(gearName) && dictGear[gearName].star >= 3)
        {
            return true;
        }
        return false;
    }
}

public class UserGearArgs
{
    //等级
    public int level;
    //星级
    public int star;
    //觉醒
    public int evolve;
}