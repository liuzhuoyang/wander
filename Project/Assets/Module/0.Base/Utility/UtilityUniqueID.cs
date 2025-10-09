using UnityEngine;

//生成唯一ID，全局递增
public static class UtilityUniqueID
{
    static int currentID;

    public static int GetNextUniqueID()
    {        
        if (currentID >= int.MaxValue)
        {
            currentID = 0;
        }

        currentID++;
        return currentID;
    }
}
