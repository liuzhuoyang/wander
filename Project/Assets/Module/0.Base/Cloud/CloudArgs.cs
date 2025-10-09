
public class NetTimeArgs
{
    public long timespan;
}

public class CheckVersionArgs
{
    public string platform;
    public bool isUnknownUser;

    public bool isUpdateNeeded;
    public string version;
    public string latestVersion;
    public string forcedVersion;
}

public class LinkStatusArgs
{
    public string facebookID;
    public string appleID;
    public string googleID;
}
