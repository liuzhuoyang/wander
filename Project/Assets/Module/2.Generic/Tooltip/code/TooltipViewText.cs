using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

//代码手动设置提示文本的参数
public class TooltipViewText : MonoBehaviour
{
    // public GameObject prefabText;
    public GameObject prefabArrow;
    [SerializeField] TextMeshProUGUI textContent;
    int marginY = 80; //边缘距离

    public void Init()
    {
        /*
        textContent.text = string.Join("\n", args.contentList);
        
        // 计算限宽
        textContent.ForceMeshUpdate();
        float preferredWidth = textContent.preferredWidth;
        float clampedWidth = Mathf.Clamp(preferredWidth, 100, 700);

        // 应用限宽
        var le = textContent.GetComponent<LayoutElement>();
        if (le != null) le.preferredWidth = clampedWidth;
        textContent.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clampedWidth);

        // 同帧强制刷新所有布局
        textContent.ForceMeshUpdate();
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textContent.rectTransform);

        var rectTransform = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        // 用 rect 而不是 sizeDelta 获取最终尺寸
        int rectWidth = (int)rectTransform.rect.width;
        int rectHeight = (int)rectTransform.rect.height;

        // 定位与约束
        transform.position = TooltipManager.CalculateTooltipPos(posArgs, rectWidth, rectHeight);
        TooltipManager.ClampToScreen(rectTransform);

        // 箭头
        var objArrow = Instantiate(prefabArrow, transform);
        TooltipManager.CalculateArrowPos(posArgs, rectWidth, rectHeight, objArrow, transform.position);

        // 动画
        transform.localScale = Vector2.zero;
        transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
        */

    }
}
