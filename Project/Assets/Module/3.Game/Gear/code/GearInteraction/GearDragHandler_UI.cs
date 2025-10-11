using UnityEngine;

using BattleGear;
using PlayerInteraction;

[RequireComponent(typeof(Draggable_UI))]
public class GearDragHandler_UI : MonoBehaviour
{
    [SerializeField] private GearData gearData;

    private Draggable_UI draggable;
    private Camera mainCam;
    private Vector2 initPos;
    private Vector2Int lastSnapGridPoint = Vector2Int.zero;

    void Awake()
    {
        mainCam = Camera.main;
        draggable = GetComponent<Draggable_UI>();
    }
    void OnEnable()
    {
        draggable.onBeginDrag += HandleBeginDrag;
        draggable.onEndDrag += HandleEndDrag;
    }
    void OnDisable()
    {
        draggable.onBeginDrag += HandleBeginDrag;
        draggable.onEndDrag -= HandleEndDrag;
    }
    void HandleBeginDrag()
    {
        initPos = transform.position;
    }
    void Update()
    {
        if (draggable.m_isDragging)
        {
            Vector2 wrdPos = mainCam.ScreenToWorldPoint(transform.position);
            //实时检测位置是否有效
            Vector2Int gridPoint = GearManager.Instance.WorldPosToGridPoint(wrdPos);
            if (lastSnapGridPoint != gridPoint)
            {
                lastSnapGridPoint = gridPoint;
                //发布检测事件
                GearEvent.Call_OnValidateGridForGear(new GearPlaceArg()
                {
                    gearKey = gearData.m_gearKey,
                    snapPoint = gridPoint
                });
            }
        }
    }
    void HandleEndDrag(Vector2 scrPos)
    {
        GearEvent.Call_OnValidateGridForGear(new GearPlaceArg()
        {
            gearKey = string.Empty,
            snapPoint = Vector2Int.zero
        });
        if (gearData != null)
        {
            Vector2 wrdPos = mainCam.ScreenToWorldPoint(scrPos);
            Vector2Int gridPoint = GearManager.Instance.WorldPosToGridPoint(wrdPos);
            Vector2 snapPos = GearManager.Instance.GridPointToWorldPos(gridPoint);

            if (GearManager.Instance.IsGearPlaceableOnGridPoint(gearData.m_gearKey, gridPoint))
            {
                var gear = GearManager.Instance.CreateGear(gearData.m_gearKey, snapPos, 1);
                gear.RestartGear();
                Destroy(gameObject);
            }
            else
            {
                transform.position = initPos;
            }
        }
    }
}