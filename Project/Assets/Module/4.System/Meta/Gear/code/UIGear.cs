using UnityEngine;

public class UIGear : UIBase
{
    [SerializeField] Transform listTransform;



    [SerializeField] Transform unlockTransform;

    [SerializeField] Transform lockTransform;

    [SerializeField] GameObject gearSlotPrefab;




    void Awake()
    {
        EventManager.StartListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnDestroy()
    {
        EventManager.StopListening<UIGearArgs>(GearEventName.EVENT_GEAR_REFRESH_UI, OnRefresh);
    }

    void OnRefresh(UIGearArgs args)
    {

    }


    private void DestroyUnitContainer()
    {
        foreach (Transform child in unlockTransform)
        {
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in lockTransform)
        {
            child.gameObject.SetActive(false);
        }
    }


}
