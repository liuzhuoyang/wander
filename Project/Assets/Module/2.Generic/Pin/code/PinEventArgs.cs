public static class EventNamePin
{
    public const string EVENT_ON_CHECK_PIN = "EVENT_ON_CHECK_PIN"; //刷新Pin的事件
    public const string EVENT_ON_REST_PIN = "EVENT_ON_REST_PIN"; //重置一次性Pin状态的事件
    public const string EVENT_ON_UPDATE_PIN = "EVENT_ON_UPDATE_PIN"; //将特定Pin节点设置为开启
}
public class PinRestArgs : EventArgs{}
public class PinUpdateArgs : EventArgs
{
    public string pinID;
    public bool newState;
    public PinUpdateArgs(string pinID, bool newState)
    {
        this.pinID = pinID;
        this.newState = newState;
    }
}
