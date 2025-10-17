using UnityEngine;

using BattleGear;
using PlayerInteraction;
using RTSDemo.Grid;

public class GearDragInteraction : Draggable
{
    private Vector2 originalPos = Vector2.zero;
    private Vector2Int lastSnapGridPoint = Vector2Int.zero;
    private GearBase gearBase;

    void Start()
    {
        gearBase = GetComponentInParent<GearBase>();
        dragRoot = gearBase.transform;
        dragOffset = GearManager.Instance.GetGearPlaceOffset(gearBase.m_gearKey);
    }
    void OnEnable()
    {
        onBeginDrag += HandleBeginDrag;
        onEndDrag += HandleEndDrag;
    }
    void OnDisable()
    {
        onBeginDrag -= HandleBeginDrag;
        onEndDrag -= HandleEndDrag;
    }

    public override void HoldingUpdate(PlayerInputControl playerInput)
    {
        base.HoldingUpdate(playerInput);

        if (isDragging)
        {
            //实时检测位置是否有效
            Vector2Int gridPoint = RTSGridWorldSystem.Instance.GetGridPointFromWorld(playerInput.m_pointerWorldPos);
            if (lastSnapGridPoint != gridPoint)
            {
                lastSnapGridPoint = gridPoint;
                //发布检测事件
                GearEvent.Call_OnValidateGridForGear(new GearPlaceArg()
                {
                    gearKey = gearBase.m_gearKey,
                    snapPoint = gridPoint
                });
            }
        }
    }
    void HandleBeginDrag()
    {
        // Handle the beginning of the drag
        originalPos = gearBase.transform.position;
        gearBase.StopGear();
    }
    void HandleEndDrag(Vector2 wrdPos)
    {
        //以empty string作为检测关闭事件
        GearEvent.Call_OnValidateGridForGear(new GearPlaceArg()
        {
            gearKey = string.Empty,
            snapPoint = Vector2Int.zero
        });

        //放置武器
        Vector2Int gridPoint = GearManager.Instance.WorldPosToGridPoint(wrdPos);
        Vector2 snapPos = GearManager.Instance.GridPointToWorldPos(gridPoint);

        if (GearManager.Instance.IsGearPlaceableOnGridPoint(gearBase.m_gearKey, gridPoint))
        {
            gearBase.transform.position = snapPos + dragOffset;
        }
        else
        {
            gearBase.transform.position = originalPos;
        }

        gearBase.RestartGear();
    }
}
