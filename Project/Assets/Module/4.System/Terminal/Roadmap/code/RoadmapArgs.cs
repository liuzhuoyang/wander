using System.Collections.Generic;

public class EventNameRoadmap
{
    public const string EVENT_ROADMAP_ON_REFRESH_UI = "EVENT_ROADMAP_ON_REFRESH_UI";
    public const string EVENT_ROADMAP_ON_REFRESH_CLAIM = "EVENT_ROADMAP_ON_REFRESH_CLAIM";
}

public class UIRoadmapArgs : UIBaseArgs
{
    public List<RoadmapSlotArgs> listRoadmapSlot;
}

public class RoadmapSlotArgs
{
    public FeatureType featureType;
    public bool isClaimed;
    public bool canClaim;
}