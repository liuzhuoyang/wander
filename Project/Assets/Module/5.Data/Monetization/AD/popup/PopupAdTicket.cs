using UnityEngine;
using System;
using TMPro;

public class PopupAdTicketArgs : PopupArgs
{
    public Action onAd;
    public Action onTicket;
    public Action onClose;
}

public class PopupAdTicket : PopupBase
{
    [SerializeField] TextMeshProUGUI textTicket;

    PopupAdTicketArgs popupAdTicketArgs;

    public override void OnOpen<T>(T args)
    {
        base.OnOpen(args);
        popupAdTicketArgs = args as PopupAdTicketArgs;

        textTicket.text = $"({ItemSystem.Instance.GetItemNum(ConstantItem.TOKEN_TICKET_AD)})";
    }

    public void OnAds()
    {
        OnClose();
        popupAdTicketArgs.onAd?.Invoke();
    }

    public void OnTicket()
    {
        OnClose();
        popupAdTicketArgs.onTicket?.Invoke();
    }

    public void Close()
    {
        OnClose();
        popupAdTicketArgs.onClose?.Invoke();
    }
}
