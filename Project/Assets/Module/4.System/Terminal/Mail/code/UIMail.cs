using UnityEngine;
public class UIMail : UIBase
{
    [SerializeField] MailDetail mailDetail;
    [SerializeField] MailView mailview;
    [SerializeField] GameObject objPinReceiveAll;
    [SerializeField] MailFunctionDetail mailFunctionDetail;
    void Awake()
    {
        EventManager.StartListening<MailListArgs>(EventNameMail.EVENT_MAIL_ON_INIT_UI, Init);
        EventManager.StartListening<MailListArgs>(EventNameMail.EVENT_MAIL_ON_REFRESH_UI, Refresh);
        EventManager.StartListening<MailArgs>(EventNameMail.EVENT_MAIL_OPEN_DETAIL_UI, OpenDetail);
        EventManager.StartListening<MailArgs>(EventNameMail.EVENT_MAIL_OPEN_FUNCTION_DETAIL_UI, OpenFunctionDetail);
    }
    void OnDestroy()
    {
        EventManager.StopListening<MailListArgs>(EventNameMail.EVENT_MAIL_ON_INIT_UI, Init);
        EventManager.StopListening<MailListArgs>(EventNameMail.EVENT_MAIL_ON_REFRESH_UI, Refresh);
        EventManager.StopListening<MailArgs>(EventNameMail.EVENT_MAIL_OPEN_DETAIL_UI, OpenDetail);
        EventManager.StopListening<MailArgs>(EventNameMail.EVENT_MAIL_OPEN_FUNCTION_DETAIL_UI, OpenFunctionDetail);
    }

    void Init(MailListArgs args)
    {
        mailFunctionDetail.gameObject.SetActive(false);
        mailDetail.gameObject.SetActive(false);
        mailview.Init(args.mailArgs);
    }
    void Refresh(MailListArgs args)
    {
        mailview.Init(args.mailArgs);
    }
    void OpenDetail(MailArgs args)
    {
        mailDetail.gameObject.SetActive(true);
        mailDetail.Open(args);
    }

    void OpenFunctionDetail(MailArgs args)
    {
        mailFunctionDetail.gameObject.SetActive(true);
        mailFunctionDetail.Open(args);
    }

    public void OnDelete()
    {
        MailSystem.Instance.OnDelete();
    }
    public void OnReceive()
    {
        MailSystem.Instance.OnReceive(-1);
    }

    public void OnClose()
    {
        base.CloseUI();
        TerminalSystem.Instance.Open();
    }
}