using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupIAPConfirmArgs : PopupArgs
{
    public string textIapTitle;
    public List<RewardArgs> listReward;
    public IAPData iapData;
    public Action onComfirm;
}

public class PopupIAPConfirm : PopupBase
{
    [SerializeField] TextMeshProUGUI textPrice;
    [SerializeField] TextMeshProUGUI textIapTitle, textRewardInfo;

    [SerializeField] GameObject objReward;
    [SerializeField] Transform containerReward;

    PopupIAPConfirmArgs popupIAPConfirmArgs;
    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        popupIAPConfirmArgs = args as PopupIAPConfirmArgs;

        textPrice.text = IAPControl.Instance.GetLocalPriceString(popupIAPConfirmArgs.iapData.productID);
        textIapTitle.text = popupIAPConfirmArgs.textIapTitle;
        textRewardInfo.text = string.Format(UtilityLocalization.GetLocalization("popup/popup_iap_purchase_info"), popupIAPConfirmArgs.textIapTitle);

        foreach (Transform child in containerReward)
        {
            Destroy(child.gameObject);
        }

        foreach (var reward in popupIAPConfirmArgs.listReward)
        {
            GameObject obj = Instantiate(objReward, containerReward);
            obj.GetComponent<RewardViewSlot>().Init(reward);
        }
    }

    public void OnClickConfirm()
    {
        string sku = popupIAPConfirmArgs.iapData.sku.skuID;
        string productID = popupIAPConfirmArgs.iapData.productID;
        IAPControl.Instance.OnPurchaseConsumable(sku, productID, () =>
        {
            popupIAPConfirmArgs.onComfirm?.Invoke();
            OnClose();
        }, null);
    }
}
