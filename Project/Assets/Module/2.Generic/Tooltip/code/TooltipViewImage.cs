using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//代码手动设置提示图片的参数
public class TooltipViewImage : MonoBehaviour
{
    public void Init(TooltipContentArgs args, TooltipPosArgs posArgs)
    {
        foreach (string imageName in args.imageNameList)
        {
            GameObject objImage = new GameObject("image");
            objImage.transform.SetParent(transform);
            Image comp = objImage.AddComponent<Image>();
            GameAssetControl.AssignSpriteUI(imageName, comp);
        }

        //宽度自适应
        int width = 10;
        width += args.imageNameList.Count * 178;

        //宽度限制在屏幕宽度的80%
        width = Mathf.Min(width, (int)(Screen.width * 0.8f));

        //高度固定
        int height = 188;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);

        // 设置位置
        transform.position = TooltipManager.CalculateTooltipPos(posArgs, width, height);
        TooltipManager.ClampToScreen(rectTransform);

        //播放动画
        transform.localScale = Vector2.zero;
        transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
    }
}
