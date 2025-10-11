using System.Collections;
using UnityEngine;
/// <summary>
/// 气泡
/// </summary>
public class BubbleSlot : MonoBehaviour
{
    [SerializeField] GameObject objMain;
    [SerializeField] RectTransform rectTransform;
    private Vector2 targetPos;
    private Rect safeArea;  // 屏幕安全区域（防止指示器被灵动岛遮挡）
    private float duration;
    private bool isVisible = true;

    public void Initialize(Vector2 targetPos, Vector2 playerPos, float duration)
    {
        this.duration = duration;
        this.targetPos = targetPos;
        SetSafeArea();
        UpdateIndicatorPosition(targetPos, playerPos);
        StartCoroutine(AutoDestroy());
    }

    // public void Initialize(GameObject targetObj, Vector3 playerPos)
    // {
    //     this.playerPos = playerPos;
    //     targetObject = targetObj.transform;
    //     SetSafeArea();
    // }

    private void Update()
    {
        if (!isVisible) return;
        // 检查目标是否在屏幕内
        if (CameraManager.Instance.IsPointOnScreen(targetPos))
        {
            objMain.SetActive(false);
        }
        else
        {
            objMain.SetActive(true);
        }
    }

    private void SetSafeArea()
    {
        safeArea = new Rect(100f, 300f, Screen.width - 200f, Screen.height - 600f);
    }

    private void UpdateIndicatorPosition(Vector2 targetPos, Vector2 playerPos)
    {
        // 将世界坐标转换为屏幕坐标
        Vector3 worldPos = new Vector3(targetPos.x, targetPos.y, 0);
        Vector3 screenPosition = CameraManager.Instance.WorldToScreenPos(worldPos);

        // 确保指示器在 Safe Area 之内
        bool isOffScreen = screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height;

        if (isOffScreen)
        {
            //确保指示器在safeArea内
            screenPosition.x = Mathf.Clamp(screenPosition.x, safeArea.xMin, safeArea.xMax);
            screenPosition.y = Mathf.Clamp(screenPosition.y, safeArea.yMin, safeArea.yMax);
            // 设置指示器整体位置
            gameObject.transform.position = screenPosition;

            // 计算方向向量（从玩家到目标）
            Vector2 direction = (targetPos - playerPos).normalized;

            // 计算旋转角度（考虑屏幕坐标系）
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 设置箭头旋转 ui中朝向向上
            rectTransform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            objMain.SetActive(false);
        }
    }

    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(duration);
        isVisible = false;
        BubbleSystem.Instance.RemoveBubble(this);
    }
}