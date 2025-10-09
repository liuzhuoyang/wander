using System;

public class DevIAPControl : IAPControl
{
    public override void Init()
    {

    }

    public override void OnPurchaseConsumable(string sku,string productID, Action callbackSucceed, Action callbackFailed)
    {
        MessageManager.Instance.OnLoading();
        StartCoroutine(TimerTick.StartRealtime(0.5f, () =>
        {
            MessageManager.Instance.CloseLoading();
            callbackSucceed?.Invoke();
        }));
    }

    public void OnPurchaseUnConsume(string productID, Action callbackSucceed, Action callbackFailed)
    {
        MessageManager.Instance.OnLoading();
        StartCoroutine(TimerTick.StartRealtime(0.5f, () =>
        {
            MessageManager.Instance.CloseLoading();
            callbackSucceed?.Invoke();
        }));
    }

    public override string GetLocalPriceString(string productID)
    {
        return AllIap.dictData[productID].sku.priceUSD.ToString();
    }

    public override void OnRestorePurchase()
    {
    }
}