using System.Collections.Generic;

public class UserPet
{
    public Dictionary<string, string> dictPet;//武器名gearName - 宠物名petName
    public List<UserPetData> listPet;
    public void InitData()
    {
        dictPet = new Dictionary<string, string>();
        listPet = new List<UserPetData>();
    }
}

public class UserPetData
{
    public string petName;
    public int level;
    public int star;
}

