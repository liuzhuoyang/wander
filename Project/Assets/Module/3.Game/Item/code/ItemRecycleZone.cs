using UnityEngine;

/// <summary>
/// 物品回收区域检测组件
/// 用于检测拖拽的物品是否在回收区域内
/// </summary>
public class ItemRecycleZone : MonoBehaviour
{
    [Header("回收区域设置")]
    [SerializeField] private RectTransform recycleArea; // 回收区域的RectTransform
    [SerializeField] private bool isActive = true; // 是否激活回收功能

    [Header("回收提示")]
    [SerializeField] private GameObject recycleHintUI; // 回收提示UI
    [SerializeField] private string recycleHintText = "回收物品";

    /// <summary>
    /// 回收区域是否激活
    /// </summary>
    public bool IsActive => isActive;

    /// <summary>
    /// 回收提示文本
    /// </summary>
    public string RecycleHintText => recycleHintText;

    void Awake()
    {
        // 如果没有设置回收区域，使用当前GameObject的RectTransform
        if (recycleArea == null)
        {
            recycleArea = GetComponent<RectTransform>();
        }

        // 初始化时隐藏提示UI
        if (recycleHintUI != null)
        {
            recycleHintUI.SetActive(false);
        }
    }

    /// <summary>
    /// 检测指定位置是否在回收区域内
    /// </summary>
    /// <param name="screenPosition">屏幕坐标</param>
    /// <returns>是否在回收区域内</returns>
    public bool IsPositionInRecycleZone(Vector2 screenPosition)
    {
        if (!isActive || recycleArea == null)
            return false;

        // 将屏幕坐标转换为本地坐标
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            recycleArea, 
            screenPosition, 
            null, // 使用Screen Space - Overlay Canvas
            out localPosition
        );

        // 检查是否在RectTransform的边界内
        return recycleArea.rect.Contains(localPosition);
    }

    /// <summary>
    /// 检测指定UI位置是否在回收区域内
    /// </summary>
    /// <param name="uiPosition">UI坐标</param>
    /// <returns>是否在回收区域内</returns>
    public bool IsUIPositionInRecycleZone(Vector2 uiPosition)
    {
        if (!isActive || recycleArea == null)
            return false;

        // 将UI坐标转换为本地坐标
        Vector2 localPosition = recycleArea.InverseTransformPoint(uiPosition);
        
        // 检查是否在RectTransform的边界内
        return recycleArea.rect.Contains(localPosition);
    }

    /// <summary>
    /// 显示回收提示
    /// </summary>
    public void ShowRecycleHint()
    {
        if (recycleHintUI != null)
        {
            recycleHintUI.SetActive(true);
        }
    }

    /// <summary>
    /// 隐藏回收提示
    /// </summary>
    public void HideRecycleHint()
    {
        if (recycleHintUI != null)
        {
            recycleHintUI.SetActive(false);
        }
    }

    /// <summary>
    /// 设置回收区域激活状态
    /// </summary>
    /// <param name="active">是否激活</param>
    public void SetActive(bool active)
    {
        isActive = active;
        
        if (!active)
        {
            HideRecycleHint();
        }
    }

    /// <summary>
    /// 获取回收区域的屏幕边界
    /// </summary>
    /// <returns>回收区域的屏幕边界</returns>
    public Rect GetRecycleZoneScreenBounds()
    {
        if (recycleArea == null)
            return Rect.zero;

        Vector3[] corners = new Vector3[4];
        recycleArea.GetWorldCorners(corners);

        // 转换为屏幕坐标
        Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    /// <summary>
    /// 在Scene视图中绘制回收区域边界（仅用于调试）
    /// </summary>
    void OnDrawGizmos()
    {
        if (recycleArea == null) return;

        Gizmos.color = isActive ? Color.red : Color.gray;
        
        // 获取世界坐标的四个角
        Vector3[] corners = new Vector3[4];
        recycleArea.GetWorldCorners(corners);

        // 绘制边界线
        for (int i = 0; i < 4; i++)
        {
            int next = (i + 1) % 4;
            Gizmos.DrawLine(corners[i], corners[next]);
        }
    }
}
