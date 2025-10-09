using System.Collections.Generic;

public class EventNameTip
{
    public const string EVENT_TIP_ON_UI = "EVENT_TIP_ON_UI";
}

public class UITipArgs : UIBaseArgs
{
    public TipType tipType;
    public string textTip;

    public float posX;
    public float posY;
    public string textTipUnlockTitle;
    public string textTipUnlockContent;

    public List<RewardArgs> rewardArgs;

    public int numberOrigin;
    public int numberResult;
}