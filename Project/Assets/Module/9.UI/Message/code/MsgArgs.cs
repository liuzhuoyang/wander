using System;

public class EventNameMsg
{
    public const string EVENT_MESSAGE_UI = "EVENT_MESSAGE_UI";
    public const string EVENT_MESSAGE_CLOSE_LOADING_UI = "EVENT_MESSAGE_CLOSE_LOADING_UI";
    public const string EVENT_MESSAGE_CLOSE_RANKING_UI = "EVENT_MESSAGE_CLOSE_RANKING_UI";
}

/*
public class UIBaseArgs : EventArgs
{
    public Action callbackClose;
    public Action callbackConfrim;
}*/

public class MsgArgs : EventArgs
{
    public Action onConfrim;
    public string content;
    public string target;
}

public class MsgRankingArgs : MsgArgs
{
    public bool isGetData;
    public Action onRetry;
    public Action onClose;
}