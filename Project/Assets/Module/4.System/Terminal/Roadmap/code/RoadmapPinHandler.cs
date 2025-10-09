using UnityEngine;

//Roadmap Pin的检测条件
public class RoadmapPinHandler : MonoBehaviour
{
    private const string ROADMAP_PIN_ID = "pin_roadmap";
    public void Init()
    {
        CheckRoadmapPin();
    }
    public void CheckRoadmapPin()
    {
        bool isCanClaim = RoadmapSystem.Instance.IsCanClaim();
        //判断是否有可领取奖励
        if (isCanClaim)
        {
            EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(ROADMAP_PIN_ID, true));
            return;
        }
        EventManager.TriggerEvent(EventNamePin.EVENT_ON_UPDATE_PIN, new PinUpdateArgs(ROADMAP_PIN_ID, false));
    }
}
