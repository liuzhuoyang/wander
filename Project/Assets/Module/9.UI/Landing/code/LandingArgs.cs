public class EventNameLanding
{
    public const string EVENT_LOADING_REFRESH_UI = "EVENT_LOADING_REFRESH_UI";
    public const string EVENT_LOADING_CLOSE_UI = "EVENT_LOADING_CLOSE_UI";
}

public class LandingUIArgs : UIBaseArgs
{
    public float currentProgress;

    //提示文本，在等GameAssetsManager加载完毕后，有字体，才可以显示
    public bool isShowHintText;
}

