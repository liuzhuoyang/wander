using UnityEngine;

using BattleGear;
using RTSDemo.Grid;

public class GearPlaceMarkerControl : MonoBehaviour
{
    [SerializeField] private SpriteRenderer placeMarkerSprite;
    void OnEnable()
    {
        GearEvent.E_OnValidateGridPointForGear += OnValidateGridPointForGear;
    }
    void OnDisable()
    {
        GearEvent.E_OnValidateGridPointForGear -= OnValidateGridPointForGear;
    }
    async void OnValidateGridPointForGear(GearPlaceArg gearPlaceArg)
    {
        if (string.IsNullOrEmpty(gearPlaceArg.gearKey))
        {
            placeMarkerSprite.enabled = false;
            return;
        }
        placeMarkerSprite.sprite = await GearManager.Instance.GetGearIcon(gearPlaceArg.gearKey);
        placeMarkerSprite.transform.position = RTS_GridWorldSystem.Instance.GetWorldPosFromGrid(gearPlaceArg.snapPoint) + GearManager.Instance.GetGearPlaceOffset(gearPlaceArg.gearKey);
        if (RTS_GridWorldSystem.Instance.GetGridNode(gearPlaceArg.snapPoint).isMountable)
        {
            placeMarkerSprite.enabled = true;
            
            if (GearManager.Instance.IsGearPlaceableOnGridPoint(gearPlaceArg.gearKey, gearPlaceArg.snapPoint))
                placeMarkerSprite.color = Color.green;
            else
                placeMarkerSprite.color = Color.red;
        }
        else
        {
            placeMarkerSprite.enabled = false;
        }
    }
}
