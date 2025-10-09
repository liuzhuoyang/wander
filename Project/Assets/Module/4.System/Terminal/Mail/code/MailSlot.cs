using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MailSlot : MonoBehaviour
{
    //邮件数据
    public TextMeshProUGUI mailTitle;
    public TextTimerHandler textTimer;
    public MailArgs mailArgs;
    public Image imgIcon1;
    public Image imgIcon2;
    public TextMeshProUGUI mailExpire;
    // public GameObject objPin;
    public void Init(MailArgs viewArgs)
    {
        mailArgs = viewArgs;
        mailTitle.text = viewArgs.title;
        CheckMailStatus();
    }

    void CheckMailStatus()
    {
        //判断是否到期
        int time = (int)(mailArgs.time - ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds());
        if (time > 0)
        {
            mailExpire.gameObject.SetActive(false);
            textTimer.gameObject.SetActive(true);
            textTimer.OnCount(time);
        }
        else
        {
            mailExpire.gameObject.SetActive(true);
            textTimer.gameObject.SetActive(false);
        }
        //判断邮件是否打开
        if (mailArgs.status == 0)
        {
            imgIcon1.gameObject.SetActive(true);
            imgIcon2.gameObject.SetActive(false);
        }
        else
        {
            imgIcon1.gameObject.SetActive(false);
            imgIcon2.gameObject.SetActive(true);
        }
    }

    //功能性邮件
    public void InitGMMail(MailArgs viewArgs)
    {
        mailArgs = viewArgs;
        mailTitle.text = UtilityLocalization.GetLocalization("page/mail/page_mail_title_gm_mail");
        CheckMailStatus();
    }
    public void OnOpenDetail()
    {
        MailSystem.Instance.OpenDetail(mailArgs);
    }
}
