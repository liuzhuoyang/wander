public class EventNameTime
{
    public const string EVENT_TIME_TICK = "EVENT_TIME_TICK";
}

public class TimeArgs : EventArgs
{
    public int secPassedSinceStart;
}
