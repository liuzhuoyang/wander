using UnityEngine;
using PlayerInteraction;

public class UIFormationDragManager : MonoBehaviour
{
    [Header("拖拽预制体")]
   // public GameObject objDragPrefab; // 普通UI拖拽预制体
    public GameObject worldDragPrefab; // 世界层拖拽预制体

    private GameObject currentDrag;

    void Awake()
    {
        EventManager.StartListening<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_INIT_UI, OnInitUI);
        EventManager.StartListening<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG, OnCreateWorldDrag);
    }
    
    private void OnDestroy()
    {
        EventManager.StopListening<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_INIT_UI, OnInitUI);
        EventManager.StopListening<FormationDragArgs>(FormationDragEvent.EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG, OnCreateWorldDrag);
    }

    public void OnInitUI(FormationDragArgs args)
    {
        // 原有的UI初始化逻辑
    }

    public void OnCreateWorldDrag(FormationDragArgs args)
    {
        Debug.Log("[FormationDragManager] 创建世界层拖拽物件");

        if (worldDragPrefab == null)
        {
            Debug.LogError("[FormationDragManager] 世界层拖拽预制体未设置");
            args.onDragComplete?.Invoke(false);
            return;
        }

        if (args.itemConfig == null)
        {
            Debug.LogError("[FormationDragManager] 物品配置为空");
            args.onDragComplete?.Invoke(false);
            return;
        }

        // 创建世界层拖拽物件
        GameObject dragObject = Instantiate(worldDragPrefab, transform);
        
        // 获取FormationItemWorldDragHandlerUI组件
        var dragHandler = dragObject.GetComponent<FormationItemWorldDragHandlerUI>();
        if (dragHandler == null)
        {
            Debug.LogError("[FormationDragManager] 拖拽物件缺少FormationItemWorldDragHandlerUI组件");
            Destroy(dragObject);
            args.onDragComplete?.Invoke(false);
            return;
        }

        // 初始化拖拽物件
        dragHandler.Initialize(args.itemConfig, args.originalNode, args.onDragComplete);

        // 设置拖拽物件位置到鼠标位置
        var rectTransform = dragObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 将鼠标屏幕坐标转换为UI坐标
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform.parent as RectTransform,
                args.mousePosition,
                null, // 使用Screen Space - Overlay Canvas
                out localPoint
            );
            rectTransform.anchoredPosition = localPoint;
        }

        // 立即开始拖拽
        dragHandler.StartDrag(args.mousePosition);

        currentDrag = dragObject;
        Debug.Log($"[FormationDragManager] 成功创建世界层拖拽物件: {args.itemConfig.itemName}，位置: {args.mousePosition}");
    }
}


public class FormationDragArgs : UIBaseArgs
{
    public FormationItemConfig itemConfig; // 物品配置
    public FormationNode originalNode; // 原始节点
    public Vector2 mousePosition; // 鼠标位置
    public System.Action<bool> onDragComplete; // 拖拽完成回调
}

public static class FormationDragEvent
{
    public const string EVENT_FORMATION_DRAG_INIT_UI = "EVENT_FORMATION_DRAG_INIT_UI";
    public const string EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG = "EVENT_FORMATION_DRAG_CREATE_WORLD_DRAG";
}