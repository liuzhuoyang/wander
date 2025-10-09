using DG.Tweening;
using TMPro;
using UnityEngine;

//战力变化
public class PopupPowerChangeArgs : PopupArgs
{
    public int oldPower;
    public int newPower;
}

public class PopupPowerChange : PopupBase
{
    [SerializeField] TextMeshProUGUI textOldPower, textNewPower;
    [SerializeField] GameObject objMain;
    [SerializeField] GameObject objUp, objDown;
    float animationDuration = 1f; // 动画持续时间
    public override void OnOpen<T>(T args)
    {
        PopupPowerChangeArgs popupPowerChangeArgs = args as PopupPowerChangeArgs;
        //判断是否增加
        if (popupPowerChangeArgs.oldPower < popupPowerChangeArgs.newPower)
        {
            textNewPower.color = Color.green;
            objUp.SetActive(true);
            objDown.SetActive(false);
        }
        else
        {
            textNewPower.color = Color.red;
            objUp.SetActive(false);
            objDown.SetActive(true);
        }
        //赋值
        textOldPower.text = popupPowerChangeArgs.oldPower.ToString();
        UpdatePower(popupPowerChangeArgs.oldPower, popupPowerChangeArgs.newPower);
    }

    void UpdatePower(int oldPower, int newPower)
    {
        // 计算字体缩放
        float targetScale = newPower > oldPower ? 1.2f : 1.0f / 1.2f;

        // 动画化数值变化
        DOTween.To(() => oldPower, x =>
        {
            if (textNewPower == null) return; // 检查 textNewPower 是否为 null
            oldPower = x;
            textNewPower.text = oldPower.ToString();
        }, newPower, animationDuration)
        .OnUpdate(() =>
        {
            if (textNewPower == null) return; // 检查 textNewPower 是否为 null
            // 计算进度
            float progress = Mathf.Abs((float)(oldPower - newPower) / (newPower - oldPower));
            if (float.IsNaN(progress) || float.IsInfinity(progress))
            {
                progress = 0; // 防止 NaN 或 Infinity
            }

            // 动画过程中调整字体大小
            textNewPower.transform.localScale = Vector3.one * Mathf.Lerp(1.0f, targetScale, progress);
        })
        .OnComplete(() =>
        {
            if (textNewPower == null) return; // 检查 textNewPower 是否为 null
            // 动画结束时恢复原始字体大小
            textNewPower.transform.localScale = Vector3.one;
            AnimateAndDestroy();
        }).OnKill(() =>
        {
            // 在动画被终止时执行清理操作
            if (textNewPower != null)
            {
                textNewPower.transform.localScale = Vector3.one;
            }
        });
    }

    void AnimateAndDestroy()
    {
        if (objMain == null) return; // 检查 objMain 是否为 null

        // 获取 objMain 的 RectTransform
        RectTransform rectTransform = objMain.GetComponent<RectTransform>();
        if (rectTransform == null) return; // 检查 rectTransform 是否为 null

        // 动画化位置向上移动
        rectTransform.DOMoveY(rectTransform.position.y + 200, animationDuration).SetEase(Ease.OutQuad)
        .OnKill(() =>
        {
            // 在动画被终止时执行清理操作
            if (rectTransform != null)
            {
                rectTransform.DOKill();
            }
        });

        // 动画化渐隐
        CanvasGroup canvasGroup = objMain.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = objMain.AddComponent<CanvasGroup>();
        }
        canvasGroup.DOFade(0, animationDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (rectTransform != null)
            {
                // 确保在销毁前终止动画
                rectTransform.DOKill();
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        });
    }

    public override void OnClose()
    {
        // 检查 objMain 是否为 null
        if (objMain != null)
        {
            // 停止 objMain 上的所有 DOTween 动画
            objMain.transform.DOKill();

            // 如果有 CanvasGroup，停止其上的动画
            CanvasGroup canvasGroup = objMain.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.DOKill();
            }
        }

        // 停止所有与 textNewPower 相关的动画
        if (textNewPower != null)
        {
            textNewPower.transform.DOKill();
        }

        // 其他清理操作
        // 例如，隐藏或销毁对象
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}

