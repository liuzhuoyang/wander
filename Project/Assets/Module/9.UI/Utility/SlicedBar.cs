using UnityEngine;
using DG.Tweening;
using System;

public class SlicedBar : MonoBehaviour
{
    public RectTransform rectBar;
    public void OnSetFill(float progress)
    {
        // 获取当前的 anchorMin 和 anchorMax
        Vector2 currentAnchorMin = rectBar.anchorMin;
        Vector2 currentAnchorMax = rectBar.anchorMax;

        // 只修改 anchorMax 的 x 值，而保持 y 值不变
        float targetProgress = progress; // progression 是你的进度值 (0.0 ~ 1.0)
        //rectBar.DOAnchorMax(new Vector2(targetProgress, currentAnchorMax.y), duration).SetEase(Ease.Linear);
        rectBar.anchorMax = new Vector2(targetProgress, currentAnchorMax.y);
    }

    public void OnSetFill(float progress, float duration, bool isReset = false, Action onComplete = null, bool isFlip = false)
    {
        rectBar.DOKill(); // 先终止当前的动画
        float adjustedProgress = Mathf.Clamp01(progress);

        if (isReset)
        {
            if (isFlip)
            {
                rectBar.anchorMin = new Vector2(1, rectBar.anchorMin.y); // 右侧
                rectBar.anchorMax = new Vector2(1, rectBar.anchorMax.y); // 右侧
            }
            else
            {
                rectBar.anchorMin = new Vector2(0, rectBar.anchorMin.y); // 左侧
                rectBar.anchorMax = new Vector2(0, rectBar.anchorMax.y); // 左侧
            }
        }

        if (isFlip)
        {
            rectBar.DOAnchorMin(new Vector2(1 - adjustedProgress, rectBar.anchorMin.y), duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke());

            // 确保 anchorMax 不冲突
            rectBar.anchorMax = new Vector2(1, rectBar.anchorMax.y);
        }
        else
        {
            rectBar.DOAnchorMax(new Vector2(adjustedProgress, rectBar.anchorMax.y), duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => onComplete?.Invoke());

            // 确保 anchorMin 不冲突
            rectBar.anchorMin = new Vector2(0, rectBar.anchorMin.y);
        }
    }
}
