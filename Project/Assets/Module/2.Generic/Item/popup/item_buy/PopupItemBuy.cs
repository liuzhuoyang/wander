using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupItemBuyArgs : PopupArgs
{
    public string rewardItemName;
    public int rewardItemCount;
    public string costItemName;
    public int costItemCount;
    public int limitCount;
    public Action<int> action;
}

public class PopupItemBuy : PopupBase
{
    [SerializeField] ItemViewSlot itemViewSlot;
    [SerializeField] TextMeshProUGUI textHaveCount, textBuyCount, textPrice;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image imgCost;
    PopupItemBuyArgs popupBuyArgs;

    int buyCount;
    int haveCount;
    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        buyCount = 1;
        popupBuyArgs = args as PopupItemBuyArgs;
        itemViewSlot.Init(popupBuyArgs.rewardItemName, popupBuyArgs.rewardItemCount);//, GameData.allItem.dictItemData[popupBuyArgs.rewardItemName].rarity);
        textHaveCount.text = UtilityLocalization.GetLocalization("popup/popup_buy_item_count", ItemSystem.Instance.GetItemNum(popupBuyArgs.rewardItemName).ToString());
        GameAssetControl.AssignIcon(popupBuyArgs.costItemName, imgCost);
        haveCount = ItemSystem.Instance.GetItemNum(popupBuyArgs.costItemName);

        RefreshPrice();
    }

    void RefreshPrice()
    {
        textBuyCount.text = buyCount.ToString();
        textPrice.text = UtilityTextFormat.GetNumColor(popupBuyArgs.costItemCount * buyCount, haveCount);
        StartCoroutine(RefreshLayoutNextFrame());
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    public void OnReduce()
    {
        if (buyCount <= 1)
        {
            return;
        }
        --buyCount;
        RefreshPrice();
    }

    public void OnAdd()
    {
        if (buyCount >= popupBuyArgs.limitCount)
        {
            TipManager.Instance.OnTip(UtilityLocalization.GetLocalization("popup/popup_buy_item_limit"));
            return;
        }
        ++buyCount;
        RefreshPrice();
    }

    public void OnMax()
    {
        buyCount = popupBuyArgs.limitCount;
        RefreshPrice();
    }

    public void OnClickConfirm()
    {
        //购买
        ItemSystem.Instance.UseItem(popupBuyArgs.costItemName, popupBuyArgs.costItemCount * buyCount, () =>
        {
            
        });
    }
}