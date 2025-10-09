using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

//代码手动设置提示物品奖励的参数
public class TooltipViewItemInfo : MonoBehaviour
{
    [SerializeField] GameObject prefabArrow;
    [SerializeField] TextMeshProUGUI textItemSource;
    string itemName;

    public void Init(TooltipContentArgs args, TooltipPosArgs posArgs)
    {
        itemName = args.itemName;
        if (itemName.Contains("shard"))
        {
            textItemSource.text = UtilityLocalization.GetLocalization("page/gear/page_gear_obtain");
        }
        else
        {
            ItemData itemData = AllItem.dictData[itemName];
            //textItemSource.text = LocalizationUtility.GetLocalization(itemData.textSource);
        }

        RectTransform rectTransform = GetComponent<RectTransform>();
        // 设置位置
        transform.position = TooltipManager.CalculateTooltipPos(posArgs, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
        TooltipManager.ClampToScreen(rectTransform);

        //设置箭头位置
        GameObject objArrow = Instantiate(prefabArrow, transform);
        TooltipManager.CalculateArrowPos(posArgs, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, objArrow, transform.position);

        //播放动画
        transform.localScale = Vector2.zero;
        transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void OnJump()
    {
           
    }
}
