using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//代码手动设置提示物品奖励的参数
public class TooltipViewItemReward : MonoBehaviour
{
    [SerializeField] GameObject prefabArrow;
    public GameObject prefabItemSlot;

    public void Init(TooltipContentArgs args, TooltipPosArgs posArgs)
    {
        foreach (RewardArgs reward in args.itemRewardList)
        {
            GameObject objItemSlot = Instantiate(prefabItemSlot, transform);
            ItemData itemArgs = AllItem.dictData[reward.reward];
            objItemSlot.GetComponent<ItemViewSlot>().Init(reward.reward, reward.num);//, itemArgs.rarity);
        }

        //宽度自适应
        int width = 70;
        width += args.itemRewardList.Count * 120;

        //宽度限制在屏幕宽度的80%
        width = Mathf.Min(width, (int)(Screen.width * 0.8f));

        //高度固定
        int height = 188;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);

        // 设置位置
        transform.position = TooltipManager.CalculateTooltipPos(posArgs, width, height);
        TooltipManager.ClampToScreen(rectTransform);

        //设置箭头位置
        GameObject objArrow = Instantiate(prefabArrow, transform);
        TooltipManager.CalculateArrowPos(posArgs, (int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y, objArrow, transform.position);

        //播放动画
        transform.localScale = Vector2.zero;
        transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetUpdate(true);
    }
}
