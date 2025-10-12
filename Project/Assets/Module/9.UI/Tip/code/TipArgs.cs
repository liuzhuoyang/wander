
public class EventNameTip
{
    public const string EVENT_TIP_ON_UI = "EVENT_TIP_ON_UI";
}

//普通类型提示，只有一行文字
public class UITipArgs : UIBaseArgs
{
    public TipType tipType;
    public string textTip;
}

//带框框的类型提示
public class UITipFrameArgs : UITipArgs
{
    public float posX;
    public float posY;
}

//自定义类型提示
public class UITipCustomArgs : UITipArgs
{

}