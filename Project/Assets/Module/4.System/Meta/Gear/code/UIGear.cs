using UnityEngine;

public class UIGear : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventManager.StartListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnRefresh(UIGearArgs args)
    {
        //刷新UI
    }


}
