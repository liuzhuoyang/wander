using System.Collections;
using System.Collections.Generic;

public class EventNameFeature
{
    public const string EVENT_FEATURE_UNLOCK_OPEN_UI = "EVENT_FEATURE_UNLOCK_OPEN_UI";
    public const string EVENT_FEATURE_UNLOCK_TRIGGER_UI = "EVENT_FEATURE_UNLOCK_TRIGGER_UI";
    //解锁功能后更新导航红点
    public const string EVENT_FEATURE_UNLOCK_REFRESH_ROADMAP = "EVENT_FEATURE_UNLOCK_REFRESH_ROADMAP";
}

public class UIFeatureArgs : UIBaseArgs
{
    public FeatureType featureType;
}