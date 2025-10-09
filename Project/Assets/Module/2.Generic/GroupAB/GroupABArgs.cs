using System.Collections.Generic;

public class GroupABArgs
{
    public GroupABType groupABType;
    public bool isOpen;
    public List<GroupABBattleLevelArgs> listBattleLevel;
}

public class GroupABBattleLevelArgs
{
    public int chapter;
    public int level;
    public int unitLevel;
    public float levelGrowth;
}

public enum GroupABType
{
    None,
    A,
    B
}