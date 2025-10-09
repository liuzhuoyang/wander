using System.Collections.Generic;

public class EventNameMail
{
    public const string EVENT_MAIL_ON_INIT_UI = "EVENT_MAIL_ON_INIT_UI";
    public const string EVENT_MAIL_ON_REFRESH_UI = "EVENT_MAIL_ON_REFRESH_UI";
    public const string EVENT_MAIL_OPEN_DETAIL_UI = "EVENT_MAIL_OPEN_DETAIL_UI";
    public const string EVENT_MAIL_OPEN_FUNCTION_DETAIL_UI = "EVENT_MAIL_OPEN_FUNCTION_DETAIL_UI";
}

public class MailListArgs : EventArgs
{
    public List<MailArgs> mailArgs;
}
public class MailArgs : EventArgs
{
    public int id;
    public string title;
    public string content;
    public string reward;
    //0未打开 1有奖励已打开未领取 2没奖励已打开或有奖励已领取 3已删除
    public int status;
    public int time;
    public bool isGM = false;
    public MailFunctionType functionType = MailFunctionType.None;
}

public enum MailFunctionType
{
    None = 0,
    ChangeChapter = 1,
}